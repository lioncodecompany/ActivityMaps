using ActivityMaps.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ActivityMaps.Helpers
{
    public class User_LogType
    {
		public async static void userLogTypesAsync(string userId, string logTypeId)
		{
			int len = RandomId.length.Next(5, 10);
			User_Log user = new User_Log
			{
				Id = RandomId.RandomString(len),
				LogDateTime = DateTime.Today,
				User_LogType_Id_FK1 = logTypeId,
				User_Id_FK2 = userId
			};

			try
			{
				await App.MobileService.GetTable<User_Log>().InsertAsync(user);
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
			}
		} 
    }
}
