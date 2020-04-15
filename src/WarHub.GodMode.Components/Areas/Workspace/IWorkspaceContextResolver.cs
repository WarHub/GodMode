using System.Threading.Tasks;
using WarHub.ArmouryModel.ProjectModel;
using WarHub.ArmouryModel.Source;
using WarHub.GodMode.SourceAnalysis;

namespace WarHub.GodMode.Components.Areas.Workspace
{
    public interface IWorkspaceContextResolver
    {
        Task<GamesystemContext> GetContext(IWorkspace workspace, CatalogueBaseNode root);
    }
}
