// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules
{
    using System;
    using Mvvm;

    public abstract class GenericViewModel<TModel> : ViewModelBase
    {
        protected GenericViewModel(TModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            Model = model;
        }

        protected TModel Model { get; }
    }
}
