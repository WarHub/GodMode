// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.ModelFacades
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Mvvm.Commands;

    public abstract class ModelFacadeBase : IModelFacade, INotifyPropertyChanged
    {
        private readonly object _lockObject = new object();
        private ICommand _removeCommand;

        private bool IsSubscribed => PropertyChangedCore != null;

        public abstract string Detail { get; }

        public abstract object Model { get; }

        public abstract string Name { get; }

        public ICommand RemoveCommand
        {
            get { return _removeCommand; }
            set
            {
                if (_removeCommand == value)
                {
                    return;
                }
                _removeCommand = value;
                PropertyChangedCore?.Invoke(this, new PropertyChangedEventArgs(nameof(RemoveCommand)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                lock (_lockObject)
                {
                    var notifyItem = Model as INotifyPropertyChanged;
                    if (notifyItem == null)
                    {
                        return;
                    }
                    if (!IsSubscribed)
                    {
                        OnPropertyChangedSubscribed();
                    }
                    PropertyChangedCore += value;
                }
            }
            remove
            {
                lock (_lockObject)
                {
                    var notifyItem = Model as INotifyPropertyChanged;
                    if (notifyItem == null)
                    {
                        return;
                    }
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

        protected void RaiseThisPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedCore?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedCore?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
    }
}
