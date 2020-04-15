using System.Collections.Generic;
using System.Threading.Tasks;
using WarHub.ArmouryModel.ProjectModel;

namespace WarHub.GodMode.Components.Areas.Workspace
{
    /// <summary>
    /// Provides selection of workspace providers.
    /// </summary>
    public interface IWorkspaceProviderAggregate
    {
        IEnumerable<WorkspaceProviderInfo> ProviderInfos { get; }

        WorkspaceProviderInfo this[WorkspaceType type] { get; }

        Task<IWorkspace> GetWorkspace(WorkspaceInfo workspaceInfo);
    }
}
