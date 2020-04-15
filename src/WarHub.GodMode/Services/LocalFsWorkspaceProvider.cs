using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WarHub.ArmouryModel.ProjectModel;
using WarHub.ArmouryModel.Workspaces.BattleScribe;
using WarHub.GodMode.Components.Areas.Workspace;

namespace WarHub.GodMode.Services
{
    public class LocalFsWorkspaceProvider : IWorkspaceProvider
    {
        private readonly string dataDir;
        private readonly IWorkspace workspace;

        public LocalFsWorkspaceProvider(IConfiguration configuration)
        {
            dataDir = configuration["DataDir"] ?? Directory.GetCurrentDirectory();
            workspace = XmlWorkspace.CreateFromDirectory(dataDir);

            ProviderInfo = new WorkspaceProviderInfo.Builder
            {
                Type = WorkspaceType.LocalFilesystem,
                InitialWorkspaceInfo = WorkspaceInfo.CreateLocalFs(workspace.Info.GetDirectoryInfo().FullName),
                Provider = this
            }.ToImmutable();
        }

        public WorkspaceProviderInfo ProviderInfo { get; }

        public Task<IWorkspace> GetWorkspace(WorkspaceInfo info)
        {
            return Task.FromResult(Core());

            IWorkspace Core()
            {
                if (info is { Type: WorkspaceType.LocalFilesystem })
                {
                    return workspace;
                }
                return null;
            }
        }
    }
}
