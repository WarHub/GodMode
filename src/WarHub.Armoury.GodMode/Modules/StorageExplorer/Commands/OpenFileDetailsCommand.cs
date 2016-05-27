// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.StorageExplorer.Commands
{
    using System.Diagnostics.CodeAnalysis;
    using AppServices;
    using GodMode.Commands;
    using PCLStorage;
    using ViewModels;
    using Views;

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class OpenFileDetailsCommand : NavigateCommandBase<IFile>
    {
        public OpenFileDetailsCommand(IAppCommandDependencyAggregate dependencyAggregate,
            INavigationService navigationService, StorageFileDetailsVmFactory storageFileDetailsVmFactory)
            : base(dependencyAggregate, navigationService)
        {
            StorageFileDetailsVmFactory = storageFileDetailsVmFactory;
        }

        private StorageFileDetailsVmFactory StorageFileDetailsVmFactory { get; }

        protected override NavTuple GetNavTuple(IFile parameter)
        {
            var vm = StorageFileDetailsVmFactory(parameter);
            return new NavTuple(new StorageFilePage(), vm);
        }

        protected override bool CanExecuteCore(IFile parameter) => parameter != null;
    }
}
