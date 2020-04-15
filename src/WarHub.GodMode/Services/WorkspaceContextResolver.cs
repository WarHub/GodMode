using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using WarHub.ArmouryModel.ProjectModel;
using WarHub.ArmouryModel.Source;
using WarHub.GodMode.Components.Areas.Workspace;
using WarHub.GodMode.SourceAnalysis;

namespace WarHub.GodMode.Services
{
    public class WorkspaceContextResolver : IWorkspaceContextResolver
    {
        private Dictionary<CatalogueBaseNode, GamesystemContext> Contexts { get; }
            = new Dictionary<CatalogueBaseNode, GamesystemContext>();

        private ConcurrentDictionary<IWorkspace, Task> WorkspaceContextCreationTasks { get; }
            = new ConcurrentDictionary<IWorkspace, Task>();

        public async Task<GamesystemContext> GetContext(IWorkspace workspace, CatalogueBaseNode node)
        {
            if (Contexts.TryGetValue(node, out var cached))
            {
                return cached;
            }
            await WorkspaceContextCreationTasks.GetOrAdd(workspace, CreateContexts);
            return Contexts[node];
        }

        private async Task CreateContexts(IWorkspace workspace)
        {
            var contexts = await Task.Run(() => GamesystemContext.Create(workspace).ToImmutableArray());
            foreach (var context in contexts)
            {
                if (context.Gamesystem is { })
                {
                    Contexts[context.Gamesystem] = context;
                }
                foreach (var rootNode in context.Catalogues)
                {
                    Contexts[rootNode] = context;
                }
            }
        }
    }
}
