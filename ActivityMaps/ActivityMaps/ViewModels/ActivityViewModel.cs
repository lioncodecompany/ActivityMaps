

namespace ActivityMaps.ViewModels
{
	using GalaSoft.MvvmLight.Command;
	using Views;
	using System.Windows.Input;
	using Xamarin.Forms;
	using System.Collections.ObjectModel;
	using Models;
	using System.Linq;
	using System.Collections;
	using Plugin.DeviceInfo;
	using ActivityMaps.Helpers;
	using ActivityMaps.AzureStorage;
	using System.IO;
	using System;
	using System.Collections.Generic;
	using System.Text;

	public class ActivityViewModel : BaseViewModel
	{
		#region Atributos

		private string activitytxt;
		private bool isRefreshing;
		private Activity_Child selectedActivity;
		private List<Activity_Child> activityResult;
		//private IEnumerable<Activity_Category> activityCatResult;
		private ObservableCollection<Activity> activities;
		private ObservableCollection<Activity_Location> locations;
		private ObservableCollection<Activity_Category> categories;
		private string categoryName;
		private bool isFilterEmpty = true;
		private string logType = "1"; //login
		private User_Log userLog;
		private ImageSource image;
		private bool isRunning;
		Filter selectedFilter;
		User_Equipment equipment;

		private List<User> userQuery;
		private User_Entered entryUser;
		#endregion

		#region Propiedades


		[Xamarin.Forms.TypeConverter(typeof(Xamarin.Forms.ImageSourceConverter))]
		public Xamarin.Forms.ImageSource Image
		{
			get { return this.image; }
			set { SetValue(ref this.image, value); }
		}

		public IList<Filter> Filters { get { return FilterData.Filters; } }


		public Filter SelectedFilter
		{
			get { return this.selectedFilter; }
			set
			{

				SetValue(ref this.selectedFilter, value);
				//Console.WriteLine("TEXT: {0}", this.SelectedFilter.Name);
				if (this.SelectedFilter.Name == "NO FILTER")
				{
					this.IsFilterEmpty = true;
				}
				else
				{
					this.IsFilterEmpty = false;
				}

				LoadActivity();

			}
		}
		public string CategoryName
		{
			get { return this.categoryName; }
			set
			{

				SetValue(ref this.categoryName, value);

			}
		}


		public string Activitytxt
		{

			get { return this.activitytxt; }
			set
			{

				SetValue(ref this.activitytxt, value);

				LoadActivity();

			}
		}

		public bool IsRefreshing
		{
			get { return this.isRefreshing; }
			set { SetValue(ref this.isRefreshing, value); }
		}

		public bool IsRunning
		{

			get { return this.isRunning; }
			set { SetValue(ref this.isRunning, value); }
		}

		public bool IsFilterEmpty
		{
			get { return this.isFilterEmpty; }
			set { SetValue(ref this.isFilterEmpty, value); }
		}

		public ObservableCollection<Activity> Activities
		{


			get { return this.activities; }
			set { SetValue(ref this.activities, value); }


		}
		public ObservableCollection<Activity_Location> Locations
		{


			get { return this.locations; }
			set { SetValue(ref this.locations, value); }


		}
		public ObservableCollection<Activity_Category> Categories
		{


			get { return this.categories; }
			set { SetValue(ref this.categories, value); }


		}

		public Activity_Child SelectedActivity
		{


			get
			{
				return this.selectedActivity;
			}
			set
			{

				if (this.selectedActivity != value)
				{
					SetValue(ref this.selectedActivity, value);
					Assign();
				}

			}


		}
		public List<Activity_Child> ActivityResult
		{


			get { return this.activityResult; }
			set { SetValue(ref this.activityResult, value); }


		}


		#endregion

		#region Commandos

		public ICommand MenuCommand
		{
			get
			{
				return new RelayCommand(Menu);
			}

		}
		public ICommand SearchCommand
		{
			get
			{
				return new RelayCommand(LoadActivity);
			}

		}
		public ICommand RefreshCommand
		{
			get
			{
				return new RelayCommand(LoadActivity);
			}
		}
		public ICommand CreateCommand
		{
			get
			{
				return new RelayCommand(CreateActivity);
			}
		}
		#endregion

		#region Contrusctores
		public ActivityViewModel()
		{

			this.Activitytxt = "";
			this.IsRefreshing = false;
			this.IsFilterEmpty = true;
			// LoadActivity();
			// fillEquipment();


		}
		public ActivityViewModel(List<User> userQuerry)
		{

			this.Activitytxt = "";
			this.IsRefreshing = false;
			LoadActivity();
			this.userQuery = userQuerry;
			fillEquipment();


		}

		public ActivityViewModel(List<User> userQuerry, User_Entered entry)
		{

			this.Activitytxt = "";
			this.IsRefreshing = false;
			LoadActivity();
			this.userQuery = userQuerry;
			this.entryUser = entry;
			fillEquipment();


		}
		#endregion

		#region Metodos

