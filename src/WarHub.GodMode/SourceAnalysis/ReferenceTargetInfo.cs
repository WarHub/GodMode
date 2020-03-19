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

    public static class ReferenceTargetInfo
    {
        public static IReferenceTargetInfo<T> Create<T>(T targetNode) where T : SourceNode
            => new ReferenceTargetInfo<T>(targetNode);
    }
}
