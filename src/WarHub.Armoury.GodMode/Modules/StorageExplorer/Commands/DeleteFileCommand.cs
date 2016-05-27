// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Modules.StorageExplorer.Commands
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using GodMode.Commands;
    using PCLStorage;

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class DeleteFileCommand : AppAsyncCommandBase<IFile>
    {
        public DeleteFileCommand(IAppCommandDependencyAggregate dependencyAggregate) : base(dependencyAggregate)
        {
        }

        protected override async Task ExecuteCoreAsync(IFile parameter)
        {
            await parameter.DeleteAsync();
        }

        protected override bool CanExecuteCore(IFile parameter) => parameter != null;
    }
}
