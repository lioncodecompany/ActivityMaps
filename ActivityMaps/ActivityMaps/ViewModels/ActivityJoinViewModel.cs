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

namespace ActivityMaps.ViewModels
{
	public class ActivityJoinViewModel : BaseViewModel
	{

		private Activity selectedActivity;
		private string userName;
		private string name;
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

		public string Participantes { get { return this.participantes; } set { SetValue(ref this.participantes, value); } }


		public List<User> UserList { get { return this.userList; }  set { SetValue(ref this.userList, value); } }
		public ObservableCollection<User> UserFoto
		{


			get { return this.userFoto; }
			set { SetValue(ref this.userFoto, value); }


		}

		private User userListView;

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

		public string Description
		{
			get { return this.description; }
			set
			{

				SetValue(ref this.description, value);

			}
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
		public ActivityJoinViewModel(Activity_Child selectedActivity, List<User> userQuery, List<User> userJoining, List<User_Log> userCreator, User_Equipment equipment)
		{
			this.userJoining = userQuery;
			this.equipment = equipment;
			this.userCreator = userCreator;
			this.userCreatorActivity = userJoining;
			this.selectedActivity = selectedActivity;
			this.name = selectedActivity.Name;
			this.categoryName = "Category: "+selectedActivity.CategoryName;
			this.description = selectedActivity.Description;
			this.UserName = userJoining[0].Nickname;
			getFile();
			getFileUserEntry();

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
		

		private async void Quit()
		{


			bool isNotEntered = true;
			List<User_Log> userLogged;
			User_Log current = null;
			int userQuit = 0;

			var queue = await App.MobileService.GetTable<User_Entered>().Where(p => p.Activity_Code_FK2 == selectedActivity.Id).ToListAsync();

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
							User_Equipment_code = userLogged[0].User_Equipment_code,
							User_LogType_Id_FK1 = "5" // quit activity
							
						};
						isNotEntered = false;
					}
				}
			}

			if (!isNotEntered)
			{
				try
				{
					if (!queue[userQuit].IsCreator)
					{
						await App.MobileService.GetTable<User_Log>().UpdateAsync(current);
						await App.MobileService.GetTable<User_Entered>().DeleteAsync(queue[userQuit]);
						await Application.Current.MainPage.DisplayAlert("Success", " You are now out of the activity", "Ok");
						getFileUserEntry();
					}
					else
					{
						await App.MobileService.GetTable<User_Log>().UpdateAsync(current);

						if(queue.Count > 1)
							for (int i = 0; i < queue.Count; i++)
							{
								await App.MobileService.GetTable<User_Entered>().DeleteAsync(queue[i]);
							}
						else
							await App.MobileService.GetTable<User_Entered>().DeleteAsync(queue[0]);



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

			Participantes = "Numero de Participantes: " + userEntry.Count;
			if (userEntry.Count > 0)
			{
				try
				{
					var querry = await App.MobileService.GetTable<User_Entered>().Where(entry => !entry.IsCreator && !entry.deleted && entry.Activity_Code_FK2 == selectedActivity.Id && !entry.deleted).ToListAsync();
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
					
						var querry = await App.MobileService.GetTable<User_Log>().Where(p => p.User_LogType_Id_FK1 == "4").ToListAsync();
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
							Nickname = arr[idx].Nickname
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
								Nickname = user.Nickname,
								ImageUserPath = file.Path

							

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
						Nickname = arr2[i].Nickname,
						ImageUser = this.ImageUser
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

			

			this.IsRunning = false;
		}
		private void queue()
		{

		}

	}
}
