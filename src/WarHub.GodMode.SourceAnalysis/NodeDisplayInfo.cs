using Amadevus.RecordGenerator;

namespace WarHub.GodMode.SourceAnalysis
{
    [Record(Features.Default | Features.Equality)]
    public partial struct NodeDisplayInfo
    {
        public string Title { get; }
        public string Icon { get; }
        public string IconMod { get; }
    }
}
