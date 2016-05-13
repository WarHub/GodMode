// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.ViewModels
{
    using System.Collections.Generic;
    using Demo;
    using Model;
    using Mvvm;

    public class EntryLimitsViewModel : GenericViewModel<EntryLimitsViewModel, IEntryLimits>
    {
        public EntryLimitsViewModel(IEntryLimits model = null) : base(model ?? ModelLocator.EntryLimits)
        {
            InForceLimit = new MinMaxViewModel<int>(Limits.InForceLimit);
            InRosterLimit = new MinMaxViewModel<int>(Limits.InRosterLimit);
            SelectionsLimit = new MinMaxViewModel<int>(Limits.SelectionsLimit);
            PointsLimit = new MinMaxViewModel<decimal>(Limits.PointsLimit);
        }

        public decimal DecimalMax { get; } = decimal.MaxValue;

        public decimal DecimalMin { get; } = decimal.MinValue;

        public MinMaxViewModel<int> InForceLimit { get; }

        public MinMaxViewModel<int> InRosterLimit { get; }

        public int IntMax { get; } = int.MaxValue;

        public int IntMin { get; } = 0;

        public MinMaxViewModel<decimal> PointsLimit { get; }

        public MinMaxViewModel<int> SelectionsLimit { get; }

        private IEntryLimits Limits => Model;

        protected override EntryLimitsViewModel WithModelCore(IEntryLimits model)
        {
            return new EntryLimitsViewModel(model);
        }
    }

    public class MinMaxViewModel<T> : ViewModelBase
    {
        public delegate void LimitChangedEventHandler(MinMaxViewModel<T> sender, T oldValue, T newValue);

        public MinMaxViewModel(IMinMax<T> limits)
        {
            Limits = limits;
        }

        public T Max
        {
            get { return Limits.Max; }
            set
            {
                var oldValue = Limits.Max;
                if (Set(() => EqualityComparer<T>.Default.Equals(Limits.Max, value), () => Limits.Max = value))
                {
                    MaxChanged?.Invoke(this, oldValue, value);
                }
            }
        }

        public T Min
        {
            get { return Limits.Min; }
            set
            {
                var oldValue = Limits.Min;
                if (Set(() => EqualityComparer<T>.Default.Equals(Limits.Min, value), () => Limits.Min = value))
                {
                    MinChanged?.Invoke(this, oldValue, value);
                }
            }
        }

        private IMinMax<T> Limits { get; }

        public event LimitChangedEventHandler MinChanged;

        public event LimitChangedEventHandler MaxChanged;
    }
}
