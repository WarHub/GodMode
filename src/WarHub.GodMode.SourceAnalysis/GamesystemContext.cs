using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Optional;
using Optional.Collections;
using Optional.Unsafe;
using WarHub.ArmouryModel.ProjectModel;
using WarHub.ArmouryModel.Source;
using MoreEnumerable = MoreLinq.MoreEnumerable;

namespace WarHub.GodMode.SourceAnalysis
{
    public sealed class GamesystemContext
    {
        private GamesystemContext(
            GamesystemNode gamesystem,
            ImmutableArray<CatalogueNode> catalogues,
            ImmutableDictionary<CatalogueBaseNode, RootNodeClosure> closures,
            ImmutableArray<Diagnostic> diagnostics)
        {
            Gamesystem = gamesystem;
            Catalogues = catalogues;
            Closures = closures;
            Diagnostics = diagnostics;
            RootContexts = Closures.ToImmutableDictionary(x => x.Key, x => CreateContext(x.Value));

            IRootNodeContext CreateContext(RootNodeClosure closure)
            {
                return new RootNodeContext(closure);
            }
        }

        public GamesystemNode Gamesystem { get; }

        public ImmutableArray<CatalogueNode> Catalogues { get; }

        public ImmutableArray<Diagnostic> Diagnostics { get; }

        public IRootNodeContext this[CatalogueBaseNode rootNode] => RootContexts[rootNode];

        private ImmutableDictionary<CatalogueBaseNode, RootNodeClosure> Closures { get; }

        private ImmutableDictionary<CatalogueBaseNode, IRootNodeContext> RootContexts { get; }

        public bool Contains(CatalogueBaseNode root) => RootContexts.ContainsKey(root);

        public static IEnumerable<GamesystemContext> Create(IWorkspace workspace)
        {
            return workspace
                .Datafiles
                .Where(x => x.DataKind == SourceKind.Catalogue || x.DataKind == SourceKind.Gamesystem)
                .Select(x => (CatalogueBaseNode?)x.GetData())
                .Where(x => x is not null)
                .GroupBy(x => GetGamesystemId(x!, "unknown"))
                .Select(group => CreateSingle(group.ToImmutableArray()!));
        }

        public static GamesystemContext CreateSingle(ImmutableArray<CatalogueBaseNode> rootNodes)
        {
            var gstCount = rootNodes.Select(GetGamesystemId).Distinct().Count();
            if (gstCount != 1)
            {
                throw new ArgumentException(
                    "This method accepts only rootNodes of a single gamesystem.",
                    nameof(rootNodes));
            }
            var datafileIndex = rootNodes.ToImmutableDictionary(x => x.Id);
            var closures = rootNodes
                .ToImmutableDictionary(x => x, root => ResolveDependencies(root, ResolveDatafile));
            var gamesystems = rootNodes.OfType<GamesystemNode>().ToImmutableArray();
            var catalogues = rootNodes.OfType<CatalogueNode>().ToImmutableArray();
            var closureDiagnostics = closures.Values.SelectMany(x => x.Diagnostics).ToImmutableArray();
            var diagnostics = gamesystems.Length == 1
                ? closureDiagnostics
                : closureDiagnostics.Add(CreateGamesystemCountDiagnostic());
            return new GamesystemContext(gamesystems.FirstOrDefault(), catalogues, closures, diagnostics);

            Diagnostic CreateGamesystemCountDiagnostic()
            {
                return new GamesystemCountDiagnostic(gamesystems);
            }
            Option<CatalogueBaseNode, Diagnostic> ResolveDatafile(string targetId)
            {
                datafileIndex.TryGetValue(targetId, out var target);
                return target.SomeNotNull(CreateDiagnostic);
                Diagnostic CreateDiagnostic() => new DatafileNotFoundDiagnostic(targetId);
            }
        }

        private static string GetGamesystemId(CatalogueBaseNode node) => GetGamesystemId(node, null);

        private static string GetGamesystemId(CatalogueBaseNode node, string fallbackId) =>
            node.Accept(new GamesystemIdVisitor(fallbackId));

        /// <summary>
        /// Handles reference resolution taking closure (referenced root nodes) into account.
        /// </summary>
        private class RootNodeContext : IRootNodeContext
        {
            public RootNodeContext(RootNodeClosure rootClosure)
            {
                RootClosure = rootClosure;
            }

            public RootNodeClosure RootClosure { get; }

            public IReferenceTargetInfo<CostTypeNode> ResolveCostType(CostBaseNode cost)
            {
                if (cost is null)
                {
                    return CreateError<CostTypeNode>("Can't resolve null link.");
                }
                return ResolveReference(cost.TypeId, x => x.CostTypes);
            }

