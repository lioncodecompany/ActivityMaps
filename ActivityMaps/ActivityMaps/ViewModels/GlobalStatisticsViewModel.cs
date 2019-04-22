using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ActivityMaps.Models;
using Xamarin.Forms;

namespace ActivityMaps.ViewModels
{
	public class GlobalStatisticsViewModel : BaseViewModel
	{
		#region Atributos
		private List<User> user;
		private bool isRunning;
		private string count;
		private string category;
		private string location;
		private string avgAge;
		#endregion

		#region properties
		public string AvgAge
		{
			get { return this.avgAge; }
			set
			{
				SetValue(ref this.avgAge, value);
			}
		}
		public string Count
		{
			get { return this.count; }
			set
			{
				SetValue(ref this.count, value);
			}
		}
		public string Location
		{
			get { return this.location; }
			set
			{
				SetValue(ref this.location, value);
			}
		}
		public string Category
		{
			get { return this.category; }
			set
			{
				SetValue(ref this.category, value);
			}
		}
		public bool IsRunning
		{
			get { return this.isRunning; }
			set
			{

				SetValue(ref this.isRunning, value);

			}
		}
		#endregion

		#region Constructores
		public GlobalStatisticsViewModel()
		{

		}

		public GlobalStatisticsViewModel(List<User> user)
		{
			this.user = user;
			IsRunning = true;
			getCount();
			getCategory();
			getLocation();
			getAverageAge();
			IsRunning = false;
		}
		#endregion

		#region Methods
		private async void getAverageAge()
		{
			try
			{
				var querry = await App.MobileService.GetTable<User>().ToListAsync();
				ObservableCollection<User> res = new ObservableCollection<User>();
				var arr = querry.ToArray();
				for (int idx = 0; idx < arr.Length; idx++)
				{
					res.Add(new User
					{
						
						Birthdate = arr[idx].Birthdate

					});

				}
				
				List<int> age = new List<int>();

				for (int i = 0; i < res.Count; i++)
				{

					age.Add((DateTime.Now.Year - res[i].Birthdate.Year));
				}

				AvgAge = String.Format("{0:0.00}", age.Average());

			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
			}
		}

		private async void getLocation()
		{
			try
			{
				
				var activities = await App.MobileService.GetTable<Activity_History>().ToListAsync();
				ObservableCollection<Activity_History> activity = new ObservableCollection<Activity_History>();
				var arr2 = activities.ToArray();
				for (int i = 0; i < arr2.Length; i++)
				{
					activity.Add(new Activity_History
					{
						Id = arr2[i].Id,
						Activity_Cat_code = arr2[i].Activity_Cat_code,
						Activity_Code_Id = arr2[i].Activity_Code_Id,
						Activity_Loc_Id_FK = arr2[i].Activity_Loc_Id_FK



					});

				}

				var localidades = await App.MobileService.GetTable<Activity_Location>().ToListAsync();
				ObservableCollection<Activity_Location> location = new ObservableCollection<Activity_Location>();
				var arr3 = localidades.ToArray();
				for (int j = 0; j < arr3.Length; j++)
				{
					location.Add(new Activity_Location
					{
						Id = arr3[j].Id,
						Nameplace = arr3[j].Nameplace

					});

				}
				var query = from act
									  in activity
							join loc in location on act.Activity_Loc_Id_FK equals loc.Id
							//group cat.Name by cat.Name into g
							select new Activity_Child

							{

								LocationPlaceName = loc.Nameplace,


							};
				List<Activity_Child> CategoryResult = query.ToList();
				if (CategoryResult.Count > 0)
				{
					var q = CategoryResult.GroupBy(x => x.LocationPlaceName).Select(x => new {
						Number = x.Count(),
						Name = x.Key,
					}).OrderByDescending(x => x.Number);

					var array = q.ToArray();
					int max = array[0].Number;

					for (int i = 0; i < array.Length; i++)
					{

						if (array[i].Number > max)
						{
							max = array[i].Number;
							Location = array[i].Name;
						}
						else
						{
							Location = array[0].Name;
						}

					}
				}
				else
				{
					Location = "Non activities played";
				}

			}

			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
			}
		}

		private async void getCategory()
		{
			try { 
				var activities = await App.MobileService.GetTable<Activity_History>().ToListAsync();
				ObservableCollection<Activity_History> activity = new ObservableCollection<Activity_History>();
				var arr2 = activities.ToArray();
				for (int i = 0; i < arr2.Length; i++)
				{
					activity.Add(new Activity_History
					{
						Id = arr2[i].Id,
						Activity_Cat_code = arr2[i].Activity_Cat_code,
						Activity_Code_Id = arr2[i].Activity_Code_Id


					});

				}

				var categories = await App.MobileService.GetTable<Activity_Category>().ToListAsync();
				ObservableCollection<Activity_Category> category = new ObservableCollection<Activity_Category>();
				var arr3 = categories.ToArray();
				for (int j = 0; j < arr3.Length; j++)
				{
					category.Add(new Activity_Category
					{
						Id = arr3[j].Id,
						Name = arr3[j].Name

					});

				}
				var query = from act
									  in activity
							join cat in category on act.Activity_Cat_code equals cat.Id
							//group cat.Name by cat.Name into g
							select new Activity_Child

							{

								CategoryName = cat.Name,


							};
				List<Activity_Child> CategoryResult = query.ToList();
				if (CategoryResult.Count > 0)
				{
					var q = CategoryResult.GroupBy(x => x.CategoryName).Select(x => new {
						Number = x.Count(),
						Name = x.Key,
					}).OrderByDescending(x => x.Number);

					var array = q.ToArray();
					int max = array[0].Number;

					for (int i = 0; i < array.Length; i++)
					{

						if (array[i].Number > max)
						{
							max = array[i].Number;
							Category = array[i].Name;
						}
						else
						{
							Category = array[0].Name;
						}

					}
				}
				else
				{
					Category = "Non activities played";
				}

			}

			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
			}
		}


		private async void getCount()
		{
			try
			{
				var querry = await App.MobileService.GetTable<Activity_History>().ToListAsync();
				if (querry.Count == 0)
				{
					this.Count = 0.ToString();
				}
				else
				{
					this.Count = (querry.Count).ToString();
				}
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
			}
		}
		#endregion
	}
}
