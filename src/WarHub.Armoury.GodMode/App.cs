// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode
{
    using System.Reflection;
    using Autofac;
    using Views;
    using Xamarin.Forms;

    public class App : Application
    {
        public App()
        {
            ServiceProvider = BuildContainer().BeginLifetimeScope();
            // The root page of your application
            MainPage = new NavigationPage(new MainPage());
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
            builder.RegisterAssemblyModules(GetType().GetTypeInfo().Assembly);
            return builder.Build();
        }
    }
}
