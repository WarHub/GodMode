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
