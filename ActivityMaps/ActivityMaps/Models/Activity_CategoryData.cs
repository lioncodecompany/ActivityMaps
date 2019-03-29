using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace ActivityMaps.Models
{
	public class Activity_CategoryData
	{

		public static ObservableCollection<Activity_Category> Categories { get; private set; }
		public static List<Activity_Category> query;
		public  Activity_CategoryData()
		{
		
	
		}

		public async static void getCategory()
		{
			try
			{
				query = await App.MobileService.GetTable<Activity_Category>().ToListAsync();
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
			}
			Categories = new ObservableCollection<Activity_Category>();
			var arr = query.ToArray();
			for (int idx = 0; idx < arr.Length; idx++)
			{
				Categories.Add(new Activity_Category
				{
					Id = arr[idx].Id,
					Parent = arr[idx].Parent,
					Name = arr[idx].Name
				});
			}
		}
	}
}
