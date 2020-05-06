using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using WarHub.ArmouryModel.ProjectModel;
using WarHub.ArmouryModel.Source;
using WarHub.ArmouryModel.Workspaces.BattleScribe;
using WarHub.GodMode.Components.Areas.Workspace;

namespace WarHub.GodMode.GithubPages.Services
{
    public class GitHubWorkspaceProvider : IWorkspaceProvider
    {
        private const string InteropNamespace = "ghpagesInterop";
        private readonly IJSRuntime js;
        private readonly JsMemoryInteropService memoryInterop;
        private readonly InteropMemFileSystem memFs;

        public GitHubWorkspaceProvider(IJSRuntime js, JsMemoryInteropService memoryInterop)
        {
            this.js = js;
            this.memoryInterop = memoryInterop;
            memFs = new InteropMemFileSystem(ReadFile);
        }

        // TODO cache -> IndexedDB
        private Dictionary<Uri, IWorkspace> Cache { get; }
            = new Dictionary<Uri, IWorkspace>();

        public async Task<IWorkspace> GetWorkspace(WorkspaceInfo info)
        {
            if (info is { Type: WorkspaceType.GitHub, GitHubUrl: { } })
            {
                if (Cache.TryGetValue(info.GitHubUrl, out var cached))
                {
                    return cached;
                }
                var newWorkspace = await GetWorkspaceCore(info);
                return Cache[info.GitHubUrl] = newWorkspace;
            }
            return null;
        }

        private async Task<IWorkspace> GetWorkspaceCore(WorkspaceInfo info)
        {
            bool clone = false;
            try
            {
                var dbfs = GetFsRoot(info);
                var files = await ListDir(dbfs);
                //clone = true;
                Console.WriteLine($"Read dir {dbfs}");
            }
            catch (Exception)
            {
                // interop/IndexedDB doesn't contain, clone again
                clone = true;
            }
            return await Task.Run(async () =>
            {
                if (clone)
                {
                    await CloneRepo(info);
                }
                return await LoadWorkspaceFromFs(info);
            });
        }

        private async Task<IWorkspace> LoadWorkspaceFromFs(WorkspaceInfo info)
        {
            var root = GetFsRoot(info);
            var rootFiles = await ListDir(root);
            //var datafiles = ReadDatafiles(root, rootFiles);
            var datafiles = await ReadDatafilesAsync(root, rootFiles).ToListAsync();
            return new InMemoryWorkspace(root, datafiles.ToImmutableArray());

            IEnumerable<IDatafileInfo> ReadDatafiles(string root, string[] filenames)
            {
                foreach (var filename in filenames)
                {
                    yield return ReadDatafile(root, filename);
                }
            }

            IDatafileInfo ReadDatafile(string root, string filename)
            {
                var filepath = $"{root}/{filename}";
                var kind = filepath.GetXmlDocumentKind();
                if (kind == XmlDocumentKind.Catalogue)
                {
                    return new LazyWeakXmlDatafileInfo<CatalogueNode>(filepath, SourceKind.Catalogue, memFs);
                }
                else if (kind == XmlDocumentKind.Gamesystem)
                {
                    return new LazyWeakXmlDatafileInfo<GamesystemNode>(filepath, SourceKind.Gamesystem, memFs);
                }
                else
                {
                    return new UnknownTypeDatafileInfo(filename);
                }
            }

            async IAsyncEnumerable<IDatafileInfo> ReadDatafilesAsync(string root, string[] filenames)
            {
                foreach (var filename in filenames)
                {
                    yield return await ReadDatafileAsync(root, filename);
                }
            }

            async Task<IDatafileInfo> ReadDatafileAsync(string root, string filename)
            {
                var filepath = $"{root}/{filename}";
                var kind = filepath.GetXmlDocumentKind();
                if (kind == XmlDocumentKind.Catalogue || kind == XmlDocumentKind.Gamesystem)
                {
                    using var filestream = await ReadFile(filepath);
                    return filestream.LoadSourceAuto(filepath);
                }
                else
                {
                    return new UnknownTypeDatafileInfo(filename);
                }
            }
        }

        private string GetFsRoot(WorkspaceInfo info) => $"/gh/{info.GitHubRepository}";

