using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Application = Android.App.Application;

namespace SimpleConnect.Droid
{
    [Activity(Label = "SimpleConnect", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private static IWindowManager windowManager;
        private static FloatView imageView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            
            if (Android.Provider.Settings.CanDrawOverlays(this))
            {
                ShowFloatView();
            }
            else
            {
                var intent = new Intent(Android.Provider.Settings.ActionManageOverlayPermission);
                StartActivityForResult(intent, 0);
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            ShowFloatView();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void ShowFloatView()
        {
            windowManager = GetSystemService(WindowService).JavaCast<IWindowManager>();
            if (imageView != null)
            {
                windowManager.RemoveView(imageView);
            }
            imageView = new FloatView(Application.Context);
            imageView.SetImageResource(Resource.Mipmap.icon);
            imageView.OnClicked += (sender, args) => MessagingCenter.Send((App)Xamarin.Forms.Application.Current, "Click");
        }
    }
}