using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using WarHub.ArmouryModel.Source;

namespace WarHub.GodMode.Data
{
    public sealed class SingleNodeTreeItemModel : TreeItemModel, ITreeItemModelWithSourceNode
    {
        private ImmutableArray<(string name, ImmutableArray<SourceNode> nodes)> childNodeGroups;

        private ImmutableList<(string name, ImmutableArray<TreeItemModel> models)> childrenGroups;

        private SingleNodeTreeItemModel(SourceNode node)
        {
            Node = node;
            Display = NodeDisplayService.GetNodeDisplayInfo(node);
            childNodeGroups = GetChildNodeGroups(node).ToImmutableArray();
            ChildCount = childNodeGroups.Sum(x => x.nodes.Length);
        }

        public SourceNode Node { get; }

        public override NodeDisplayInfo Display { get; }

        public override int ChildCount { get; }

        public override bool ShowChildrenGroupNames => true;
        SourceNode ITreeItemModelWithSourceNode.Node => Node;

        private static ImmutableHashSet<string> ExcludedListNames { get; }
            = new[]
            {
                nameof(SelectionEntryNode.Costs),
                nameof(SelectionEntryBaseNode.CategoryLinks),
                nameof(ProfileNode.Characteristics),
                nameof(ProfileTypeNode.CharacteristicTypes),
            }.ToImmutableHashSet();

        public static SingleNodeTreeItemModel Create(SourceNode node)
        {
            return new SingleNodeTreeItemModel(node);
        }

        public override IEnumerable<(string name, ImmutableArray<TreeItemModel> models)> GetChildrenGroups()
        {
            return childrenGroups ??= childNodeGroups
                    .Select(x => (x.name, models: x.nodes.Select(Create).ToImmutableArray<TreeItemModel>()))
                    .ToImmutableList();
        }

        private static IEnumerable<(string name, ImmutableArray<SourceNode> nodes)> GetChildNodeGroups(SourceNode node)
        {
            var itemNameGroups = new[]
            {
                new[]
                {
                    nameof(SelectionEntryBaseCore.SelectionEntries),
                    nameof(SelectionEntryBaseCore.SelectionEntryGroups),
                    nameof(SelectionEntryBaseCore.EntryLinks),
                },
                new[]
                {
                    nameof(SelectionEntryBaseCore.Profiles),
                    nameof(SelectionEntryBaseCore.Rules),
                    nameof(SelectionEntryBaseCore.InfoGroups),
                    nameof(SelectionEntryBaseCore.InfoLinks),
                },
                new[]
                {
                    nameof(EntryBaseCore.Modifiers),
                    nameof(EntryBaseCore.ModifierGroups),
                }
            };
            var groups =
                from childInfo in node.ChildrenInfos().Where(x => !ExcludedListNames.Contains(x.Name))
                join association in (
                    from g in itemNameGroups
                    from name in g
                    select new
                    {
                        groupName = g[0],
                        listName = name
                    }) on childInfo.Name equals association.listName into assocGroup
                from assoc in assocGroup.DefaultIfEmpty(new { groupName = childInfo.Name, listName = childInfo.Name })
                group childInfo by assoc into listGroup
                let nodes = listGroup.SelectMany(x => x.Node.Children()).ToImmutableArray()
                where nodes.Length > 0
                select (name: listGroup.Key.groupName, nodes);
            return groups;
        }
    }
}
