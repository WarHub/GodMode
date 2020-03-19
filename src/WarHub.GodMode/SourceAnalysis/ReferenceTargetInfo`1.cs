using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Optional;
using Optional.Collections;
using Optional.Unsafe;
using WarHub.ArmouryModel.Source;
using WarHub.ArmouryModel.Workspaces.BattleScribe;
using MoreEnumerable = MoreLinq.MoreEnumerable;

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
