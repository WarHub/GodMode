using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using WarHub.ArmouryModel.Source;
using WarHub.GodMode.SourceAnalysis;

namespace WarHub.GodMode.Data
{
    public sealed class ListNodeTreeItemModel : TreeItemModel
    {
        public static ListNodeTreeItemModel Create(
            NodeDisplayInfo display,
            GamesystemContext context,
            params IListNode[] listNodes)
        {
            return new ListNodeTreeItemModel(display, context, listNodes);
        }

        private ListNodeTreeItemModel(
            NodeDisplayInfo display,
            GamesystemContext context,
            IEnumerable<IListNode> listNodes)
        {
            Display = display;
            ListNodes = listNodes.ToImmutableArray();
            ChildCount = ListNodes.Sum(x => x.NodeList.Count);
            Context = context;
        }

        public ImmutableArray<IListNode> ListNodes { get; }

        public override NodeDisplayInfo Display { get; }

        public override bool ShowChildrenGroupNames => false;

        public override int ChildCount { get; }
        public GamesystemContext Context { get; }

        private ImmutableArray<TreeItemModel>? models;

        public override IEnumerable<(string name, ImmutableArray<TreeItemModel> models)> GetChildrenGroups()
        {
            yield return (null, models ??= GetModels());
        }

        private ImmutableArray<TreeItemModel> GetModels()
        {
            return ListNodes
                .SelectMany(list => list.NodeList.Select(node => SingleNodeTreeItemModel.Create(node, Context)))
                .ToImmutableArray<TreeItemModel>();
        }
    }
}
