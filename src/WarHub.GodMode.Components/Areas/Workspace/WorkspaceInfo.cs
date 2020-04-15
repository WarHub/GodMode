using System;
using Amadevus.RecordGenerator;

namespace WarHub.GodMode.Components.Areas.Workspace
{
    [Record(Features.Default | Features.Equality)]
    public sealed partial class WorkspaceInfo
    {
        private string repository;
        private string appRoute;

        public WorkspaceType Type { get; }

        public string LocalFsPath { get; }

        public Uri GitHubUrl { get; }

        public string GitHubRepository => repository ??= string.Concat(GitHubUrl?.Segments[^2..]);

        public string AppRoute => appRoute ??= Type switch
        {
            WorkspaceType.GitHub => $"/gh/{GitHubRepository}",
            WorkspaceType.LocalFilesystem => "/",
            _ => throw new InvalidOperationException()
        };

        public static WorkspaceInfo CreateLocalFs(string path)
            => new WorkspaceInfo(WorkspaceType.LocalFilesystem, path, null);

        public static WorkspaceInfo CreateGitHub(Uri gitHubUrl)
            => new WorkspaceInfo(WorkspaceType.GitHub, null, gitHubUrl);
    }
}
