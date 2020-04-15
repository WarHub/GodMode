using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using WarHub.ArmouryModel.ProjectModel;
using WarHub.ArmouryModel.Workspaces.BattleScribe;
using WarHub.GodMode.Components.Areas.Workspace;

namespace WarHub.GodMode.Services
{
    public class GitHubWorkspaceProvider : IWorkspaceProvider
    {
        public GitHubWorkspaceProvider(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
        }

        private IHttpClientFactory HttpClientFactory { get; }

        private Dictionary<Uri, (string path, WeakReference<IWorkspace> workspaceRef)> Cache { get; }
            = new Dictionary<Uri, (string path, WeakReference<IWorkspace> workspaceRef)>();

        public async Task<IWorkspace> GetWorkspace(WorkspaceInfo info)
        {
            if (info is { Type: WorkspaceType.GitHub, GitHubUrl: { } })
            {
                return await GetWorkspaceCore(info.GitHubUrl);
            }
            return null;
        }

        public async Task<IWorkspace> GetWorkspaceCore(Uri repository)
        {
            if (Cache.TryGetValue(repository, out var cached))
            {
                if (cached.workspaceRef.TryGetTarget(out var cachedWorkspace))
                {
                    return cachedWorkspace;
                }
                return CreateAndSaveToCache(cached.path);
            }
            return await Task.Run(async () =>
            {
                var repoPath = string.Concat(repository.Segments[^2..]);
                var extractDirectory = Path.Join(Path.GetTempPath(), "godmode", "download", repoPath);
                if (Directory.Exists(extractDirectory))
                {
                    Directory.Delete(extractDirectory, recursive: true);
                }
                var repoUri = repository + "/archive/master.zip";
                var response = await HttpClientFactory.CreateClient().GetAsync(repoUri);
                response.EnsureSuccessStatusCode();
                using var zipStream = await response.Content.ReadAsStreamAsync();
                using var zip = new ZipArchive(zipStream);
                Directory.CreateDirectory(extractDirectory);
                zip.ExtractToDirectory(extractDirectory, overwriteFiles: true);
                var rootPath = Directory.GetDirectories(extractDirectory)[0];
                return CreateAndSaveToCache(rootPath);
            });

            IWorkspace CreateAndSaveToCache(string rootPath)
            {
                var workspace = XmlWorkspace.CreateFromDirectory(rootPath);
                Cache[repository] = (rootPath, new WeakReference<IWorkspace>(workspace));
                return workspace;
            }
        }
    }
}
