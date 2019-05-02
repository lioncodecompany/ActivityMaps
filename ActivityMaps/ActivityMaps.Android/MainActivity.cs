using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.IO;
using Microsoft.WindowsAzure.MobileServices;
using Android.Support.V4.App;
using Android;
using Plugin.LocalNotifications;

namespace ActivityMaps.Droid
{
    [Activity(Label = "ActivityMaps", Icon = "@drawable/Logo", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : 
        global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
        
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
			
			TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
			

			base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
			CurrentPlatform.Init();

			string nombreArchivo = "DB_ActivityMaps.sqlite";
			string rutaCarpeta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			string rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);

            //Init Google Maps
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.FormsMaps.Init(this, savedInstanceState);

            LoadApplication(new App(rutaCompleta));


        }
        protected override void OnStart()
        {
            base.OnStart();
			LocalNotificationsImplementation.NotificationIconId = Resource.Drawable.Logo;
			//Ponerlo en otra clase
			//if (ActivityCompat.CheckSelfPermission(this, "ACCESS_FINE_LOCATION") != Permission.Granted
			//    && ActivityCompat.CheckSelfPermission(this, "ACCESS_COARSE_LOCATION") != Permission.Granted)
			//{
			//    return;
			//}


			//if (ActivityCompat.CheckSelfPermission(this, "ACCESS_FINE_LOCATION") != Permission.Granted)
   //         {
   //             ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation }, 0);
   //         }
   //         else
   //         {
   //             System.Diagnostics.Debug.WriteLine("Permission Granted!!!");
   //         }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
		{
			Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }
}