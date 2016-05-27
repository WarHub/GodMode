// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Threading.Tasks;
    using Autofac;
    using Model.BattleScribe.Services;
    using Model.DataAccess;
    using Model.DataAccess.Autofac;
    using Modules.Home.Commands;
    using Xamarin.Forms;

    public class App : Application
    {
        public App()
        {
            ServiceProvider = BuildContainer();
            // The root page of your application
            MainPage = new NavigationPage();
            ServiceProvider.Resolve<OpenHomeCommand>().Execute();
        }

        public static IComponentContext ServiceProvider { get; private set; }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<GodModeAutofacModule>();
            return builder.Build();
        }
    }
}
