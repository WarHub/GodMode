using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using WarHub.ArmouryModel.Source;

namespace WarHub.GodMode.Data
{
    public sealed class ListNodeTreeItemModel : TreeItemModel
    {
        public static ListNodeTreeItemModel Create(NodeDisplayInfo display, IEnumerable<IListNode> listNodes)
        {
            return new ListNodeTreeItemModel(display, listNodes);
        }

        public static ListNodeTreeItemModel Create(NodeDisplayInfo display, params IListNode[] listNodes)
        {
            return new ListNodeTreeItemModel(display, listNodes);
        }

        private ListNodeTreeItemModel(NodeDisplayInfo display, IEnumerable<IListNode> listNodes)
        {
            Display = display;
            ListNodes = listNodes.ToImmutableArray();
            ChildCount = ListNodes.Sum(x => x.NodeList.Count);
        }

        public ImmutableArray<IListNode> ListNodes { get; }

        public override NodeDisplayInfo Display { get; }

        public override bool ShowChildrenGroupNames => false;

        public override int ChildCount { get; }

        private ImmutableArray<TreeItemModel>? models;

        public override IEnumerable<(string name, ImmutableArray<TreeItemModel> models)> GetChildrenGroups()
        {
            yield return (null, models ??= GetModels());
        }

        private ImmutableArray<TreeItemModel> GetModels()
        {
            return ListNodes
                .SelectMany(list => list.NodeList.Select(SingleNodeTreeItemModel.Create))
                .ToImmutableArray<TreeItemModel>();
        }
    }
}
