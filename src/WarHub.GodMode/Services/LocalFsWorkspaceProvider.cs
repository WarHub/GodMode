using WarHub.ArmouryModel.ProjectModel;
using WarHub.ArmouryModel.Workspaces.BattleScribe;
using WarHub.GodMode.Components.Areas.Workspace;

namespace WarHub.GodMode.Services;

public class LocalFsWorkspaceProvider : IWorkspaceProvider
{
    private readonly IWorkspace workspace;

    public LocalFsWorkspaceProvider(IConfiguration configuration)
    {
        var dataDir = configuration["DataDir"] ?? Directory.GetCurrentDirectory();
        workspace = XmlWorkspace.CreateFromDirectory(dataDir);

        ProviderInfo = new WorkspaceProviderInfo(
            WorkspaceType.LocalFilesystem,
            WorkspaceInfo.CreateLocalFs(Path.GetFullPath(workspace.RootPath)),
            this);
    }

    public WorkspaceProviderInfo ProviderInfo { get; }

    public Task<IWorkspace> GetWorkspace(WorkspaceInfo info)
    {
        return Task.FromResult(Core())!;

        IWorkspace? Core()
        {
            if (info is { Type: WorkspaceType.LocalFilesystem })
            {
                return workspace;
            }
            return null;
        }
    }
}
