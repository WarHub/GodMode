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
        public WorkspaceType Type { get; }

        public Uri GitHubUrl { get; }

        public static WorkspaceInfo LocalFs { get; } = new WorkspaceInfo(WorkspaceType.LocalFilesystem, null);
        public static WorkspaceInfo CreateGitHub(Uri gitHubUrl) => new WorkspaceInfo(WorkspaceType.GitHub, gitHubUrl);
    }
}
