using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ActivityMaps.Models;
using Xamarin.Forms;

namespace ActivityMaps.ViewModels
{
	public class StatisticsViewModel : BaseViewModel
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
		public StatisticsViewModel()
		{

		}

		public StatisticsViewModel(List<User> user)
		{
			this.user = user;
			IsRunning = true;
			getCount();
			
			IsRunning = false;
		}
		#endregion

		#region Methods
		private async void getAverageAge(string count)
		{
			if (count.Equals("0"))
			{
				AvgAge = "Non activities played";
				return;

			}
			try
			{
				var querry = await App.MobileService.GetTable<Entered_History>().Where(ent => ent.UserCreator == user[0].Id || ent.UserJoin == user[0].Id).ToListAsync();
				ObservableCollection<Entered_History> entered = new ObservableCollection<Entered_History>();
				var arr = querry.ToArray();
				for (int idx = 0; idx < arr.Length; idx++)
				{
					entered.Add(new Entered_History
					{
						Id = arr[idx].Id,
						Activity_Code_FK2 = arr[idx].Activity_Code_FK2,
						IsCreator = arr[idx].IsCreator,
						Status = arr[idx].Status,
						UserCreator = arr[idx].UserCreator,
						UserJoin = arr[idx].UserJoin

					});

				}
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

				var users = await App.MobileService.GetTable<User>().Where(p => p.Id != user[0].Id).ToListAsync();
				ObservableCollection<User> userS = new ObservableCollection<User>();
				var arr3 = users.ToArray();
				for (int j = 0; j < arr3.Length; j++)
				{
					userS.Add(new User
					{
						Id = arr3[j].Id,
						Birthdate = arr3[j].Birthdate

					});

				}
				var query = from ent
									  in entered
							join act in activity on ent.Activity_Code_FK2 equals act.Activity_Code_Id

							//where (ent.UserCreator != user[0].Id && ent.UserJoin != user[0].Id)
							//group cat.Name by cat.Name into g
							select new Entered_History

							{
								Activity_Code_FK2 = ent.Activity_Code_FK2,
								UserCreator = ent.UserCreator,
								UserJoin = ent.UserJoin

							};
				List<Entered_History> userResult = query.ToList();
				
				var userResultArr = userResult.ToArray();
				List<User> userList = new List<User>();

				
				for (int i = 0; i < userResultArr.Length; i++)
				{
					userList.Add(new User { Id = userResultArr[i].UserCreator });
					userList.Add(new User { Id = userResultArr[i].UserJoin});
				}

				List<User> distinctUser = userList
					.GroupBy(p => p.Id)
					.Select(g => g.First())
					.ToList();
				

				var birthdate = from b in distinctUser
								join u in userS on b.Id equals u.Id
								//where(u.Id == b.Id)
								select new User
								{
									Birthdate = u.Birthdate
								};
				var result = birthdate.ToArray();

				List<int> age = new List<int>();

				for (int i = 0; i < result.Length; i++)
				{
			
					age.Add((DateTime.Now.Year - result[i].Birthdate.Year));
				}
				age.Add(DateTime.Now.Year - user[0].Birthdate.Year);

				AvgAge = String.Format("{0:0.00}", age.Average()); 

			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
			}
		}

		private async void getLocation(string count)
		{
			if (count.Equals("0"))
			{
				Location = "Non activities played";
				return;

			}
			try
			{
				var querry = await App.MobileService.GetTable<Entered_History>().Where(p => (p.UserJoin == user[0].Id || p.UserCreator == user[0].Id) && p.Status != "Aborting").ToListAsync();
				ObservableCollection<Entered_History> entered = new ObservableCollection<Entered_History>();
				var arr = querry.ToArray();
				for (int idx = 0; idx < arr.Length; idx++)
				{
					entered.Add(new Entered_History
					{
						Id = arr[idx].Id,
						Activity_Code_FK2 = arr[idx].Activity_Code_FK2,
						IsCreator = arr[idx].IsCreator,
						Status = arr[idx].Status,
						UserCreator = arr[idx].UserCreator,
						UserJoin = arr[idx].UserJoin

					});

				}
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
				var query = from ent
									  in entered
							join act in activity on ent.Activity_Code_FK2 equals act.Activity_Code_Id
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

		private async void getCategory(string count)
		{
			if (count.Equals("0"))
			{
				Category = "Non activities played";
				return;

			}
			try
			{
				var querry = await App.MobileService.GetTable<Entered_History>().Where(p => p.UserJoin == user[0].Id || p.UserCreator == user[0].Id).ToListAsync();
				ObservableCollection<Entered_History> entered = new ObservableCollection<Entered_History>();
				var arr = querry.ToArray();
				for (int idx = 0; idx < arr.Length; idx++)
				{
						entered.Add(new Entered_History
						{
							Id = arr[idx].Id,
							Activity_Code_FK2 = arr[idx].Activity_Code_FK2,
							IsCreator = arr[idx].IsCreator,
							Status = arr[idx].Status,
							UserCreator = arr[idx].UserCreator,
							UserJoin = arr[idx].UserJoin
						
						});

				}
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
				var query = from ent
									  in entered
							join act in activity on ent.Activity_Code_FK2 equals act.Activity_Code_Id
							join cat in category on act.Activity_Cat_code equals cat.Id
							//group cat.Name by cat.Name into g
							select new Activity_Child

							{
								
								CategoryName = cat.Name,
							

							};
				List<Activity_Child> CategoryResult = query.ToList();
				if(CategoryResult.Count > 0)
				{
					var q = CategoryResult.GroupBy(x => x.CategoryName).Select(x => new {
						Number = x.Count(),
						Name = x.Key,}).OrderByDescending(x => x.Number);

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
				var querry = await App.MobileService.GetTable<Entered_History>().Where(p => (p.UserCreator == user[0].Id || p.UserJoin == user[0].Id) && p.Status == "Out").ToListAsync();
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
			getAverageAge(this.Count);
			getCategory(this.Count);
			getLocation(this.Count);

		}
		#endregion
	} 
}
