using WarHub.ArmouryModel.Source;
using WarHub.GodMode.SourceAnalysis;

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
}
