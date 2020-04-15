using System;
using Amadevus.RecordGenerator;

namespace WarHub.GodMode.Data
{
    public enum WorkspaceType
    {
        LocalFilesystem,
        GitHub
    }

    [Record(Features.Default | Features.Equality)]
    public sealed partial class WorkspaceInfo
    {
        private string repository;
        private string appRoute;

        public WorkspaceType Type { get; }

        public Uri GitHubUrl { get; }

        public string GitHubRepository => repository ??= string.Concat(GitHubUrl?.Segments[^2..]);

        public string AppRoute => appRoute ??= Type switch
        {
            WorkspaceType.GitHub => $"/gh/{GitHubRepository}",
            WorkspaceType.LocalFilesystem => "/",
            _ => throw new InvalidOperationException()
        };

        public static WorkspaceInfo LocalFs { get; } = new WorkspaceInfo(WorkspaceType.LocalFilesystem, null);
        public static WorkspaceInfo CreateGitHub(Uri gitHubUrl) => new WorkspaceInfo(WorkspaceType.GitHub, gitHubUrl);
    }
}
