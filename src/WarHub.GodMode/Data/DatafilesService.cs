using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WarHub.ArmouryModel.ProjectModel;
using WarHub.ArmouryModel.Source;
using WarHub.ArmouryModel.Workspaces.BattleScribe;
using WarHub.GodMode.SourceAnalysis;

namespace WarHub.GodMode.Data
{
    public class DatafilesService
    {
        private readonly string dataDir;
        private readonly IWorkspace workspace;

        public ImmutableArray<IDatafileInfo> Datafiles => workspace.Datafiles;

        public DatafilesService(IConfiguration configuration)
        {
            dataDir = configuration["DataDir"];
            workspace = XmlWorkspace.CreateFromDirectory(dataDir);
        }

        private Dictionary<SourceNode, GamesystemContext> Contexts { get; }
            = new Dictionary<SourceNode, GamesystemContext>();

        public async Task<GamesystemContext> GetContextFor(CatalogueBaseNode node)
        {
            if (Contexts.TryGetValue(node, out var cached))
            {
                return cached;
            }
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
            return Contexts[node];
        }
    }
}
