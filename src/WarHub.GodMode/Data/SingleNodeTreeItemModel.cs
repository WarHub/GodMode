using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using WarHub.ArmouryModel.Source;
using WarHub.GodMode.SourceAnalysis;

namespace WarHub.GodMode.Data
{
    public sealed class SingleNodeTreeItemModel : TreeItemModel, ITreeItemModelWithSourceNode
    {
        private ImmutableArray<(string name, ImmutableArray<SourceNode> nodes)> childNodeGroups;

        private ImmutableList<(string name, ImmutableArray<TreeItemModel> models)> childrenGroups;

        private SingleNodeTreeItemModel(SourceNode node, GamesystemContext context)
        {
            Node = node;
            Display = context.GetNodeDisplayInfo(node);
            childNodeGroups = GetChildNodeGroups(node).ToImmutableArray();
            ChildCount = childNodeGroups.Sum(x => x.nodes.Length);
            Context = context;
        }

        public SourceNode Node { get; }

        public override NodeDisplayInfo Display { get; }

        public override int ChildCount { get; }

        public override bool ShowChildrenGroupNames => true;

        public GamesystemContext Context { get; }

        SourceNode ITreeItemModelWithSourceNode.Node => Node;

        private static ImmutableHashSet<string> ExcludedListNames { get; }
            = new[]
            {
                nameof(SelectionEntryNode.Costs),
                nameof(SelectionEntryBaseNode.CategoryLinks),
                nameof(ProfileNode.Characteristics),
                nameof(ProfileTypeNode.CharacteristicTypes),
            }.ToImmutableHashSet();

        public static SingleNodeTreeItemModel Create(SourceNode node, GamesystemContext context)
        {
            return new SingleNodeTreeItemModel(node, context);
        }

        public override IEnumerable<(string name, ImmutableArray<TreeItemModel> models)> GetChildrenGroups()
        {
            return childrenGroups ??= childNodeGroups
                    .Select(x => (x.name, models: CreateModels(x.nodes)))
                    .ToImmutableList();

            ImmutableArray<TreeItemModel> CreateModels(ImmutableArray<SourceNode> nodes) =>
                nodes.Select(node => Create(node, Context)).ToImmutableArray<TreeItemModel>();
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
