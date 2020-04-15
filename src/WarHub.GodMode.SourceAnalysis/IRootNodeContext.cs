using WarHub.ArmouryModel.Source;

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
