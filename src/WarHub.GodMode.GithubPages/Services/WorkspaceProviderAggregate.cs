using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using WarHub.ArmouryModel.ProjectModel;
using WarHub.GodMode.Components.Areas.Workspace;

namespace WarHub.GodMode.GithubPages.Services
{
    public class WorkspaceProviderAggregate : IWorkspaceProviderAggregate
    {
        public WorkspaceProviderAggregate(GitHubWorkspaceProvider gitHubService)
        {
            GitHubService = gitHubService;
            ProviderInfosByType = new WorkspaceProviderInfo[]
            {
                new WorkspaceProviderInfo.Builder
                {
                    Type = WorkspaceType.GitHub,
                    InitialWorkspaceInfo = null,
                    Provider = GitHubService
                }.ToImmutable()
            }.ToImmutableDictionary(x => x.Type);
        }

        public WorkspaceProviderInfo this[WorkspaceType type] => ProviderInfosByType[type];

        public IEnumerable<WorkspaceProviderInfo> ProviderInfos => ProviderInfosByType.Values;

        private GitHubWorkspaceProvider GitHubService { get; }

        private ImmutableDictionary<WorkspaceType, WorkspaceProviderInfo> ProviderInfosByType { get; }

        public async Task<IWorkspace> GetWorkspace(WorkspaceInfo workspaceInfo)
        {
            return await this[workspaceInfo.Type].Provider.GetWorkspace(workspaceInfo);
        }
    }
}
