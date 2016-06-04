// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Commands
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using AppServices;
    using Model.DataAccess;
    using Mvvm.Commands;

    /// <summary>
    ///     Wraps execution exceptions into UI dialog.
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public abstract class AppAsyncCommandBase : ProgressingAsyncCommandBase
    {
        protected AppAsyncCommandBase(IAppCommandDependencyAggregate dependencyAggregate)
        {
            if (dependencyAggregate == null)
                throw new ArgumentNullException(nameof(dependencyAggregate));
            DependencyAggregate = dependencyAggregate;
            UseHandleExecutionException = true;
            RethrowExecutionException = false;
        }

        protected IDialogService DialogService => DependencyAggregate.DialogService;

        protected ILog Log => DependencyAggregate.Log;

        private IAppCommandDependencyAggregate DependencyAggregate { get; }

        /// <summary>
        ///     Shows UI dialog with exception message.
        /// </summary>
        /// <param name="e">Exception shown in dialog.</param>
        protected override void HandleExecutionException(Exception e)
        {
            DependencyAggregate.HandleException(e);
        }
    }

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public abstract class AppAsyncCommandBase<T> : ProgressingAsyncCommandBase<T>
    {
        protected AppAsyncCommandBase(IAppCommandDependencyAggregate dependencyAggregate)
        {
            if (dependencyAggregate == null)
                throw new ArgumentNullException(nameof(dependencyAggregate));
            DependencyAggregate = dependencyAggregate;
            UseHandleExecutionException = true;
            RethrowExecutionException = false;
        }

        protected IDialogService DialogService => DependencyAggregate.DialogService;

        protected ILog Log => DependencyAggregate.Log;

        private IAppCommandDependencyAggregate DependencyAggregate { get; }

        /// <summary>
        ///     Shows UI dialog with exception message.
        /// </summary>
        /// <param name="e">Exception shown in dialog.</param>
        protected override void HandleExecutionException(Exception e)
        {
            DependencyAggregate.HandleException(e);
        }
    }
}
