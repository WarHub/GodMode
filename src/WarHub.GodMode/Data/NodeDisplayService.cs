using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarHub.ArmouryModel.Source;

namespace WarHub.GodMode.Data
{
    public struct NodeDisplayInfo
    {
        public string Name;
        public string Icon;
        public string IconModifier;

        public void Deconstruct(out string name, out string icon, out string iconModifier)
        {
            name = Name;
            icon = Icon;
            iconModifier = IconModifier;
        }
    }

    public class NodeDisplayService
    {
        public static NodeDisplayInfo GetNodeDisplayInfo(SourceNode node)
        {
            var name = node is INameableNode named ? named.Name : node.Kind.ToString();
            var (icon, modifier) = node.Kind switch
            {
                // open iconic
                SourceKind.Catalogue => ("grid-three-up", null),
                SourceKind.CatalogueLink => ("grid-three-up", "link-intact"),
                SourceKind.CategoryEntry => ("tag", null),
                SourceKind.CategoryLink => ("tag", "link-intact"),
                SourceKind.Condition => ("cog", null),
                SourceKind.ConditionGroup => ("cog", "list"),
                SourceKind.Constraint => ("crop", null),
                SourceKind.Cost => ("dollar", null),
                SourceKind.CostLimit => ("dollar", "lock-locked"),
                SourceKind.CostType => ("dollar", "beaker"),
                SourceKind.EntryLink => ("link-intact", null),
                SourceKind.ForceEntry => ("project", null),
                SourceKind.Gamesystem => ("target", null),
                SourceKind.InfoGroup => ("info", "list"),
                SourceKind.InfoLink => ("info", "link-intact"),
                SourceKind.Modifier => ("wrench", null),
                SourceKind.ModifierGroup => ("wrench", "list"),
                SourceKind.Profile => ("spreadsheet", null),
                SourceKind.ProfileType => ("spreadsheet", "beaker"),
                SourceKind.Publication => ("book", null),
                SourceKind.Repeat => ("loop", null),
                SourceKind.Roster => ("document", null),
                SourceKind.Rule => ("text", null),
                SourceKind.SelectionEntry => ("file", null),
                SourceKind.SelectionEntryGroup => ("folder", null),
                _ => ("question-mark", null),
            };
            return new NodeDisplayInfo
            {
                Icon = icon,
                Name = name,
                IconModifier = modifier
            };
        }
    }
}
