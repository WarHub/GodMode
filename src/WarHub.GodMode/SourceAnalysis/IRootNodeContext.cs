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

    public interface IRootNodeContext
    {
        IReferenceTargetInfo<SourceNode> ResolveLink(SourceNode link);
        IReferenceTargetInfo<PublicationNode> ResolvePublication(IPublicationReferencingNode reference);
        IReferenceTargetInfo<CostTypeNode> ResolveCostType(CostBaseNode cost);
        IReferenceTargetInfo<ProfileTypeNode> ResolveProfileType(ProfileNode profile);
    }
}
