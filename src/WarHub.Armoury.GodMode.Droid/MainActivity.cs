// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Droid
{
    using Android.App;
    using Android.Content.PM;
    using Android.OS;
    using HockeyApp;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.Android;

    [Activity(Label = "WarHub GodMode", Icon = "@drawable/icon", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            LoadApplication(new App());

            CrashManager.Register(this, "e17a1b24223e49eca705acd43e780a31");
            CheckForUpdates();
        }
        private void CheckForUpdates()
        {
            // Remove this for store builds!
            UpdateManager.Register(this, "e17a1b24223e49eca705acd43e780a31");
        }

        private void UnregisterManagers()
        {
            UpdateManager.Unregister();
        }

        protected override void OnPause()
        {
            base.OnPause();
            UnregisterManagers();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            UnregisterManagers();
        }
    }
}
