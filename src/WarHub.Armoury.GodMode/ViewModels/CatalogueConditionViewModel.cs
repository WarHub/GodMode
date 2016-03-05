// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.ViewModels
{
    using System.Collections.Generic;
    using Demo;
    using Model;
    using Services;

    public class CatalogueConditionViewModel : GenericViewModel<CatalogueConditionViewModel, ICatalogueCondition>
    {
        private IEnumerable<ConditionValueUnit> _childValueUnits;

        public CatalogueConditionViewModel(IDialogService dialogService, ICatalogueCondition model = null)
            : base(model ?? ModelLocator.CatalogueCondition)
        {
            DialogService = dialogService;
            SetValidChildValueUnits();
        }

        public decimal ChildValue
        {
            get { return Condition.ChildValue; }
            set { Set(() => Condition.ChildValue == value, () => Condition.ChildValue = value); }
        }

        public ConditionValueUnit ChildValueUnit
        {
            get { return Condition.ChildValueUnit; }
            set { Set(() => Condition.ChildValueUnit == value, () => Condition.ChildValueUnit = value); }
        }

        public IEnumerable<ConditionValueUnit> ChildValueUnits
        {
            get { return _childValueUnits; }
            private set { Set(ref _childValueUnits, value); }
        }

        //TODO ChildLink and ParentLink

        public ConditionChildKind ConditionChildKind
        {
            get { return Condition.ChildKind; }
            set { Set(() => Condition.ChildKind == value, () => Condition.ChildKind = value); }
        }

        public IEnumerable<ConditionChildKind> ConditionChildKinds { get; } = new[]
        {ConditionChildKind.NoChild, ConditionChildKind.Reference};

        public ConditionKind ConditionKind
        {
            get { return Condition.ConditionKind; }
            set
            {
                if (value == ConditionKind.InstanceOf &&
                    (ParentKind == ConditionParentKind.Roster || ParentKind == ConditionParentKind.Reference))
                {
                    WarnInvalidConditionKind();
                    return;
                }
                if (Set(() => Condition.ConditionKind == value, () => Condition.ConditionKind = value))
                {
                    SetConditionKindDependencies();
                }
            }
        }

        public IEnumerable<ConditionKind> ConditionKinds { get; } = new[]
        {
            ConditionKind.LessThan, ConditionKind.AtMost, ConditionKind.EqualTo, ConditionKind.NotEqualTo,
            ConditionKind.GreaterThan, ConditionKind.AtLeast, ConditionKind.InstanceOf
        };

        public ConditionParentKind ParentKind
        {
            get { return Condition.ParentKind; }
            set
            {
                if (Set(() => Condition.ParentKind == value, () => Condition.ParentKind = value))
                {
                    SetValidChildValueUnits();
                }
            }
        }

        public IEnumerable<ConditionParentKind> ParentKinds { get; } = new[]
        {
            ConditionParentKind.Roster, ConditionParentKind.ForceType, ConditionParentKind.Category,
            ConditionParentKind.DirectParent, ConditionParentKind.Reference
        };

        private ICatalogueCondition Condition => Model;

        private IDialogService DialogService { get; }

        private void SetConditionKindDependencies()
        {
            if (ConditionKind == ConditionKind.InstanceOf)
            {
                ChildValue = default(decimal);
                ChildValueUnit = default(ConditionValueUnit);
            }
        }

        private void SetValidChildValueUnits()
        {
            ChildValueUnits = Condition.ParentKind == ConditionParentKind.Roster
                ? new[]
                {
                    ConditionValueUnit.Points, ConditionValueUnit.Selections, ConditionValueUnit.PointsLimit,
                    ConditionValueUnit.TotalSelections
                }
                : new[] {ConditionValueUnit.Points, ConditionValueUnit.Selections};
        }

        protected override CatalogueConditionViewModel WithModelCore(ICatalogueCondition model)
        {
            return new CatalogueConditionViewModel(DialogService, model);
        }


        private void WarnInvalidConditionKind()
        {
            DialogService.ShowDialogAsync("Invalid value", "Cannot set 'instance of' for roster/reference parent kind.",
                "Oh well");
        }
    }
}
