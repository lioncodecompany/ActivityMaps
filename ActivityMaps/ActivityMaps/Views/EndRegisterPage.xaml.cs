using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActivityMaps.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ActivityMaps.Helpers;
using Microsoft.WindowsAzure.MobileServices;
using ActivityMaps.ViewModels;

namespace ActivityMaps.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EndRegisterPage : ContentPage
	{
		

		public EndRegisterPage ()
		{
			InitializeComponent ();
		}

		private async void Button_Clicked(object sender, EventArgs args)
		{
		}
	}
}