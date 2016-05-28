// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.Editor.Models
{
    using System.ComponentModel;
    using Mvvm.Commands;

    public abstract class ModelFacadeBase : IModelFacade, INotifyPropertyChanged
    {
        public abstract string Detail { get; }

        public abstract object Model { get; }

        public abstract string Name { get; }

        public abstract ICommand RemoveCommand { get; }

        public override bool Equals(object obj)
        {
            var other = obj as ModelFacadeBase;
            return other != null && Model != null && other.Model != null && Model.Equals(other.Model);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
            return Model?.GetHashCode() ?? base.GetHashCode();
        }

        #region PropertyChanged

        private readonly object _lockObject = new object();

        private bool IsSubscribed => PropertyChangedCore != null;

        private bool ModelIsNotifyPropertyChanged() => Model is INotifyPropertyChanged;

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                if (!ModelIsNotifyPropertyChanged())
                {
                    return;
                }
                lock (_lockObject)
                {
                    if (!IsSubscribed)
                    {
                        OnPropertyChangedSubscribed();
                    }
                    PropertyChangedCore += value;
                }
            }
            remove
            {
                if (!ModelIsNotifyPropertyChanged())
                {
                    return;
                }
                lock (_lockObject)
                {
                    PropertyChangedCore -= value;
                    if (!IsSubscribed)
                    {
                        OnPropertyChangedUnsubscribed();
                    }
                }
            }
        }

        protected event PropertyChangedEventHandler PropertyChangedCore;

        protected virtual void OnPropertyChangedUnsubscribed()
        {
            var notifyItem = Model as INotifyPropertyChanged;
            if (notifyItem == null)
            {
                return;
            }
            notifyItem.PropertyChanged -= OnModelPropertyChanged;
        }

        protected virtual void OnPropertyChangedSubscribed()
        {
            var notifyItem = Model as INotifyPropertyChanged;
            if (notifyItem == null)
            {
                return;
            }
            notifyItem.PropertyChanged += OnModelPropertyChanged;
        }

        protected virtual void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedCore?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
