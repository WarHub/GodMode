using System.Threading.Tasks;
using WarHub.ArmouryModel.ProjectModel;

namespace WarHub.GodMode.Components.Areas.Workspace
{
    /// <summary>
    /// Given workspace info, loads requested workspace.
    /// </summary>
    public interface IWorkspaceProvider
    {
        Task<IWorkspace> GetWorkspace(WorkspaceInfo info);
    }
}
