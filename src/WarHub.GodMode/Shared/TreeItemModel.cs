using System.Collections.Generic;
using System.Collections.Immutable;
using WarHub.ArmouryModel.Source;

namespace WarHub.GodMode.Shared
{
    public abstract class TreeItemModel
    {
        public abstract string Title { get; }

        public abstract string Icon { get; }

        public abstract string IconMod { get; }

        public abstract bool ShowChildrenGroupNames { get; }

        public abstract int ChildCount { get; }

        public abstract IEnumerable<(string name, ImmutableArray<TreeItemModel> models)> GetChildrenGroups();

        protected static ImmutableHashSet<string> ExcludedListNames { get; }
            = new[]
            {
                nameof(SelectionEntryNode.Costs),
                nameof(SelectionEntryBaseNode.CategoryLinks),
                nameof(ProfileNode.Characteristics),
                nameof(ProfileTypeNode.CharacteristicTypes),
            }.ToImmutableHashSet();
    }
}
