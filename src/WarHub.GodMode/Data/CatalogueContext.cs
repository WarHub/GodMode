using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Optional;
using Optional.Collections;
using Optional.Unsafe;
using WarHub.ArmouryModel.Source;
using WarHub.ArmouryModel.Workspaces.BattleScribe;
using static MoreLinq.Extensions.DistinctByExtension;
using MoreEnumerable = MoreLinq.MoreEnumerable;

namespace WarHub.GodMode.Data
{
    public abstract class Diagnostic
    {
        public abstract string GetMessage();
    }

    public class DatafileNotFoundDiagnostic : Diagnostic
    {
        public DatafileNotFoundDiagnostic(string datafileId)
        {
            DatafileId = datafileId;
        }

        public string DatafileId { get; }

        public override string GetMessage() => $"Couldn't find datafile with id='{DatafileId}'.";
    }

    public sealed class CatalogueContext
    {
        public SourceNode this[string id] => Index[id];

        private ImmutableDictionary<string, SourceNode> Index { get; }

        public CatalogueBaseNode RootNode { get; }

        public XmlWorkspace Workspace { get; }

        public ImmutableArray<Diagnostic> Diagnostics { get; }

        private static ImmutableHashSet<SourceKind> NonIndexableListKinds { get; }
            = ImmutableHashSet.Create(
                SourceKind.CatalogueLinkList,
                SourceKind.CategoryLinkList,
                SourceKind.ConstraintList,
                SourceKind.CostTypeList,
                SourceKind.ProfileTypeList,
                SourceKind.PublicationList);

        private CatalogueContext(
            ImmutableDictionary<string, SourceNode> index,
            CatalogueBaseNode catalogueBase,
            XmlWorkspace workspace,
            ImmutableArray<Diagnostic> diagnostics)
        {
            Index = index;
            RootNode = catalogueBase;
            Workspace = workspace;
            Diagnostics = diagnostics;
        }

        public static CatalogueContext Create(XmlDocument document) =>
            Create((CatalogueNode)document.GetRoot(), document.Workspace);

        public static CatalogueContext Create(CatalogueNode catalogue, XmlWorkspace workspace)
        {
            var gamesystemId = catalogue is CatalogueNode { GamesystemId: var gstId } ? gstId : catalogue.Id;
            var datafileIndex = workspace
                .GetDocuments(SourceKind.Catalogue, SourceKind.Gamesystem)
                .Select(x => (CatalogueBaseNode)x.GetRoot())
                .Prepend(catalogue)
                .Where(x => x.Kind switch
                {
                    SourceKind.Gamesystem => x.Id == gamesystemId,
                    SourceKind.Catalogue => ((CatalogueNode)x).GamesystemId == gamesystemId,
                    _ => false
                })
                .Distinct()
                .ToImmutableDictionary(x => x.Id);
            var resolutionResult = ResolveDependencies(catalogue, Resolve);
            var index = resolutionResult
                .References
                .Prepend(catalogue)
                .SelectMany(x => x.DescendantsAndSelf(current => !NonIndexableListKinds.Contains(current.Kind)))
                .OfType<IIdentifiableNode>()
                .Concat(datafileIndex.Values)
                .DistinctBy(x => x.Id)
                .ToImmutableDictionary(x => x.Id, x => (SourceNode)x);
            return new CatalogueContext(index, catalogue, workspace, resolutionResult.Diagnostics);

            Option<SourceNode, Diagnostic> Resolve(string targetId)
            {
                datafileIndex.TryGetValue(targetId, out var target);
                return (target as SourceNode).SomeNotNull(CreateDiagnostic);
                Diagnostic CreateDiagnostic() => new DatafileNotFoundDiagnostic(targetId);
            }
        }

        private class NodeIndex
        {
            private NodeIndex(
                SourceNode root,
                ImmutableDictionary<string, SourceNode> index,
                ImmutableArray<SourceNode> references,
                ImmutableArray<Diagnostic> diagnostics,
                Func<SourceNode, NodeIndex> getReferenceIndex)
            {
                Root = root;
                Index = index;
                References = references;
                Diagnostics = diagnostics;
                GetReferenceIndex = getReferenceIndex;
            }

            public SourceNode Root { get; }
            public ImmutableDictionary<string, SourceNode> Index { get; }
            public ImmutableArray<SourceNode> References { get; }
            public ImmutableArray<Diagnostic> Diagnostics { get; }
            public NodeIndex this[SourceNode reference] => GetReferenceIndex(reference);
            private Func<SourceNode, NodeIndex> GetReferenceIndex { get; }

            public static NodeIndex Create(
                SourceNode root,
                Func<SourceNode, DependencyResolutionResult> dependencyResolver,
                Func<SourceNode, NodeIndex> getReferenceIndex)
            {
                var indexGroups = root.DescendantsAndSelf(x => !NonIndexableListKinds.Contains(x.Kind))
                    .OfType<IIdentifiableNode>()
                    .GroupBy(x => x.Id)
                    .ToImmutableArray();
                var index = indexGroups.ToImmutableDictionary(x => x.Key, x => (SourceNode)x.First());
                var indexDiagnostics = indexGroups.Where(x => x.Count() > 1).Select(CreateDuplicateIdDiagnostic);
                var depResult = dependencyResolver(root);
                var diagnostics = indexDiagnostics.Concat(depResult.Diagnostics).ToImmutableArray();
                return new NodeIndex(root, index, depResult.References, diagnostics, getReferenceIndex);

                Diagnostic CreateDuplicateIdDiagnostic(IGrouping<string, IIdentifiableNode> group)
                {
                    return new DuplicateIdDiagnostic(root, group);
                }
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
        }

        private static DependencyResolutionResult ResolveDependencies(
            SourceNode root,
            Func<string, Option<SourceNode, Diagnostic>> idResolver)
        {
            return root switch
            {
                CatalogueNode catalogue => ResolveDependencies(catalogue, idResolver),
                _ => DependencyResolutionResult.Empty
            };
        }

        private static DependencyResolutionResult ResolveDependencies(
            CatalogueNode catalogue,
            Func<string, Option<SourceNode, Diagnostic>> idResolver)
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
            return new DependencyResolutionResult
            {
                References = ids.Values().Skip(1 /* first is always the catalogue itself*/).ToImmutableArray(),
                Diagnostics = ids.Exceptions().ToImmutableArray()
            };
        }

        private struct DependencyResolutionResult
        {
            public ImmutableArray<SourceNode> References;
            public ImmutableArray<Diagnostic> Diagnostics;

            public static DependencyResolutionResult Empty = new DependencyResolutionResult
            {
                References = ImmutableArray<SourceNode>.Empty,
                Diagnostics = ImmutableArray<Diagnostic>.Empty,
            };
        }
    }
}
