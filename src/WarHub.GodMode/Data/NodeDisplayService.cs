using WarHub.ArmouryModel.Source;

namespace WarHub.GodMode.Data
{
    public struct NodeDisplayInfo
    {
        public string Title;
        public string Icon;
        public string IconMod;

        public void Deconstruct(out string name, out string icon, out string iconModifier)
        {
            name = Title;
            icon = Icon;
            iconModifier = IconMod;
        }
    }

    public class NodeDisplayService
    {
        public static (string Icon, string IconMod) GetSourceKindIcon(SourceKind kind) => kind switch
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

        public static NodeDisplayInfo GetNodeDisplayInfo(SourceNode node)
        {
            var name = node is INameableNode named ? named.Name : node.Kind.ToString();
            var (icon, modifier) = GetSourceKindIcon(node.Kind);
            return new NodeDisplayInfo
            {
                Icon = icon,
                Title = name,
                IconMod = modifier
            };
        }
    }
}
