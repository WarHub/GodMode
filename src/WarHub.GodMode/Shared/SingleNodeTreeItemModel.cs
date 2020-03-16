using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using WarHub.ArmouryModel.Source;
using WarHub.GodMode.Data;

namespace WarHub.GodMode.Shared
{
    //public class ListNodeTreeItemModel : TreeItemModel
    //{

    //}

    public class SingleNodeTreeItemModel : TreeItemModel
    {
        public SingleNodeTreeItemModel(SourceNode node)
        {
            Node = node;
            (Title, Icon, IconMod) = NodeDisplayService.GetNodeDisplayInfo(node);
            childNodeGroups = GetChildNodeGroups(node).ToImmutableArray();
            ChildCount = childNodeGroups.Sum(x => x.nodes.Length);
        }

        public SourceNode Node { get; }

        public override string Title { get; }

        public override string Icon { get; }

        public override string IconMod { get; }

        public override bool ShowChildrenGroupNames => true;

        public override int ChildCount { get; }

        private ImmutableArray<(string name, ImmutableArray<SourceNode> nodes)> childNodeGroups;

        private ImmutableList<(string name, ImmutableArray<TreeItemModel> models)> childrenGroups;

        public override IEnumerable<(string name, ImmutableArray<TreeItemModel> models)> GetChildrenGroups()
        {
            return childrenGroups ??= childNodeGroups
                    .Select(x => (x.name, models: x.nodes.Select(node => new SingleNodeTreeItemModel(node)).ToImmutableArray<TreeItemModel>()))
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
