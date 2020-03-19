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
    // represents a diagnostic message from data engine
    public abstract class Diagnostic
    {
        public abstract string GetMessage();
    }
}
