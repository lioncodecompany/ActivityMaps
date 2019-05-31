using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Plugin.Connectivity;
using System.Threading.Tasks;
using ActivityMaps.Models;

namespace ActivityMaps.Helpers
{
	public class CheckConnectionInternet
	{

		#region Methods
		public async Task<Response> CheckConnection()
		{
			if (!CrossConnectivity.Current.IsConnected)
			{
				return new Response
				{
					IsSuccess = false,
					Message = "Please turn on your internet settings.",
				};
			}

			var isReachable = await CrossConnectivity.Current.IsRemoteReachable(
				"google.com",80,5000);
			if (!isReachable)
			{
				return new Response
				{
					IsSuccess = false,
					Message = "Check you internet connection.",
				};
			}

			return new Response
			{
				IsSuccess = true,
				Message = "Ok",
			};
		}
		public static async void checkConnectivity()
		{
			CheckConnectionInternet internet = new CheckConnectionInternet();
			var connection = await internet.CheckConnection();

			if (!connection.IsSuccess)
			{
				
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					connection.Message,
					"Accept");
				await Application.Current.MainPage.Navigation.PopAsync();
				return;
			}
			#endregion
		}
	}
}
