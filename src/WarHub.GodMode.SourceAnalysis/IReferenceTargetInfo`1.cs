using WarHub.ArmouryModel.Source;

namespace WarHub.GodMode.SourceAnalysis
{
    /// <summary>
    /// Contains information on reference resolution:
    /// if it was found, if there were other candidates, etc.
    /// </summary>
    public interface IReferenceTargetInfo<out TNode> where TNode : SourceNode
    {
        bool IsResolved { get; }
        TNode TargetNode { get; }
        ReferenceErrorInfo Error { get; }
    }
}