            public IReferenceTargetInfo<SourceNode> ResolveLink(SourceNode node)
            {
                return node switch
                {
                    CatalogueLinkNode { Type: CatalogueLinkKind.Catalogue } link =>
                        ResolveReferenceFlat(link.TargetId, RootClosure.References),

                    CategoryLinkNode link =>
                        ResolveReference(link.TargetId, x => x.CategoryEntries),

                    EntryLinkNode { Type: EntryLinkKind.SelectionEntry } link =>
                        ResolveReference(link.TargetId, x => x.SharedSelectionEntries),

                    EntryLinkNode { Type: EntryLinkKind.SelectionEntryGroup } link =>
                        ResolveReference(link.TargetId, x => x.SharedSelectionEntryGroups),

                    InfoLinkNode { Type: InfoLinkKind.InfoGroup } link =>
                        ResolveReference(link.TargetId, x => x.SharedInfoGroups),

                    InfoLinkNode { Type: InfoLinkKind.Profile } link =>
                        ResolveReference(link.TargetId, x => x.SharedProfiles),

                    InfoLinkNode { Type: InfoLinkKind.Rule } link =>
                        ResolveReference(link.TargetId, x => x.SharedRules),

                    null => CreateError("Can't resolve null link."),
                    _ => CreateError($"Can't resolve unknown link type ({node.GetType()}).")
                };
            }

            public IReferenceTargetInfo<ProfileTypeNode> ResolveProfileType(ProfileNode profile)
            {
                if (profile is null)
                {
                    return CreateError<ProfileTypeNode>("Can't resolve null link.");
                }
                return ResolveReference(profile.TypeId, x => x.ProfileTypes);
            }

            public IReferenceTargetInfo<PublicationNode> ResolvePublication(IPublicationReferencingNode reference)
            {
                if (reference is null)
                {
                    return CreateError<PublicationNode>("Can't resolve null link.");
                }
                return ResolveReference(reference.PublicationId, x => x.Publications);
            }

            private static IReferenceTargetInfo<SourceNode> CreateError(string message) =>
                new ReferenceTargetInfo<SourceNode>(new ReferenceErrorInfo(message));
            private static IReferenceTargetInfo<T> CreateError<T>(string message) where T : SourceNode =>
                new ReferenceTargetInfo<T>(new ReferenceErrorInfo(message));

            private IReferenceTargetInfo<TNode> ResolveReference<TNode>(
                string targetId,
                Func<CatalogueBaseNode, ListNode<TNode>> getList) where TNode : SourceNode, IIdentifiableNode
            {
                if (string.IsNullOrEmpty(targetId))
                {
                    return CreateError(new ReferenceErrorInfo("Empty targetId is non-resolvable."));
                }
                foreach (var rootNode in RootClosure.References.Prepend(RootClosure.Root))
                {
                    if (TryGetTarget(rootNode, out var target))
                    {
                        return ReferenceTargetInfo.Create(target);
                    }
                }
                return CreateError(new ReferenceErrorInfo($"Failed to resolve targetId '{targetId}'."));

                bool TryGetTarget(CatalogueBaseNode root, out TNode target)
                {
                    var list = getList(root);
                    var targets = list.NodeList.Where(x => x.Id == targetId);
                    target = targets.FirstOrDefault();
                    return target != null;
                }

                static IReferenceTargetInfo<TNode> CreateError(ReferenceErrorInfo error)
                {
                    return new ReferenceTargetInfo<TNode>(error);
                }
            }

            private IReferenceTargetInfo<TNode> ResolveReferenceFlat<TNode>(
                string targetId,
                IEnumerable<TNode> list) where TNode : SourceNode, IIdentifiableNode
            {
                if (string.IsNullOrEmpty(targetId))
                {
                    return CreateError(new ReferenceErrorInfo("Empty targetId is non-resolvable."));
                }
                if (TryGetTarget(out var target))
                {
                    return ReferenceTargetInfo.Create(target);
                }
                return CreateError(new ReferenceErrorInfo($"Failed to resolve targetId '{targetId}'."));

                bool TryGetTarget(out TNode target)
                {
                    var targets = list.Where(x => x.Id == targetId);
                    target = targets.FirstOrDefault();
                    return target != null;
                }

                static IReferenceTargetInfo<TNode> CreateError(ReferenceErrorInfo error)
                {
                    return new ReferenceTargetInfo<TNode>(error);
                }
            }
        }

        private static RootNodeClosure ResolveDependencies(
            CatalogueBaseNode root,
            Func<string, Option<CatalogueBaseNode, Diagnostic>> idResolver)
        {
            return root switch
            {
                CatalogueNode catalogue => ResolveDependencies(catalogue, idResolver),
                _ => new RootNodeClosure(root)
            };
        }

