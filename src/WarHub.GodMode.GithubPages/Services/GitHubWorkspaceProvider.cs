using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using WarHub.ArmouryModel.ProjectModel;
using WarHub.ArmouryModel.Source;
using WarHub.ArmouryModel.Workspaces.BattleScribe;
using WarHub.GodMode.Components.Areas.Workspace;

namespace WarHub.GodMode.GithubPages.Services
{
    public class GitHubWorkspaceProvider : IWorkspaceProvider
    {
        public GitHubWorkspaceProvider(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        private HttpClient HttpClient { get; }

        // TODO cache -> IndexedDB
        private Dictionary<Uri, IWorkspace> Cache { get; }
            = new Dictionary<Uri, IWorkspace>();

        public async Task<IWorkspace> GetWorkspace(WorkspaceInfo info)
        {
            if (info is { Type: WorkspaceType.GitHub, GitHubUrl: { } })
            {
                return await GetWorkspaceCore(info);
            }
            return null;
        }

        public async Task<IWorkspace> GetWorkspaceCore(WorkspaceInfo info)
        {
            if (Cache.TryGetValue(info.GitHubUrl, out var cached))
            {
                return cached;
            }
            return await Task.Run(async () =>
            {
                var repoPath = string.Concat(info.GitHubUrl.Segments[^2..]);
                var repoUri = info.GitHubUrl + "/archive/master.zip";
                var response = await HttpClient.GetAsync(repoUri);
                response.EnsureSuccessStatusCode();
                using var zipStream = await response.Content.ReadAsStreamAsync();
                using var zip = new ZipArchive(zipStream);
                var datafiles = GetDatafiles(zip).ToImmutableArray();
                var workspace = new InMemoryWorkspace(info.GitHubUrl.ToString(), datafiles);
                return Cache[info.GitHubUrl] = workspace;
            });

            IEnumerable<IDatafileInfo> GetDatafiles(ZipArchive zip)
            {
                foreach (var entry in zip.Entries)
                {
                    using var entryStream = entry.Open();
                    var file = entry.Name.GetXmlDocumentKind() switch
                    {
                        XmlDocumentKind.Gamesystem => entryStream.LoadSourceAuto(entry.Name),
                        XmlDocumentKind.Catalogue => entryStream.LoadSourceAuto(entry.Name),
                        _ => (IDatafileInfo)new UnknownTypeDatafileInfo(entry.Name)
                    };
                    if (file is { })
                    {
                        yield return file;
                    }
                }
            }
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
    }
}
