using System.Collections.Immutable;
using Microsoft.JSInterop;
using WarHub.ArmouryModel.ProjectModel;
using WarHub.ArmouryModel.Source;
using WarHub.ArmouryModel.Workspaces.BattleScribe;
using WarHub.GodMode.Components.Areas.Workspace;

namespace WarHub.GodMode.GithubPages.Services;

public class GitHubWorkspaceProvider : IWorkspaceProvider
{
    private const string InteropNamespace = "ghpagesInterop";
    private readonly IJSRuntime js;

    public GitHubWorkspaceProvider(IJSRuntime js)
    {
        this.js = js;
    }

    private Dictionary<Uri, IWorkspace> Cache { get; } = [];

    public async Task<IWorkspace?> GetWorkspace(WorkspaceInfo info)
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
        var clone = false;
        try
        {
            var dbfs = GetFsRoot(info);
            await ListDir(dbfs);
            Console.WriteLine($"Read dir {dbfs}");
        }
        catch (Exception)
        {
            clone = true;
        }
        if (clone)
        {
            await CloneRepo(info);
        }
        return await LoadWorkspaceFromFs(info);
    }

    private async Task<IWorkspace> LoadWorkspaceFromFs(WorkspaceInfo info)
    {
        var root = GetFsRoot(info);
        var rootFiles = await ListDir(root);
        var datafiles = await ReadDatafilesAsync(root, rootFiles).ToListAsync();
        return new InMemoryWorkspace(root, datafiles.ToImmutableArray());

        async IAsyncEnumerable<IDatafileInfo> ReadDatafilesAsync(string dir, string[] filenames)
        {
            foreach (var filename in filenames)
            {
                yield return await ReadDatafileAsync(dir, filename);
            }
        }

        async Task<IDatafileInfo> ReadDatafileAsync(string dir, string filename)
        {
            var filepath = $"{dir}/{filename}";
            var kind = filepath.GetXmlDocumentKind();
            if (kind is XmlDocumentKind.Catalogue or XmlDocumentKind.Gamesystem)
            {
                using var filestream = await ReadFile(filepath);
                var node = filestream.LoadSourceAuto(filepath);
                if (node is not null)
                    return DatafileInfo.Create(filepath, node);
            }
            return new UnknownTypeDatafileInfo(filename);
        }
    }

    private static string GetFsRoot(WorkspaceInfo info) => $"/gh/{info.GitHubRepository}";

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
        var bytes = await js.InvokeAsync<byte[]>(InteropNamespace + ".fsReadFile", path);
        return new MemoryStream(bytes);
    }

    private class CloneResult
    {
        public string? Root { get; set; }
        public string[]? RootEntries { get; set; }
    }

    private record InMemoryWorkspace(string RootPath, ImmutableArray<IDatafileInfo> Datafiles) : IWorkspace;
}
