using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarHub.ArmouryModel.Source;

namespace WarHub.GodMode.Data
{
    public class NodeDisplayService
    {
        public struct NodeDisplayInfo
        {
            public string Icon;
            public string Name;
        }

        public static NodeDisplayInfo GetNodeDisplayInfo(SourceNode node)
        {
            var name = node is INameableNode named ? named.Name : node.Kind.ToString();
            var icon = node.Kind switch
            {
                // open iconic
                SourceKind.Catalogue => "grid-three-up",
                SourceKind.CategoryEntry => "tag",
                SourceKind.Condition => "cog",
                SourceKind.Constraint => "crop",
                SourceKind.Cost => "dollar",
                SourceKind.ForceEntry => "project",
                SourceKind.Gamesystem => "target",
                SourceKind.InfoGroup => "list",
                SourceKind.Modifier => "wrench",
                SourceKind.Profile => "spreadsheet",
                SourceKind.Publication => "book",
                SourceKind.Repeat => "loop",
                SourceKind.Roster => "document",
                SourceKind.Rule => "text",
                SourceKind.SelectionEntry => "file",
                SourceKind.SelectionEntryGroup => "folder",
                _ => "question-mark"
            };
            return new NodeDisplayInfo
            {
                Icon = icon,
                Name = name
            };
        }
    }
}
