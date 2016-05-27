// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.StorageExplorer.Commands
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using GodMode.Commands;
    using PCLStorage;

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class DeleteFolderCommand : AppAsyncCommandBase<IFolder>
    {
        public DeleteFolderCommand(IAppCommandDependencyAggregate dependencyAggregate) : base(dependencyAggregate)
        {
        }

        protected override async Task ExecuteCoreAsync(IFolder parameter)
        {
            await parameter.DeleteAsync();
        }

        protected override bool CanExecuteCore(IFolder parameter) => parameter != null;
    }
}