        private static RootNodeClosure ResolveDependencies(
            CatalogueNode catalogue,
            Func<string, Option<CatalogueBaseNode, Diagnostic>> idResolver)
        {
            var ids = MoreEnumerable.Unfold(
                   state: (
                       imported: ImmutableHashSet.Create<string>(),
                       required: ImmutableQueue.Create(catalogue.Id, catalogue.GamesystemId)),
                   generator: state =>
                   {
                       var (oldImported, oldRequired) = state;
                       var skippedRequired = MoreEnumerable
                           .Generate(oldRequired, queue => queue.Dequeue())
                           .TakeWhile(queue => !queue.IsEmpty)
                           .FirstOrDefault(queue => !oldImported.Contains(queue.Peek()));
                       if (skippedRequired is null)
                       {
                           return (default, state: (oldImported, oldRequired.Clear()));
                       }
                       var (next, requiredNoNext) = (skippedRequired.Peek(), skippedRequired.Dequeue());
                       var processed = idResolver(next);
                       var imported = oldImported.Add(next);
                       var required = (processed.ValueOrDefault() as CatalogueNode)?
                           .CatalogueLinks
                           .Where(x => !imported.Contains(x.TargetId))
                           .Aggregate(requiredNoNext, (sum, next) => sum.Enqueue(next.TargetId)) ?? requiredNoNext;
                       return (processed: processed.Some(), state: (imported, required));
                   },
                   predicate: result => result.processed.HasValue,
                   stateSelector: result => result.state,
                   resultSelector: result => result.processed.ValueOrDefault());
            var references = ids.Values().Skip(1 /* first is always the catalogue itself*/).ToImmutableArray();
            var diagnostics = ids.Exceptions().ToImmutableArray();
            return new RootNodeClosure(catalogue, references, diagnostics);
        }

        private class DatafileNotFoundDiagnostic : Diagnostic
        {
            public DatafileNotFoundDiagnostic(string datafileId)
            {
                DatafileId = datafileId;
            }

            public string DatafileId { get; }

            public override string GetMessage() => $"Couldn't find datafile with id='{DatafileId}'.";
        }

        private class DuplicateIdDiagnostic : Diagnostic
        {
            public DuplicateIdDiagnostic(SourceNode root, IGrouping<string, IIdentifiableNode> group)
            {
                Root = root;
                Group = group;
            }

            public SourceNode Root { get; }

            public IGrouping<string, IIdentifiableNode> Group { get; }

            public override string GetMessage() => $"Duplicated ID '{Group.Key}' (found {Group.Count()} times).";
        }

        private class GamesystemCountDiagnostic : Diagnostic
        {
            public GamesystemCountDiagnostic(ImmutableArray<GamesystemNode> nodes)
            {
                Nodes = nodes;
            }

            public ImmutableArray<GamesystemNode> Nodes { get; }

            public override string GetMessage() => $"Found {Nodes.Length} gamesystems (should be only 1).";
        }

        private class RootNodeClosure
        {
            public RootNodeClosure(CatalogueBaseNode root)
            {
                Root = root;
                References = ImmutableArray<CatalogueBaseNode>.Empty;
                Diagnostics = ImmutableArray<Diagnostic>.Empty;
            }

            public RootNodeClosure(
                CatalogueBaseNode root,
                ImmutableArray<CatalogueBaseNode> references,
                ImmutableArray<Diagnostic> diagnostics)
            {
                Root = root;
                References = references;
                Diagnostics = diagnostics;
            }

            public CatalogueBaseNode Root { get; }
            public ImmutableArray<CatalogueBaseNode> References { get; }
            public ImmutableArray<Diagnostic> Diagnostics { get; }
        }

        private class GamesystemIdVisitor : SourceVisitor<string>
        {
            public GamesystemIdVisitor(string fallbackId)
            {
                FallbackId = fallbackId;
            }
            public string FallbackId { get; }
            public override string DefaultVisit(SourceNode node) => FallbackId;
            public override string VisitCatalogue(CatalogueNode node) => node.GamesystemId;
            public override string VisitGamesystem(GamesystemNode node) => node.Id;
        }

        public interface INodeIndex
        {
            SourceNode this[string id] { get; }
        }

        public interface INodeIndex<TNode> : INodeIndex where TNode : SourceNode
        {
            new TNode this[string id] { get; }
        }

        public class SimpleNodeIndex : INodeIndex
        {
            public SimpleNodeIndex(ImmutableDictionary<string, SourceNode> index)
            {
                Index = index;
            }

            public SourceNode this[string id] => Index[id];

            public ImmutableDictionary<string, SourceNode> Index { get; }
        }
    }
}
