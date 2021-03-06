﻿

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
	using Plugin.LocalNotifications;

	public class ActivityViewModel : BaseViewModel
	{
		#region Atributos

		private string activitytxt;
		private bool isRefreshing;
		private Activity_Child selectedActivity;
        private Activity_Child pselectedActivity;
        private List<Activity_Child> activityResult;
		//private IEnumerable<Activity_Category> activityCatResult;
		private ObservableCollection<Activity> activities;
		private ObservableCollection<Activity_Location> locations;
        private ObservableCollection<User_Entered> users_entered;
        private ObservableCollection<Activity_Category> categories;
		private string categoryName;
		private bool isFilterEmpty = true;
		private string logType = "1"; //login
		private User_Log userLog;
		private ImageSource image;
		private bool isRunning;
		Filter selectedFilter;
		User_Equipment equipment;
        private bool isActivityFound;
        private string startActFormat;

        private List<User> userQuery;
		private User_Entered entryUser;
		private string color;
        private int countPeople;
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
        public string StartActFormat
        {
			get { return this.startActFormat; }
			set
			{

				SetValue(ref this.startActFormat, value);

			}
		}

		public string Color
		{
			get { return this.color; }
			set
			{

				SetValue(ref this.color, value);

			}
		}

        public int CountPeople
        {
            get { return this.countPeople; }
            set
            {

                SetValue(ref this.countPeople, value);

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

        public bool IsActivityFound
        {

            get { return this.isActivityFound; }
            set { SetValue(ref this.isActivityFound, value); }
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
        public ObservableCollection<User_Entered> Users_Entered
        {


            get { return this.users_entered; }
            set { SetValue(ref this.users_entered, value); }


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

			//this.Activitytxt = "";
			this.IsRefreshing = false;
			this.IsFilterEmpty = true;
            this.IsActivityFound = false;
            // LoadActivity();
            // fillEquipment();


        }
		public ActivityViewModel(List<User> userQuerry)
		{

			this.Activitytxt = "";
			this.IsRefreshing = false;
            this.IsActivityFound = false;
            LoadActivity();
			this.userQuery = userQuerry;
			fillEquipment();
			//notification();


		}

		public ActivityViewModel(List<User> userQuerry, User_Entered entry)
		{

			this.Activitytxt = "";
			this.IsRefreshing = false;
            this.IsActivityFound = false;
            LoadActivity();
			this.userQuery = userQuerry;
			this.entryUser = entry;
			fillEquipment();
			//notification();

		}
		#endregion

		#region Metodos
		
		private async void Menu()
		{
			CheckConnectionInternet.checkConnectivity();
			MainViewModel.GetInstance().Menu = new MenuViewModel(userQuery, userLog);
			await Application.Current.MainPage.Navigation.PushAsync(new MenuPage());
			//LoadActivity();


		}
		private async void CreateActivity()
		{
			CheckConnectionInternet.checkConnectivity();
			MainViewModel.GetInstance().CreateActivity = new CreateActivityViewModel(userQuery, userLog, Categories);
			await Application.Current.MainPage.Navigation.PushAsync(new CreateActivityPage());
			//LoadActivity();
		}
		public async void LoadActivity()
		{

			//Activities = ActivityData.Activities;
			try
			{
				UpadateListDB();
				var querry = await App.MobileService.GetTable<Activity>().Where(p => p.End_Act_Datetime > DateTime.Now).ToListAsync();
                var query2 = await App.MobileService.GetTable<Activity_Location>().ToListAsync();
                var queryUsers = await App.MobileService.GetTable<User_Entered>().ToListAsync();
                Activities = new ObservableCollection<Activity>();
                Locations = new ObservableCollection<Activity_Location>();
                Users_Entered = new ObservableCollection<User_Entered>();
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
							Activity_Loc_Id = arr[idx].Activity_Loc_Id,
							Activity_Cat_Code = arr[idx].Activity_Cat_Code,
							Created_Date = arr[idx].Created_Date,
							IsService = arr[idx].IsService,
							Start_Act_Datetime = arr[idx].Start_Act_Datetime,
							End_Act_Datetime = arr[idx].End_Act_Datetime
						});
					}

				}

                var arr2 = query2.ToArray();
                for (int idx = 0; idx < arr2.Length; idx++)
                {
                   
                        Locations.Add(new Activity_Location
                        {
                            Id = arr2[idx].Id,
                            Nameplace = arr2[idx].Nameplace,
                            City = arr2[idx].City,
                            State = arr2[idx].State,
                            Country = arr2[idx].Country,
                            Latitude = arr2[idx].Latitude,
                            Longitude = arr2[idx].Longitude
                        });
                    }


                var arr3 = queryUsers.ToArray();
                for (int idx = 0; idx < arr3.Length; idx++)
                {
                    Console.WriteLine("Show: "+arr3[idx].Activity_Code_FK2.ToString() +" ***");
                    Users_Entered.Add(new User_Entered
                    {
                        Id = arr3[idx].Id,
                        Status = arr3[idx].Status,
                        IsCreator = arr3[idx].IsCreator,
                        User_Log_Id_FK1 = arr3[idx].User_Log_Id_FK1,
                        Activity_Code_FK2 = arr3[idx].Activity_Code_FK2,
                        deleted = arr3[idx].deleted
                    });
                }
            }
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
			}
            //Locations = Activity_LocationData.Locations;
            //try
            //{

            //    var query2 = await App.MobileService.GetTable<Activity_Location>().ToListAsync();

            //    Locations = new ObservableCollection<Activity_Location>();


            //    var arr2 = query2.ToArray();
            //    for (int idx = 0; idx < arr2.Length; idx++)
            //    {

            //        await Application.Current.MainPage.DisplayAlert("Error", arr2[idx].Id, "Ok");
            //        Locations.Add(new Activity_Location
            //        {
            //            Id = arr2[idx].Id,
            //            Nameplace = arr2[idx].Nameplace,
            //            City = arr2[idx].City,
            //            State = arr2[idx].State,
            //            Country = arr2[idx].Country,
            //            Latitude = arr2[idx].Latitude,
            //            Longitude = arr2[idx].Longitude
            //        });
            //    }



            //}
            //catch (Exception ex)
            //{
            //    await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            //}


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
            this.IsActivityFound = false;
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
                            ||
                           (loc.City.ToUpper().StartsWith(this.Activitytxt.ToUpper()))
                            orderby act.Start_Act_Datetime ascending
                            select new Activity_Child

                            {
                                Id = act.Id,
                                Name = act.Name,
                                CategoryName = cat.Name,
                                Description = act.Description,
                                LocationTown = loc.City != "" ? "Location: " + loc.City : "Location: Unknown",
								Town = loc.City,
                                Created_Date = act.Created_Date,
                                Activity_Loc_Id = act.Activity_Loc_Id,
                                IsService = act.IsService,
                                Start_Act_Datetime = act.Start_Act_Datetime,
                                StartActFormat = act.Start_Act_Datetime.Date == DateTime.Now.Date ? "Today " + act.Start_Act_Datetime.ToString("h:mm tt") :
                                                 act.Start_Act_Datetime.Date == DateTime.Now.AddDays(1).Date ? "Tomorrow " + act.Start_Act_Datetime.ToString("h:mm tt") :
                                                 act.Start_Act_Datetime.ToString("ddd, MMM d yyyy h:mm tt"),
                                End_Act_Datetime = act.End_Act_Datetime,
                                Color = act.IsService ? "Green" : "Gray",
                                CountPeople = (from users in users_entered
                                               where (users.Activity_Code_FK2.Equals(act.Id)) select users).Count()


							};
				this.ActivityResult = query.ToList();

			}
			else
			{

				var query = from act
									  in Activities
							join cat in Categories on act.Activity_Cat_Code equals cat.Id
							join loc in Locations on act.Activity_Loc_Id equals loc.Id
							where (act.Name.ToUpper().Contains(this.Activitytxt.ToUpper()) &&
                            (cat.Name.ToUpper().StartsWith(this.SelectedFilter.Name.ToUpper())))
							||
							(loc.City.ToUpper().StartsWith(this.Activitytxt.ToUpper()) &&
                            (cat.Name.ToUpper().StartsWith(this.SelectedFilter.Name.ToUpper())))
                            orderby act.Start_Act_Datetime ascending
                            select new Activity_Child

							{
								Id = act.Id,
								Name = act.Name,
								CategoryName = cat.Name,
								Description = act.Description,
								LocationTown = loc.City != "" ? "Location: " + loc.City : "Location: Unknow",
								Town = loc.City,
								Created_Date = act.Created_Date,
								Activity_Loc_Id = act.Activity_Loc_Id,
								IsService = act.IsService,
								Start_Act_Datetime = act.Start_Act_Datetime,
                                StartActFormat = act.Start_Act_Datetime.Date == DateTime.Now.Date ? "Today " + act.Start_Act_Datetime.ToString("h:mm tt") : 
                                                 act.Start_Act_Datetime.Date == DateTime.Now.AddDays(1).Date ? "Tomorrow " + act.Start_Act_Datetime.ToString("h:mm tt") :
                                                 act.Start_Act_Datetime.ToString("ddd,MMM d yyyy h:mm tt"),
                                End_Act_Datetime = act.End_Act_Datetime,
								Color = act.IsService ? "Green" : "Gray",
								CountPeople = (from users in users_entered
											   where (users.Activity_Code_FK2.Equals(act.Id))
											   select users).Count()


							};

				this.ActivityResult = query.ToList();
			}
			if(ActivityResult.Count > 0 )
				notification(ActivityResult);
            else
                this.IsActivityFound = true; // si no hay actividades aparece el label



            this.IsRefreshing = false;
		}


		public async void Assign()
		{
			this.IsRunning = true;
			var actID = this.SelectedActivity.Id;
			string actName = this.SelectedActivity.Name;
            pselectedActivity = this.SelectedActivity;
            SetValue(ref this.selectedActivity, null);
            //LoadActivity();

            var user = await App.MobileService.GetTable<User_Entered>().Where(p => p.Activity_Code_FK2 == pselectedActivity.Id && p.IsCreator).ToListAsync();
			var userCreator = await App.MobileService.GetTable<User_Log>().Where(p => p.Id == user[0].User_Log_Id_FK1).ToListAsync();
			var userName = await App.MobileService.GetTable<User>().Where(p => p.Id == userCreator[0].User_Id_FK2).ToListAsync();


			this.IsRunning = false;
			MainViewModel.GetInstance().ActivityJoin = new ActivityJoinViewModel(pselectedActivity, this.userQuery, userName, userCreator, equipment);
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
		private async void notification(List<Activity_Child> activities)
		{
			//CrossLocalNotifications.Current.Show("Welcome", "TEST");
			int count = 0;
			List<Activity_Category> catCode = new List<Activity_Category>();
			var isNotification = await App.MobileService.GetTable<User_Setting>().Where(p => p.User_Id_FK2 == userQuery[0].Id).ToListAsync();
			var fav = await App.MobileService.GetTable<Activity_Preference>().Where(p => p.User_Id_FK == userQuery[0].Id).ToListAsync();
			if(fav.Count > 0 && isNotification[0].SendNotification)
			{
				catCode = await App.MobileService.GetTable<Activity_Category>().Where(p => p.Id == fav[0].Activity_Cat_code).ToListAsync();
				var querry = await App.MobileService.GetTable<Address>().Where(p => p.Id == userQuery[0].Address_Id_FK).ToListAsync();
				bool alert = false;

				for (int i = 0; i < activities.Count; i++)
				{

					double day = DateTime.Now.Day - activities[i].Created_Date.Day;
					double hour = DateTime.Now.Hour - activities[i].Created_Date.Hour;
					if (querry[0].City.ToUpper().Equals(activities[i].Town.ToUpper()) && (hour <= 1 && hour >= 0) 
						&& (day < 1 && day >= 0) && catCode[0].Name == activities[i].CategoryName)
					{
						alert = true;
						count = i;
					}

				}

				if (alert)
				{
					CrossLocalNotifications.Current.Show("Activity Found", "Antoher user has been created your favorite activity\n" +
						"Name: " + activities[count].Name +
						"\nCategory: " + activities[count].CategoryName);
				
				}
			}
			
		}
		private async void UpadateListDB()
		{
			try
			{
				var querry = await App.MobileService.GetTable<Activity>().Where(p => p.End_Act_Datetime < DateTime.Now).ToListAsync();
				var query2 = await App.MobileService.GetTable<Entered_History>().Where(p => p.Status == "in").ToListAsync();
                var query3 = await App.MobileService.GetTable<User_Entered>().Where(p => p.Status == "in").ToListAsync();

                if (querry.Count > 0 && query2.Count > 0)
				{
					ObservableCollection<Activity> Activities = new ObservableCollection<Activity>();
					ObservableCollection<Entered_History> Entered = new ObservableCollection<Entered_History>();
                    ObservableCollection<User_Entered> Queue = new ObservableCollection<User_Entered>();
                    var arr = querry.ToArray();
					for (int idx = 0; idx < arr.Length; idx++)
					{
						if (!arr[idx].deleted && arr.Length > 0)
						{
							Activities.Add(new Activity
							{
								Id = arr[idx].Id,
								Description = arr[idx].Description,
								Name = arr[idx].Name,
								Activity_Loc_Id = arr[idx].Activity_Loc_Id,
								Activity_Cat_Code = arr[idx].Activity_Cat_Code,
								Created_Date = arr[idx].Created_Date,
								IsService = arr[idx].IsService,
								Start_Act_Datetime = arr[idx].Start_Act_Datetime,
								End_Act_Datetime = arr[idx].End_Act_Datetime
							});
						}

					}

					var arr2 = query2.ToArray();
					for (int idx = 0; idx < arr2.Length; idx++)
					{

						Entered.Add(new Entered_History
						{
							Id = arr2[idx].Id,
							Status = arr2[idx].Status,
							IsCreator = arr2[idx].IsCreator,
							Activity_Code_FK2 = arr2[idx].Activity_Code_FK2,
							deleted = arr2[idx].deleted,
							UserJoin = arr2[idx].UserJoin,
							UserCreator = arr2[idx].UserCreator
						});
					}
                    var queue = query3.ToArray();
                    for (int idx = 0; idx < queue.Length; idx++)
                    {

                        Queue.Add(new User_Entered
                        {
                            Id = queue[idx].Id,
                            Status = queue[idx].Status,
                            IsCreator = queue[idx].IsCreator,
                            Activity_Code_FK2 = queue[idx].Activity_Code_FK2,
                            deleted = queue[idx].deleted
                            
                        });
                    }

					var queryEnt = from ent in Entered
								   join act in Activities on ent.Activity_Code_FK2 equals act.Id

								   select new Entered_History

								   {
									   Id = ent.Id,
									   Status = "Completed",
									   IsCreator = ent.IsCreator,
									   Activity_Code_FK2 = ent.Activity_Code_FK2,
									   deleted = ent.deleted,
									   UserJoin = ent.UserJoin,
									   UserCreator = ent.UserCreator
								   };

                    var queryQueue = from ent in Queue
                                   join act in Activities on ent.Activity_Code_FK2 equals act.Id

                                   select new User_Entered

                                   {
                                       Id = ent.Id,
                                      
                                   };

                    List<User_Entered> user_Entereds = queryQueue.ToList();
                    var arrQueue = user_Entereds.ToArray();


                    List<Entered_History> entered_Histories = queryEnt.ToList();
					var arrEnt = entered_Histories.ToArray();

                    for (int j = 0; j < arrEnt.Length; j++)
                    {
                        await App.MobileService.GetTable<Entered_History>().UpdateAsync(arrEnt[j]);
                    }

                    for (int x = 0; x < arrQueue.Length; x++)
                    {
                        await App.MobileService.GetTable<User_Entered>().DeleteAsync(arrQueue[x]);
                    }
                    for (int i = 0; i < querry.Count; i++)
					{
						await App.MobileService.GetTable<Activity>().DeleteAsync(querry[i]);
					}
					
				}
				else
				{
					return;
				}
				
			}
			catch (Exception ex)
			{
				//await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
			}
		}
		#endregion

	}


}