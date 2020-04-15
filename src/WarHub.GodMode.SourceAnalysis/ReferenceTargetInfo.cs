using WarHub.ArmouryModel.Source;

namespace WarHub.GodMode.SourceAnalysis
{
    public static class ReferenceTargetInfo
    {
        public static IReferenceTargetInfo<T> Create<T>(T targetNode) where T : SourceNode
            => new ReferenceTargetInfo<T>(targetNode);
    }
}
