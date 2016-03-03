// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.GodMode.Droid
{
    using Android.App;
    using Android.Content.PM;
    using Android.OS;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.Android;

    [Activity(Label = "WarHub.GodMode", Icon = "@drawable/icon", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            LoadApplication(new App());
        }
    }
}
