using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ActivityMaps
{
	using Microsoft.WindowsAzure.MobileServices;
	using Views;
	public partial class App : Application

	{
		public static MobileServiceClient MobileService =
			new MobileServiceClient("https://activitymaps.azurewebsites.net");



		public App()
		{
			InitializeComponent();

			this.MainPage = new NavigationPage(new LoginPage());
		}

		public static string RUTADB;

		public static object MobileServiceClient { get; internal set; }

		public App(string rutaDB)
		{
			InitializeComponent();

			this.MainPage = new NavigationPage(new LoginPage());

			
			RUTADB = rutaDB;
		}


		protected override void OnStart()
		{
			AppCenter.Start("android=e028cdf2-7933-4798-8592-f0c197108e22;" , typeof(Analytics), typeof(Crashes), typeof(Push));

		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}

	}
}
