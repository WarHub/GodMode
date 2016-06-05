﻿// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.iOS
{
    using Foundation;
    using HockeyApp;
    using UIKit;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.iOS;

    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();
            LoadApplication(new App());

            var manager = BITHockeyManager.SharedHockeyManager;
            manager.Configure("25ef8049cd15437d839129e191a1aaec");
            manager.StartManager();
            manager.Authenticator.AuthenticateInstallation();

            return base.FinishedLaunching(app, options);
        }
    }
}
