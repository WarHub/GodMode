using System.Collections.Immutable;
using WarHub.ArmouryModel.ProjectModel;
using WarHub.GodMode.Components.Areas.Workspace;

namespace WarHub.GodMode.Services;

public class WorkspaceProviderAggregate : IWorkspaceProviderAggregate
{
    public WorkspaceProviderAggregate(GitHubWorkspaceProvider gitHubService, LocalFsWorkspaceProvider localFsService)
    {
        GitHubService = gitHubService;
        LocalFsService = localFsService;
        ProviderInfosByType = new WorkspaceProviderInfo[]
        {
            LocalFsService.ProviderInfo,
            new WorkspaceProviderInfo(WorkspaceType.GitHub, null, GitHubService)
        }.ToImmutableDictionary(x => x.Type);
    }

    public WorkspaceProviderInfo this[WorkspaceType type] => ProviderInfosByType[type];

    public IEnumerable<WorkspaceProviderInfo> ProviderInfos => ProviderInfosByType.Values;

    private GitHubWorkspaceProvider GitHubService { get; }

    private LocalFsWorkspaceProvider LocalFsService { get; }

    private ImmutableDictionary<WorkspaceType, WorkspaceProviderInfo> ProviderInfosByType { get; }

    public async Task<IWorkspace> GetWorkspace(WorkspaceInfo workspaceInfo)
    {
        return await this[workspaceInfo.Type].Provider.GetWorkspace(workspaceInfo);
    }
}
