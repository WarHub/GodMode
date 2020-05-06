using WarHub.ArmouryModel.Source;

namespace WarHub.GodMode.SourceAnalysis
{
    public class ReferenceTargetInfo<TNode> : IReferenceTargetInfo<TNode> where TNode : SourceNode
    {
        private ReferenceTargetInfo(bool isResolved, TNode targetNode, ReferenceErrorInfo error)
        {
            IsResolved = isResolved;
            TargetNode = targetNode;
            Error = error;
        }

        public ReferenceTargetInfo(TNode targetNode) : this(true, targetNode, null) { }

        public ReferenceTargetInfo(ReferenceErrorInfo error) : this(false, null, error) { }

        public bool IsResolved { get; }

        public TNode TargetNode { get; }

        public ReferenceErrorInfo Error { get; }
    }
}
