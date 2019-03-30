using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using ActivityMaps.Helpers;
using ActivityMaps.Models;
using ActivityMaps.Views;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;

namespace ActivityMaps.ViewModels
{
    public class CreateActivityViewModel : BaseViewModel
    {
		#region Atributos
		private string colorButton;
		private string buttonText;
		Activity_Category selectedCategory;
		private string activityName;
		private string description;
		private TimeSpan startHour;
		private TimeSpan finishHour;
		private DateTime startDay;
		private DateTime finishDay;
		private bool isVisible = false;
		private List<User> userQuery;
		private User_Log userLog;
		public static User_Log userCreating;
		private string usLog = "3"; //Create activity
		private ObservableCollection<Activity_Category> categories;
		#endregion
		#region Propieades

		public string ButtonText
		{
			get { return this.buttonText; }
			set { SetValue(ref this.buttonText, value); }
		}

		public string ButtonColor
		{
			get { return this.colorButton; }
			set { SetValue(ref this.colorButton, value); }
		}

		public bool IsVisible
		{
			get { return this.isVisible; }
			set { SetValue(ref this.isVisible, value); }
		} 

		public string ActivityName
		{
			get { return this.activityName; }
			set { SetValue(ref this.activityName, value); }
		}

		public string Description
		{
			get { return this.description; }
			set { SetValue(ref this.description, value); }
		}

		public TimeSpan StartHour
		{
			get { return this.startHour; }
			set { SetValue(ref this.startHour, value); }
		}

		public TimeSpan FinishHour
		{
			get { return this.finishHour; }
			set { SetValue(ref this.finishHour, value); }
		}


		public ObservableCollection<Activity_Category> Categories
		{


			get { return this.categories; }
			set { SetValue(ref this.categories, value); }


		}


		public Activity_Category SelectedCategory
		{
			get { return this.selectedCategory; }
			set { SetValue(ref this.selectedCategory, value); }
		}

		public DateTime StartDay
		{
			get { return this.startDay; }
			set { SetValue(ref this.startDay, value); }
		}

		public DateTime FinishDay
		{
			get { return this.finishDay; }
			set { SetValue(ref this.finishDay, value); }
		}

		#endregion
		#region Contrusctores
		public CreateActivityViewModel()
		{
			this.ButtonText = "Select Location";
			this.ButtonColor = "Red";
		}

		public CreateActivityViewModel(List<User> userQuery, User_Log userLog, ObservableCollection<Activity_Category> categories)
		{
		
			this.ButtonText = "Select Location";
			this.ButtonColor = "Red";

			this.userQuery = userQuery;
			this.userLog = userLog;
			this.Categories = categories;
			

		}
		#endregion

		public ICommand Create
		{
			get
			{
				return new RelayCommand(CreateActivity);
			}
		}
		public ICommand LCommand
		{
			get
			{
				return new RelayCommand(Location);
			}
		}

		private async void Location()
		{
			CheckConnectionInternet.checkConnectivity();
			MainViewModel.GetInstance().Location = new LocationViewModel();
			await Application.Current.MainPage.Navigation.PushAsync(new LocationPage());

			this.IsVisible = true;
			this.ButtonColor = "Green";
			this.ButtonText = "Modify Location";


		}

		private async void CreateActivity()
		{
			CheckConnectionInternet.checkConnectivity();
			
			

			if (string.IsNullOrEmpty(this.ActivityName))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter an Activity Name.",
					"Accept");
				return;
			}
			if (string.IsNullOrEmpty(this.Description))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter a description.",
					"Accept");
				return;
			}
			if (SelectedCategory == null)
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must select an Category.",
					"Accept");
				return;
			}
			int len = RandomId.length.Next(5, 10);
			Activity activity = new Activity()
			{
				Id = RandomId.RandomString(len),
				Name = this.ActivityName,
				Created_Date = DateTime.Now,
				IsPrivate = false,//todo
				Start_Act_Datetime = this.StartDay.Date,
				End_Act_Datetime = this.FinishDay.Date,
				Description = this.Description,
				Status = 1,//check
				IsService = false,//todo
				Activity_Cat_Code = SelectedCategory.Id,
				Activity_Loc_Id = "1" //todo
			};
			
			//ctivity history
			Activity_History activityHistory = new Activity_History()
			{
				Id = RandomId.RandomString(len),
				Activity_Code_Id = activity.Id,
				Name = this.ActivityName,
				Created_Date = DateTime.Now,
				IsPrivate = false,//todo
				Start_Act_Date = this.StartDay.Date,
				End_Act_Date = this.FinishDay.Date,
				Description = this.Description,
				Status = 1,//check
				IsService = false,//todo
				Activity_Cat_code = SelectedCategory.Id,
				Activity_Loc_Id_FK = "1" //todo
			};
			userCreating = User_LogType.userLogTypesAsync(userQuery[0].Id, usLog);
			User_Entered entry = new User_Entered()
			{
				Id = RandomId.RandomString(len),
				Status = "in",
				IsCreator = true,
				User_Log_Id_FK1 = userCreating.Id,
				Activity_Code_FK2 = activityHistory.Activity_Code_Id
			};
			try
			{
				await App.MobileService.GetTable<Activity>().InsertAsync(activity);
				await App.MobileService.GetTable<Activity_History>().InsertAsync(activityHistory);
				await App.MobileService.GetTable<User_Log>().InsertAsync(userCreating);
				await App.MobileService.GetTable<User_Entered>().InsertAsync(entry);
				
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
			}
			
			MainViewModel.GetInstance().Activity_Child = new ActivityViewModel(userQuery, entry);
			await Application.Current.MainPage.Navigation.PushAsync(new ActivityPage());
		}

		
		
	}
}
