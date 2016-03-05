// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Commands
{
    using System;
    using Mvvm;
    using Xamarin.Forms;

    public class NavTuple
    {
        public NavTuple(Page page, ViewModelBase viewModel)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));
            Page = page;
            ViewModel = viewModel;
        }

        public Page Page { get; }

        public ViewModelBase ViewModel { get; }
    }
}
