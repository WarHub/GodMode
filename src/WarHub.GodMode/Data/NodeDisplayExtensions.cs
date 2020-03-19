using System.Globalization;
using WarHub.ArmouryModel.Source;
using WarHub.GodMode.SourceAnalysis;
using static System.FormattableString;

namespace WarHub.GodMode.Data
{
    public static class NodeDisplayExtensions
    {
        public static (string Icon, string IconMod) GetIcon(this SourceKind kind) => kind switch
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

        public static NodeDisplayInfo GetNodeDisplayInfo(this GamesystemContext ctx, SourceNode node)
        {
            var name = ctx.GetNodeDisplayTitle(node);
            var (icon, modifier) = node.Kind.GetIcon();
            return new NodeDisplayInfo
            {
                Icon = icon,
                Title = name,
                IconMod = modifier
            };
        }

        public static string GetNodeDisplayTitle(this GamesystemContext ctx, SourceNode node)
        {
            var root = node.FirstAncestorOrSelf<CatalogueBaseNode>();
            return node switch
            {
                CatalogueLinkNode link => GetLinkNameFromTargetOrSelf(link),
                CategoryLinkNode link => GetLinkNameFromTargetOrSelf(link),
                EntryLinkNode link => GetLinkNameFromTargetOrSelf(link),
                InfoLinkNode link => GetLinkNameFromTargetOrSelf(link),
                INameableNode named => named.Name,
                ConditionNode cond =>
                    Invariant($"if {cond.Type} {FormatValue(cond.Value, cond.PercentValue)} {cond.Field} in {cond.Scope} of {cond.ChildId}"),
                ConditionGroupNode condGroup => Invariant($"{condGroup.Type}"),
                ConstraintNode constr =>
                    Invariant($"{constr.Type} {FormatValue(constr.Value, constr.PercentValue)} {constr.Field} in {constr.Scope}"),
                ModifierNode mod => GetModifierTitle(mod),
                _ => node.Kind.ToString()
            };

            string GetLinkNameFromTargetOrSelf<T>(T link) where T : SourceNode, INameableNode
            {
                return ctx?[root].ResolveLink(link) is { IsResolved: true, TargetNode: { } target }
                    ? ctx.GetNodeDisplayTitle(target)
                    : link.Name;
            }

            string GetModifierTitle(ModifierNode mod)
            {
                var typeText = mod.Type switch
                {
                    ModifierKind.Increment => " by",
                    ModifierKind.Decrement => " by",
                    ModifierKind.Set => " to",
                    ModifierKind.Append => " with",
                    _ => ""
                };
                return Invariant($"{mod.Type} {mod.Field}{typeText} {mod.Value}");
            }
            string FormatValue(decimal number, bool percent)
            {
                return number.ToString(percent ? "P" : "G", CultureInfo.InvariantCulture);
            }
        }
    }
}
