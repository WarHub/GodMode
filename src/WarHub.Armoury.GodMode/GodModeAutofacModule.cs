// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Autofac;
    using Autofac.Core;
    using Model.BattleScribe.Services;
    using Model.DataAccess;
    using Model.DataAccess.Autofac;
    using Mvvm;
    using PCLStorage;
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
            builder.RegisterType<Dispatcher>().As<IDispatcher>();

            // filesystem required by DataAccessModule
            builder.RegisterInstance(FileSystem.Current).As<IFileSystem>();

            // override log registration for DataAccessModule
            builder.RegisterModule<LoggingModule>();

            // repo manager implementation is required by DataAccessModule
            builder.RegisterType<BattleScribeRepoManager>().AsImplementedInterfaces();
        }

        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
        private class LoggingModule : Module
        {
            private static ResolvedParameter LogParameter { get; } = new ResolvedParameter(
                (info, context) => info.ParameterType == typeof(ILog),
                (info, context) => new DebugLogger(info.Member.DeclaringType));

            private static ResolvedParameter AggregateLogParameter(Type aggregateOwnerType)
            {
                return new ResolvedParameter(
                    (info, context) => info.ParameterType == typeof(ILog),
                    (info, context) => new DebugLogger(aggregateOwnerType));
            }

            protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry,
                IComponentRegistration registration)
            {
                registration.Preparing += OnComponentPreparing;
            }

            private static void OnComponentPreparing(object sender, PreparingEventArgs e)
            {
                e.Parameters = e.Parameters.Union(new[]
                {
                    new ResolvedParameter(
                        (info, context) => info.ParameterType.Name.EndsWith("Aggregate"),
                        (info, context) =>
                            context.Resolve(info.ParameterType, AggregateLogParameter(info.Member.DeclaringType)))
                });
                e.Parameters = e.Parameters.Union(new[] {LogParameter});
            }
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

        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
        private class DebugLogger : ILog
        {
            public DebugLogger(Type ownerType)
            {
                OwnerType = ownerType;
            }

            private Type OwnerType { get; }

            public void With(string message)
            {
                Log(message);
            }

            public void With(string message, IDictionary<string, string> properties)
            {
                Log(message);
                Log(properties);
            }

            public void With(Exception e)
            {
                Log(e);
            }

            public void With(Exception e, IDictionary<string, string> properties)
            {
                Log(e);
                Log(properties);
            }

            public void With(string message, Exception exception)
            {
                Log(message);
                Log(exception);
            }

            public void With(string message, Exception exception, IDictionary<string, string> properties)
            {
                Log(message);
                Log(exception);
                Log(properties);
            }

            public ILog Debug => this;

            public ILog Error => this;

            public ILog Info => this;

            public ILog Trace => this;

            public ILog Warn => this;

            private void Log(string message)
            {
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow.ToString("O")}@{OwnerType.FullName}]: {message}");
            }

            private void Log(Exception e)
            {
                Log(e.GetType().FullName);
                Log(e.Message);
                Log(e.StackTrace);
            }

            private void Log(IDictionary<string, string> properties)
            {
                foreach (var pair in properties)
                {
                    Log($"  [{pair.Key}]={pair.Value}");
                }
            }
        }
    }
}