        private async Task<CloneResult> CloneRepo(WorkspaceInfo info)
        {
            var repo = info.GitHubUrl;
            var fsRoot = GetFsRoot(info);
            return await js.InvokeAsync<CloneResult>(InteropNamespace + ".gitClone", repo, fsRoot);
        }

        private async Task<string[]> ListDir(string path)
        {
            return await js.InvokeAsync<string[]>("pfs.readdir", path);
        }

        private async Task<Stream> ReadFile(string path)
        {
            const string ReadFileName = InteropNamespace + ".fsReadFileMemRef";
            return await memoryInterop.OpenReadAsync(
                async (script) => await js.InvokeAsync<int>(ReadFileName, path, script));
        }

        private class CloneResult
        {
            public string Root { get; set; }
            public string[] RootEntries { get; set; }
        }

        private class InMemoryWorkspace : IWorkspace
        {
            public InMemoryWorkspace(string rootPath, ImmutableArray<IDatafileInfo> datafiles)
            {
                RootPath = rootPath;
                Datafiles = datafiles;
            }

            public string RootPath { get; }

            public ImmutableArray<IDatafileInfo> Datafiles { get; }

            public ProjectConfigurationInfo Info { get; } = new ProjectConfigurationInfo.Builder
            {
                Filepath = ProjectConfiguration.FileExtension,
                Configuration = new ProjectConfiguration.Builder
                {
                    FormatProvider = ProjectFormatProviderType.BattleScribeXml,
                    OutputPath = ".",
                    SourceDirectories = ImmutableArray<SourceFolder>.Empty,
                    ToolsetVersion = ProjectToolset.Version
                }.ToImmutable()
            }.ToImmutable();
        }

        private class UnknownTypeDatafileInfo : IDatafileInfo
        {
            public UnknownTypeDatafileInfo(string filepath)
            {
                Filepath = filepath;
            }

            public string Filepath { get; }

            public SourceKind DataKind => SourceKind.Unknown;

            public SourceNode GetData() => null;

            public string GetStorageName() => Path.GetFileNameWithoutExtension(Filepath);
        }

        private class LazyWeakXmlDatafileInfo<TData> : IDatafileInfo<TData> where TData : SourceNode
        {
            private readonly IFileSystem fs;

            public LazyWeakXmlDatafileInfo(string path, SourceKind dataKind, IFileSystem fs)
            {
                Filepath = path;
                DataKind = dataKind;
                this.fs = fs;
            }

            public string Filepath { get; }

            public TData Data => GetData();

            public SourceKind DataKind { get; }

            private WeakReference<TData> WeakData { get; } = new WeakReference<TData>(null);

            public TData GetData()
            {
                if (WeakData.TryGetTarget(out var cached))
                {
                    return cached;
                }
                var data = ReadFile();
                WeakData.SetTarget(data);
                return data;
            }

            public string GetStorageName() => Path.GetFileNameWithoutExtension(Filepath);

            SourceNode IDatafileInfo.GetData() => GetData();

            private TData ReadFile()
            {
                using (var filestream = fs.OpenRead(Filepath))
                {
                    var node = XmlFileExtensions.ZippedExtensions.Contains(Path.GetExtension(Filepath))
                        ? filestream.LoadSourceZipped(DataKind.GetXmlDocumentKindOrUnknown())
                        : filestream.LoadSource(DataKind.GetXmlDocumentKindOrUnknown());
                    return (TData)node;
                }
            }
        }

        private interface IFileSystem
        {
            Stream OpenRead(string path);
        }

        private class InteropMemFileSystem : IFileSystem
        {
            private readonly Func<string, Task<Stream>> asyncOpen;

            public InteropMemFileSystem(Func<string, Task<Stream>> asyncOpen)
            {
                this.asyncOpen = asyncOpen;
            }

            public Stream OpenRead(string path)
            {
                var task = asyncOpen(path);
                if (task.IsCompleted)
                {
                    return task.Result;
                }
                else
                {
                    while (!task.IsCompleted)
                    {
                        Thread.Sleep(100);
                    }
                    return task.Result;
                }
            }
        }
    }
}
