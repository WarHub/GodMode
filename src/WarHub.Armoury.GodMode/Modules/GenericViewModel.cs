// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules
{
    using System;
    using Mvvm;

    public abstract class GenericViewModel<TViewModel, TModel> : ViewModelBase
    {
        protected GenericViewModel(TModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            Model = model;
        }

        protected TModel Model { get; }

        /// <summary>
        ///     Creates new instance of this class, but one based on provided <paramref name="model" />.
        /// </summary>
        /// <param name="model">Model to base new instance on.</param>
        /// <returns>New instance of this class.</returns>
        public TViewModel WithModel(TModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            return WithModelCore(model);
        }

        protected abstract TViewModel WithModelCore(TModel model);
    }
}