		private async void Menu()
		{
			CheckConnectionInternet.checkConnectivity();
			MainViewModel.GetInstance().Menu = new MenuViewModel(userQuery, userLog);
			await Application.Current.MainPage.Navigation.PushAsync(new MenuPage());
			LoadActivity();


		}
		private async void CreateActivity()
		{
			CheckConnectionInternet.checkConnectivity();
			MainViewModel.GetInstance().CreateActivity = new CreateActivityViewModel(userQuery, userLog, Categories);
			await Application.Current.MainPage.Navigation.PushAsync(new CreateActivityPage());
			LoadActivity();
		}
		public async void LoadActivity()
		{

			//Activities = ActivityData.Activities;
			try
			{
				var querry = await App.MobileService.GetTable<Activity>().ToListAsync();
				Activities = new ObservableCollection<Activity>();
				var arr = querry.ToArray();
				for (int idx = 0; idx < arr.Length; idx++)
				{
					if (!arr[idx].deleted)
					{
						Activities.Add(new Activity
						{
							Id = arr[idx].Id,
							Description = arr[idx].Description,
							Name = arr[idx].Name,
							Activity_Loc_Id = "1",
							Activity_Cat_Code = arr[idx].Activity_Cat_Code
						});
					}

				}
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
			}
			Locations = Activity_LocationData.Locations;

			try
			{
				var querry = await App.MobileService.GetTable<Activity_Category>().ToListAsync();
				Categories = new ObservableCollection<Activity_Category>();
				var arr = querry.ToArray();
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
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
			}
			this.IsRefreshing = true;
			//Categories = Activity_CategoryData.Categories;

			//this.ActivityResult;
			//var query
			if (this.IsFilterEmpty)
			{
				var query = from act
									  in Activities
							join cat in Categories on act.Activity_Cat_Code equals cat.Id
							join loc in Locations on act.Activity_Loc_Id equals loc.Id
							where (act.Name.ToUpper().StartsWith(this.Activitytxt.ToUpper()))
							||
							(cat.Name.ToUpper().StartsWith(this.Activitytxt.ToUpper()))
							//||
							//(loc.City.ToUpper().StartsWith(this.Activitytxt.ToUpper()))
							select new Activity_Child

							{
								Id = act.Id,
								Name = act.Name,
								CategoryName = cat.Name,
								Description = act.Description

							};
				this.ActivityResult = query.ToList();
			}
			else
			{

				var query = from act
									  in Activities
							join cat in Categories on act.Activity_Cat_Code equals cat.Id
							join loc in Locations on act.Activity_Loc_Id equals loc.Id
							where (act.Name.ToUpper().Contains(this.Activitytxt.ToUpper()))
							&&
							(cat.Name.ToUpper().StartsWith(this.SelectedFilter.Name.ToUpper()))
							//||
							//(loc.City.ToUpper().StartsWith(this.Activitytxt.ToUpper()))
							select new Activity_Child

							{
								Id = act.Id,
								Name = act.Name,
								CategoryName = cat.Name,
								Description = act.Description
							};

				this.ActivityResult = query.ToList();
			}



			this.IsRefreshing = false;
		}


		public async void Assign()
		{
			this.IsRunning = true;
			var actID = this.SelectedActivity.Id;
			string actName = this.SelectedActivity.Name;

			//SetValue(ref this.selectedActivity, null);
			LoadActivity();

			var user = await App.MobileService.GetTable<User_Entered>().Where(p => p.Activity_Code_FK2 == selectedActivity.Id && p.IsCreator).ToListAsync();
			var userCreator = await App.MobileService.GetTable<User_Log>().Where(p => p.Id == user[0].User_Log_Id_FK1).ToListAsync();
			var userName = await App.MobileService.GetTable<User>().Where(p => p.Id == userCreator[0].User_Id_FK2).ToListAsync();


			this.IsRunning = false;
			MainViewModel.GetInstance().ActivityJoin = new ActivityJoinViewModel(this.SelectedActivity, this.userQuery, userName, userCreator, equipment);
			await Application.Current.MainPage.Navigation.PushAsync(new ActivityJoinPage());


			// note name is property in my model (say : GeneralDataModel )
		}

		private async void fillEquipment()
		{

			int len = RandomId.length.Next(5, 10);
			equipment = new User_Equipment
			{
				Id = RandomId.RandomString(len),
				Phone_Model_Num = CrossDeviceInfo.Current.Platform.ToString(),
				Phone_Name = CrossDeviceInfo.Current.DeviceName,
				Phone_Version = CrossDeviceInfo.Current.Version,
				User_Id_FK = userQuery[0].Id

			};

			try
			{
				await App.MobileService.GetTable<User_Equipment>().InsertAsync(equipment);
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
			}

			userLog = new User_Log
			{
				Id = RandomId.RandomString(len),
				LogDateTime = DateTime.Today,
				User_LogType_Id_FK1 = logType,
				User_Equipment_code = equipment.Id,
				User_Id_FK2 = userQuery[0].Id
			};

			try
			{
				await App.MobileService.GetTable<User_Log>().InsertAsync(userLog);
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
			}

		}
		#endregion

	}


}