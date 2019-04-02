using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Input;
using ActivityMaps.AzureStorage;
using ActivityMaps.Helpers;
using ActivityMaps.Models;
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
		private bool isRunning;
		List<User_Log> userCreator;
		List<User> userCreatorActivity;
		User_Equipment equipment;
		private string logtypeJoin = "4";
		List<User> userJoining;

		[Xamarin.Forms.TypeConverter(typeof(Xamarin.Forms.ImageSourceConverter))]
		public Xamarin.Forms.ImageSource Image
		{
			get { return this.image; }
			set { SetValue(ref this.image, value); }
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
			this.UserName = this.userJoining[0].Nickname;
			getFile();

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

		private async void Join()
		{

			

			var queue = await App.MobileService.GetTable<User_Entered>().Where(p => p.Activity_Code_FK2 == selectedActivity.Id && !p.IsCreator).ToListAsync();
			
			if (queue.Count > 0)
			{

				for (int i = 0; i < queue.Count; i++)
				{
					var userLogged = await App.MobileService.GetTable<User_Log>().Where(p => p.Id == queue[i].User_Log_Id_FK1 && p.User_LogType_Id_FK1 == "4").ToListAsync();
					if (userLogged[i].User_Id_FK2.Equals(userJoining[0].Id))
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

	}
}
