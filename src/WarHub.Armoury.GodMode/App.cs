// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode
{
    using Autofac;
    using Modules.Home.Commands;
    using Xamarin.Forms;

    public class App : Application
    {
        public App()
        {
            var container = BuildContainer();
            
            // The root page of your application
            MainPage = new NavigationPage();

            container.Resolve<OpenHomeCommand>().Execute();
        }

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

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<GodModeAutofacModule>();
            return builder.Build();
        }
    }
}
