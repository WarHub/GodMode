using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using WarHub.ArmouryModel.Source;
using WarHub.GodMode.SourceAnalysis;

namespace WarHub.GodMode.Data
{
    public sealed class CatalogueBaseTreeItemModel : TreeItemModel, ITreeItemModelWithSourceNode
    {
        public static CatalogueBaseTreeItemModel Create(CatalogueBaseNode node, GamesystemContext context)
        {
            return new CatalogueBaseTreeItemModel(node, context);
        }

        private CatalogueBaseTreeItemModel(CatalogueBaseNode node, GamesystemContext context)
        {
            Node = node;
            Context = context;
            Display = context.GetNodeDisplayInfo(node);
            ChildModels = CreateChildModels();
        }

        public CatalogueBaseNode Node { get; }

        public override NodeDisplayInfo Display { get; }

        public override int ChildCount => ChildModels.Length;

        public override bool ShowChildrenGroupNames => false;

        private ImmutableArray<TreeItemModel> ChildModels { get; }

        SourceNode ITreeItemModelWithSourceNode.Node => Node;

        public GamesystemContext Context { get; }

        public override IEnumerable<(string name, ImmutableArray<TreeItemModel> models)> GetChildrenGroups()
        {
            yield return (null, ChildModels);
        }

        private ImmutableArray<TreeItemModel> CreateChildModels()
        {
            IEnumerable<ListGroup> GetListGroups()
            {
                if (Node is CatalogueNode catalogue)
                {
                    yield return ListGroup.Create("Catalogue links", catalogue.CatalogueLinks);
                }
                yield return ListGroup.Create("Publications", Node.Publications);
                yield return ListGroup.Create("Cost types", Node.CostTypes);
                yield return ListGroup.Create("Profile types", Node.ProfileTypes);
                yield return ListGroup.Create("Category entries", Node.CategoryEntries);
                yield return ListGroup.Create("Force entries", Node.ForceEntries);
                yield return ListGroup.Create("Shared profiles", Node.SharedProfiles);
                yield return ListGroup.Create("Shared rules", Node.SharedRules);
                yield return ListGroup.Create("Shared info groups", Node.SharedInfoGroups);
                yield return ListGroup.Create("Shared selection entries", Node.SharedSelectionEntries);
                yield return ListGroup.Create("Shared selection entry groups", Node.SharedSelectionEntryGroups);
                yield return ListGroup.Create("Root selection entries", Node.SelectionEntries, Node.EntryLinks);
                yield return ListGroup.Create("Rules", Node.Rules, Node.InfoLinks);
            }
            return GetListGroups()
                .Select(Map)
                .ToImmutableArray();

            TreeItemModel Map(ListGroup group)
            {
                var (icon, iconMod) = group.ItemKind.GetIcon();
                return ListNodeTreeItemModel.Create(
                    new NodeDisplayInfo
                    {
                        Title = group.Title,
                        Icon = icon,
                        IconMod = iconMod
                    },
                    Context,
                    group.Lists);
            }
        }

        private struct ListGroup
        {
            public string Title;
            public SourceKind ItemKind;
            public IListNode[] Lists;

            public static ListGroup Create(string title, params IListNode[] lists)
            {
                return new ListGroup
                {
                    Title = title,
                    ItemKind = lists[0].ElementKind,
                    Lists = lists
                };
            }
        }
    }
}
