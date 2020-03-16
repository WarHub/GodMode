using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using WarHub.ArmouryModel.Source;

namespace WarHub.GodMode.Data
{
    public sealed class CatalogueBaseTreeItemModel : TreeItemModel, ITreeItemModelWithSourceNode
    {
        public static CatalogueBaseTreeItemModel Create(CatalogueBaseNode node)
        {
            return new CatalogueBaseTreeItemModel(node);
        }

        private CatalogueBaseTreeItemModel(CatalogueBaseNode node)
        {
            Node = node;
            Display = NodeDisplayService.GetNodeDisplayInfo(node);
            ChildModels = CreateChildModels();
        }

        public CatalogueBaseNode Node { get; }

        public override NodeDisplayInfo Display { get; }

        public override int ChildCount => ChildModels.Length;

        public override bool ShowChildrenGroupNames => false;

        private ImmutableArray<TreeItemModel> ChildModels { get; }

        SourceNode ITreeItemModelWithSourceNode.Node => Node;

        public override IEnumerable<(string name, ImmutableArray<TreeItemModel> models)> GetChildrenGroups()
        {
            yield return (null, ChildModels);
        }

        private ImmutableArray<TreeItemModel> CreateChildModels()
        {
            return new ListGroup[]
            {
                ListGroup.Create("Publications", Node.Publications),
                ListGroup.Create("Cost types", Node.CostTypes),
                ListGroup.Create("Profile types", Node.ProfileTypes),
                ListGroup.Create("Category entries", Node.CategoryEntries),
                ListGroup.Create("Force entries", Node.ForceEntries),
                ListGroup.Create("Shared profiles", Node.SharedProfiles),
                ListGroup.Create("Shared rules", Node.SharedRules),
                ListGroup.Create("Shared info groups", Node.SharedInfoGroups),
                ListGroup.Create("Shared selection entries", Node.SharedSelectionEntries),
                ListGroup.Create("Shared selection entry groups", Node.SharedSelectionEntryGroups),
                ListGroup.Create("Root selection entries", Node.SelectionEntries, Node.EntryLinks),
                ListGroup.Create("Rules", Node.Rules, Node.InfoLinks),
            }
                .Select(Map)
                .ToImmutableArray();

            static TreeItemModel Map(ListGroup group)
            {
                var (icon, iconMod) = NodeDisplayService.GetSourceKindIcon(group.ItemKind);
                return ListNodeTreeItemModel.Create(
                    new NodeDisplayInfo
                    {
                        Title = group.Title,
                        Icon = icon,
                        IconMod = iconMod
                    }, group.Lists);
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
