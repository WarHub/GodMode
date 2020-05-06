using System.Collections.Generic;
using System.Collections.Immutable;
using WarHub.GodMode.SourceAnalysis;

namespace WarHub.GodMode.Components.Areas.TreeView
{
    public abstract class TreeItemModel
    {
        public abstract NodeDisplayInfo Display { get; }

        public abstract bool ShowChildrenGroupNames { get; }

        public abstract int ChildCount { get; }

        public abstract IEnumerable<(string name, ImmutableArray<TreeItemModel> models)> GetChildrenGroups();
    }
}
