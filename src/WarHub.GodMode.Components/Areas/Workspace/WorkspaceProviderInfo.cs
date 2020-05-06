using Amadevus.RecordGenerator;

namespace WarHub.GodMode.Components.Areas.Workspace
{
    [Record]
    public partial class WorkspaceProviderInfo
    {
        public WorkspaceType Type { get; }

        public WorkspaceInfo InitialWorkspaceInfo { get; }

        public IWorkspaceProvider Provider { get; }
    }
}
