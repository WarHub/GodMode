// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Autofac;
    using Model.BattleScribe.Services;
    using Model.DataAccess;
    using Model.DataAccess.Autofac;
    using Mvvm;
    using Xamarin.Forms;

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class GodModeAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // register concrete types
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(type => type.IsAssignableTo<ViewModelBase>() || type.Name.EndsWith("Command"));

            // register service implementations
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(type => type.Name.EndsWith("Service") || type.Name.EndsWith("Aggregate"))
                .AsImplementedInterfaces();

            builder.RegisterModule<DataAccessModule>();

            // dispatcher is required by DataAccessModule
            builder.RegisterType<Dispatcher>().AsImplementedInterfaces();

            // repo manager implementation is required by DataAccessModule
            builder.RegisterType<BattleScribeRepoManager>().AsImplementedInterfaces();
        }

        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
        private class Dispatcher : IDispatcher
        {
            public Task InvokeOnUiAsync(Action action)
            {
                var taskCompletionSource = new TaskCompletionSource<int>();
                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        action();
                        taskCompletionSource.SetResult(0);
                    }
                    catch (Exception e)
                    {
                        taskCompletionSource.SetException(e);
                    }
                });
                return taskCompletionSource.Task;
            }

            public Task InvokeOnUiAsync(Func<Task> asyncAction)
            {
                var taskCompletionSource = new TaskCompletionSource<int>();
                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        await asyncAction();
                        taskCompletionSource.SetResult(0);
                    }
                    catch (TaskCanceledException)
                    {
                        taskCompletionSource.SetCanceled();
                    }
                    catch (Exception e)
                    {
                        taskCompletionSource.SetException(e);
                    }
                });
                return taskCompletionSource.Task;
            }
        }
    }
}
