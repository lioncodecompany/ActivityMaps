using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Input;
using ActivityMaps.AzureStorage;
using ActivityMaps.Helpers;
using ActivityMaps.Models;
using ActivityMaps.Views;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace ActivityMaps.ViewModels
{
	public class ActivityJoinViewModel : BaseViewModel
	{

		private Activity selectedActivity;
		private User selectedUser;
		private string userName;
		private string name;
        private string locationCode;
        private string categoryName;
		private string description;
		private ImageSource image;
		private ImageSource imageUser;
		private bool isRunning;
		List<User_Log> userCreator;
		List<User> userCreatorActivity;
		User_Equipment equipment;
		private string logtypeJoin = "4";
		List<User> userJoining;
		private ObservableCollection<User> userFoto;
		public List<User> userList;
		private string participantes;
		private bool isService;
		private string color;
		private string start;
		private string end;
		private string userRating;
		private string locationRating;

		public string Participantes { get { return this.participantes; } set { SetValue(ref this.participantes, value); } }
		public string Start { get { return this.start; } set { SetValue(ref this.start, value); } }
		public string End { get { return this.end; } set { SetValue(ref this.end, value); } }

		public List<User> UserList { get { return this.userList; }  set { SetValue(ref this.userList, value); } }
		public ObservableCollection<User> UserFoto
		{


			get { return this.userFoto; }
			set { SetValue(ref this.userFoto, value); }


		}
		public User SelectedUser
		{


			get
			{
				return this.selectedUser;
			}
			set
			{

				if (this.selectedUser != value)
				{
					SetValue(ref this.selectedUser, value);
					//Assign();
				}

			}

		}
			[Xamarin.Forms.TypeConverter(typeof(Xamarin.Forms.ImageSourceConverter))]
		public Xamarin.Forms.ImageSource Image
		{
			get { return this.image; }
			set { SetValue(ref this.image, value); }
		}

		[Xamarin.Forms.TypeConverter(typeof(Xamarin.Forms.ImageSourceConverter))]
		public Xamarin.Forms.ImageSource ImageUser
		{
			get { return this.imageUser; }
			set { SetValue(ref this.imageUser, value); }
		}


		public bool IsService
		{
			get { return this.isService; }
			set
			{

				SetValue(ref this.isService, value);

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

		public string UserName
		{
			get { return this.userName; }
			set
			{

				SetValue(ref this.userName, value);

			}
		}
		public string UserRating
		{
			get { return this.userRating; }
			set
			{

				SetValue(ref this.userRating, value);

			}
		}
		public string LocationRating
		{
			get { return this.locationRating; }
			set
			{

				SetValue(ref this.locationRating, value);

			}
		}
		public string Description
		{
			get { return this.description; }
			set
			{

				SetValue(ref this.description, value);

			}
		}
		public string Color
		{
			get { return this.color; }
			set { SetValue(ref this.color, value); }
		}

		public string Name
		{
			get { return this.name; }
			set
			{

				SetValue(ref this.name, value);

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

        public string LocationCode
        {
            get { return this.locationCode; }
            set
            {

                SetValue(ref this.locationCode, value);

            }
        }
        public ActivityJoinViewModel(Activity_Child selectedActivity, List<User> userQuery, List<User> userJoining, List<User_Log> userCreator, User_Equipment equipment)
		{
			this.userJoining = userQuery;
			this.equipment = equipment;
			this.userCreator = userCreator;
			this.userCreatorActivity = userJoining;
			this.selectedActivity = selectedActivity;
			this.name = selectedActivity.Name;
            this.LocationCode = selectedActivity.Activity_Loc_Id;         
            this.categoryName = selectedActivity.CategoryName;
			this.description = selectedActivity.Description;
			this.UserName = userJoining[0].Nickname;
			this.IsService = selectedActivity.IsService;
			this.Start = selectedActivity.Start_Act_Datetime.ToString();
			this.End = selectedActivity.End_Act_Datetime.ToString();
			if (this.IsService)
				this.Color = "Pink";
			getFile();
			getFileUserEntry();
			getUserRating();
			getLocationRating();
			//this.UserName = "me " + userQuery[0].Nickname;
		}

			
		public ActivityJoinViewModel()
		{

		}
		public ICommand JoinCommand
		{
			get
			{
				return new RelayCommand(Join);
			}
		}

        public ICommand LocationMapCommand
        {
            get
            {
                return new RelayCommand(LaunchLocation);
            }

        }


        public ICommand QuitCommand
		{
			get
			{
				return new RelayCommand(Quit);
			}
		}

		public ObservableCollection<User_Log> UserLog { get; private set; }
		public ObservableCollection<User> User { get; private set; }
		public ObservableCollection<File_Path> FilePath { get; private set; }
		public ObservableCollection<User_Entered> UserEntry { get; private set; }
        //public ObservableCollection<Activity_Location> ActivityLocation { get; private set; }


        private async void Quit()
		{


			bool isNotEntered = true;
			List<User_Log> userLogged;
			User_Log current = null;
			int userQuit = 0;

			var queue = await App.MobileService.GetTable<User_Entered>().Where(p => p.Activity_Code_FK2 == selectedActivity.Id).ToListAsync();

			var userStr = await App.MobileService.GetTable<Entered_History>().Where(p => p.Activity_Code_FK2 == selectedActivity.Id).ToListAsync();
			if (queue.Count > 0)
			{

				for (int i = 0; i < queue.Count; i++)
				{
					
					userLogged = await App.MobileService.GetTable<User_Log>().Where(p => p.Id == queue[i].User_Log_Id_FK1 && (p.User_LogType_Id_FK1 == "4" || p.User_LogType_Id_FK1 == "3")).ToListAsync();
					if (userLogged[0].User_Id_FK2.Equals(userJoining[0].Id))
					{
						userQuit = i;
						current = new User_Log
						{
							
							Id = userLogged[0].Id,
							LogDateTime = DateTime.Now,
							User_Id_FK2 = userLogged[0].User_Id_FK2,
							Activity_code = selectedActivity.Id,
							User_Equipment_code = userLogged[0].User_Equipment_code,
							User_LogType_Id_FK1 = "5" // quit activity
							
						};
						isNotEntered = false;
					}
				}
			}

			if (!isNotEntered)
			{
				Entered_History entered = null;
				try
				{
					if (!queue[userQuit].IsCreator)
					{
						await App.MobileService.GetTable<User_Log>().UpdateAsync(current);
						entered = new Entered_History
						{
							Id = queue[userQuit].Id,
							Activity_Code_FK2 = queue[userQuit].Activity_Code_FK2,
							Status = "Aborting",
							IsCreator = queue[userQuit].IsCreator,
							UserJoin = userJoining[0].Id,
							UserCreator = userCreatorActivity[0].Id,
							

						};
						await App.MobileService.GetTable<Entered_History>().UpdateAsync(entered);
						await App.MobileService.GetTable<User_Entered>().DeleteAsync(queue[userQuit]);
						entered = null;
						await Application.Current.MainPage.DisplayAlert("Success", " You are now out of the activity", "Ok");
						getFileUserEntry();
					}
					else
					{
						await App.MobileService.GetTable<User_Log>().UpdateAsync(current);

						if (queue.Count > 1)
							for (int i = 0; i < queue.Count; i++)
							{
								entered = new Entered_History
								{
									Id = queue[i].Id,
									Activity_Code_FK2 = queue[i].Activity_Code_FK2,
									Status = "Out",
									IsCreator = queue[i].IsCreator,
									UserJoin = userStr[i].UserJoin,
									UserCreator = userCreatorActivity[0].Id,
									


								};
								await App.MobileService.GetTable<Entered_History>().UpdateAsync(entered);
								await App.MobileService.GetTable<User_Entered>().DeleteAsync(queue[i]);
								entered = null;
							}
						else
						{
							 entered = new Entered_History
							{
								Id = queue[0].Id,
								Activity_Code_FK2 = queue[0].Activity_Code_FK2,
								Status = "Out",
								IsCreator = queue[0].IsCreator,
								UserJoin = userJoining[0].Id,
								UserCreator = userCreatorActivity[0].Id,

							};
							await App.MobileService.GetTable<Entered_History>().UpdateAsync(entered);
							await App.MobileService.GetTable<User_Entered>().DeleteAsync(queue[0]);
							entered = null;
						}



						await App.MobileService.GetTable<Activity>().DeleteAsync(selectedActivity);
						await Application.Current.MainPage.DisplayAlert("Success", " You are deleted the activity", "Ok");
						MainViewModel.GetInstance().Activity_Child = new ActivityViewModel(userCreatorActivity);
						await Application.Current.MainPage.Navigation.PushAsync(new ActivityPage());
					}
				}
				catch (Exception ex)
				{
					await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
					return;

				}

			}
			else
			{
				await Application.Current.MainPage.DisplayAlert("Error", "You are not on the lobby", "Ok");
				return;
			}
			getFileUserEntry();

		}

		private async void Join()
		{

			

			var queue = await App.MobileService.GetTable<User_Entered>().Where(p => p.Activity_Code_FK2 == selectedActivity.Id && !p.IsCreator).ToListAsync();
			
			if (queue.Count > 0)
			{

				for (int i = 0; i < queue.Count; i++)
				{
					var userLogged = await App.MobileService.GetTable<User_Log>().Where(p => p.Id == queue[i].User_Log_Id_FK1 && p.User_LogType_Id_FK1 == "4").ToListAsync();
					if (userLogged[0].User_Id_FK2.Equals(userJoining[0].Id))
					{
						await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You are in the lobby",
					"Accept");
						return;
					}
				}
			}

			int len = RandomId.length.Next(5, 10);
			var userLog = new User_Log
			{
				Id = RandomId.RandomString(len),
				LogDateTime = DateTime.Today,
				User_LogType_Id_FK1 = logtypeJoin,
				User_Equipment_code = equipment.Id,
				Activity_code = selectedActivity.Id,
				User_Id_FK2 = userJoining[0].Id
			};
			User_Entered entry = new User_Entered()
			{
				Id = RandomId.RandomString(len),
				Status = "in",
				IsCreator = false,
				User_Log_Id_FK1 = userLog.Id,
				Activity_Code_FK2 = selectedActivity.Id
			};
			Entered_History entryHistory = new Entered_History()
			{
				Id = entry.Id,
				Status = "in",
				IsCreator = false,
				Activity_Code_FK2 = selectedActivity.Id,
				UserJoin = userJoining[0].Id,
				UserCreator = userCreatorActivity[0].Id
			};
			if (userJoining[0].Id.Equals(userCreatorActivity[0].Id))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You are the Creator!!",
					"Accept");
				return;
			}

			try
			{
				await App.MobileService.GetTable<User_Log>().InsertAsync(userLog);
				await App.MobileService.GetTable<User_Entered>().InsertAsync(entry);
				await App.MobileService.GetTable<Entered_History>().InsertAsync(entryHistory);
				getFileUserEntry();
				

			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
				return;
			}
			await Application.Current.MainPage.DisplayAlert("Success", " You are now on the queue", "Ok");

		}

		private async void getFile()
		{
			this.IsRunning = true;
			var filePath = await App.MobileService.GetTable<File_Path>().Where(p => p.User_Id_FK == userCreator[0].User_Id_FK2).ToListAsync();

			byte[] imageData = null;
			if (filePath.Count > 0)
			{
				imageData = await AzureStorage.AzureStorage.GetFileAsync(ContainerType.Image, filePath[0].Path);
				Image = ImageSource.FromStream(() => new MemoryStream(imageData));
			}

			this.IsRunning = false;
		}

		private async void getFileUserEntry()
		{
			
			this.IsRunning = true;
			var userEntry = await App.MobileService.GetTable<User_Entered>().Where(p => !p.deleted && p.Activity_Code_FK2 == selectedActivity.Id  && !p.IsCreator).ToListAsync();

			Participantes = "Number of participants: " + (userEntry.Count + 1);
			if (userEntry.Count > 0)
			{
				try
				{
					var querry = await App.MobileService.GetTable<User_Entered>().Where(entry => !entry.IsCreator && !entry.deleted && entry.Activity_Code_FK2 == selectedActivity.Id && !entry.deleted && entry.Status == "in").ToListAsync();
					UserEntry = new ObservableCollection<User_Entered>();
					var arr = querry.ToArray();
					for (int idx = 0; idx < arr.Length; idx++)
					{

						UserEntry.Add(new User_Entered
						{
							Id = arr[idx].Id,
							deleted = arr[idx].deleted,
							Activity_Code_FK2 = arr[idx].Activity_Code_FK2,
							IsCreator = arr[idx].IsCreator,
							User_Log_Id_FK1 = arr[idx].User_Log_Id_FK1
							
							
						});


					}
				}
				catch (Exception ex)
				{
					await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
				}

				try
				{
					
						var querry = await App.MobileService.GetTable<User_Log>().Where(p => p.User_LogType_Id_FK1 == "4" && p.Activity_code == selectedActivity.Id).ToListAsync();
						UserLog = new ObservableCollection<User_Log>();
						var arr = querry.ToArray();

					for (int idx = 0; idx < arr.Length; idx++)
					{
						UserLog.Add(new User_Log
						{
							Id = arr[idx].Id,
							User_Id_FK2 = arr[idx].User_Id_FK2
						});


					}
					
				}
				catch (Exception ex)
				{
					await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
				}

				try
				{
					var querry = await App.MobileService.GetTable<User>().ToListAsync();
					User = new ObservableCollection<User>();
					var arr = querry.ToArray();
					for (int idx = 0; idx < arr.Length; idx++)
					{

						User.Add(new User
						{
							Id = arr[idx].Id,
							Nickname = arr[idx].Nickname,
							Email = arr[idx].Email,
							Name = arr[idx].Name,
							Last_Name = arr[idx].Last_Name,
							Gender = arr[idx].Gender,
							Address_Id_FK = arr[idx].Address_Id_FK
						});


					}
				}
				catch (Exception ex)
				{
					await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
				}

				try
				{
					var querry = await App.MobileService.GetTable<File_Path>().ToListAsync();
					FilePath = new ObservableCollection<File_Path>();
					var arr = querry.ToArray();
					for (int idx = 0; idx < arr.Length; idx++)
					{

						FilePath.Add(new File_Path
						{
							Id = arr[idx].Id,
							User_Id_FK = arr[idx].User_Id_FK,
							Path = arr[idx].Path
						});


					}
				}
				catch (Exception ex)
				{
					await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
				}


				var query =
									from entry in UserEntry
									join usL in UserLog on entry.User_Log_Id_FK1 equals usL.Id
									join user in User on usL.User_Id_FK2 equals user.Id
									join file in FilePath on user.Id equals file.User_Id_FK
							
							select new User

							{
								Id = user.Id,
								Nickname = user.Nickname,
								ImageUserPath = file.Path,
								Email = user.Email,
							    Name = user.Name,
								Last_Name = user.Last_Name,
								Gender = user.Gender,
								Address_Id_FK = user.Address_Id_FK
			

							};
				//	userEntry = query.ToList();
				//this.UserFoto = query.ToList();
				var arr2 = query.ToArray();
				UserFoto = new ObservableCollection<User>();
				for (int i = 0; i < arr2.Length; i++)
				{
					byte[] imageData = null;

					ImageUser = null;
					imageData = await AzureStorage.AzureStorage.GetFileAsync(ContainerType.Image, arr2[i].ImageUserPath);
					this.ImageUser = ImageSource.FromStream(() => new MemoryStream(imageData));

					UserFoto.Add(new User
					{ 
						Id = arr2[i].Id,
						Nickname = arr2[i].Nickname,
						ImageUser = this.ImageUser,
						Email = arr2[i].Email,
						Name = arr2[i].Name,
						Last_Name = arr2[i].Last_Name,
						Gender = arr2[i].Gender,
						Address_Id_FK = arr2[i].Address_Id_FK
					});
					
				}

				this.UserList = UserFoto.ToList();
				//try
				//{

				//		//var userLog = await App.MobileService.GetTable<User_Log>().Where(p => p.Id == userEntry[i].User_Log_Id_FK1).ToListAsync();
				//		var user = await App.MobileService.GetTable<User>().Where(p => p.Id == userLog[i].User_Id_FK2).ToListAsync();
				//	    var filePath = await App.MobileService.GetTable<File_Path>().Where(p => p.User_Id_FK == user[i].Id).ToListAsync();

				//		byte[] imageData = null;
				//		if (filePath.Count > 0)
				//		{
				//			ImageUser = null;
				//			imageData = await AzureStorage.AzureStorage.GetFileAsync(ContainerType.Image, filePath[i].Path);
				//			this.ImageUser = ImageSource.FromStream(() => new MemoryStream(imageData));
				//			UserFoto = new ObservableCollection<User>();
				//			userListView = new User
				//			{
				//				Nickname = user[i].Nickname,
				//				ImageUser = this.ImageUser
				//			};
				//			UserFoto.Add(userListView);
				//		}
				//	}
				//	catch (Exception e)
				//	{
				//		await Application.Current.MainPage.DisplayAlert("Error", e.Message, "Ok");
				//	}


			}
			else
			{
				this.UserList = null;
			}

			

			this.IsRunning = false;
		}
		private void queue()
		{

		}

        private async void LaunchLocation()
        {


            try
            {
                var query = await App.MobileService.GetTable<Activity_Location>().Where(p => p.Id == this.LocationCode).ToListAsync();
                //ActivityLocation = new ObservableCollection<Activity_Location>();
                var arr = query.ToArray();
                //for (int idx = 0; idx < arr.Length; idx++)
                //{

                //    ActivityLocation.Add(new Activity_Location
                //    {
                //        Id = arr[idx].Id,
                //        Latitude = arr[idx].Latitude,
                //        Longitude = arr[idx].Longitude
                //    });


                //}

                var location = new Location((double)arr[0].Latitude, (double)arr[0].Longitude);
                var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };

                await Map.OpenAsync(location, options);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }


        }
		private async void Assign()
		{
			List<User> user = new List<User>();
			user.Add(new User
			{
				Id = SelectedUser.Id,
				Nickname = SelectedUser.Nickname,
				Email = SelectedUser.Email,
				Name = SelectedUser.Name,
				Last_Name = SelectedUser.Last_Name,
				Gender = SelectedUser.Gender,
				Address_Id_FK = SelectedUser.Address_Id_FK
			});
			MainViewModel.GetInstance().Profile= new ProfileViewModel(user);
			await Application.Current.MainPage.Navigation.PushAsync(new ProfilePage());
		}

		private async void getUserRating()
		{
			try
			{
				var query = await App.MobileService.GetTable<User_Rating>().Where(p => p.User_IdReported_FK2 == userCreatorActivity[0].Id).ToListAsync();
				if(query.Count > 0)
				{
					var arr = query.ToArray();
					double avg = 0;
					for (int i = 0; i < arr.Length; i++)
					{
						avg += System.Convert.ToInt32(arr[i].Rating);
					}

					UserRating = "Creator Rating: " + String.Format("{0:0.0}", (avg / arr.Length));
				}
				else
				{
					UserRating = "Creator Rating: 0.0";
				}
			}
			catch (Exception e)
			{
				await Application.Current.MainPage.DisplayAlert("Error", e.Message, "Ok");
			}
		}
		private async void getLocationRating()
		{
			try
			{
				var query = await App.MobileService.GetTable<Location_Rating>().Where(p => p.Activity_Loc_Id_FK == this.LocationCode).ToListAsync();
				if (query.Count > 0)
				{
					var arr = query.ToArray();
					double avg = 0;
					for (int i = 0; i < arr.Length; i++)
					{
						avg += System.Convert.ToInt32(arr[i].Rating);
					}

					LocationRating = "Location Rating: " + String.Format("{0:0.0}", (avg / arr.Length));
				}
				else
				{
					LocationRating = "Location Rating: 0.0";
				}
				var query1 = await App.MobileService.GetTable<Activity_Location>().Where(p => p.Id == this.LocationCode).ToListAsync();
				if (query1[0].IsSecure)
				{
					LocationRating = "Location Rating: Safe";
				}
			}
			catch (Exception e)
			{
				await Application.Current.MainPage.DisplayAlert("Error", e.Message, "Ok");
			}
		}


	}
}
