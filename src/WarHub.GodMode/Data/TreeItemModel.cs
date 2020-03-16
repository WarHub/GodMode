using System.Collections.Generic;
using System.Collections.Immutable;
using WarHub.ArmouryModel.Source;

namespace WarHub.GodMode.Data
{
    public abstract class TreeItemModel
    {
        public abstract NodeDisplayInfo Display { get; }

        public abstract bool ShowChildrenGroupNames { get; }

        public abstract int ChildCount { get; }

        public abstract IEnumerable<(string name, ImmutableArray<TreeItemModel> models)> GetChildrenGroups();
    }
}
