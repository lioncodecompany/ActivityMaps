using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ActivityMaps.Models;
using ActivityMaps.Views;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;

namespace ActivityMaps.ViewModels
{
	public class HistoryDetailsViewModel : BaseViewModel
	{
		private List<User> user;
		private Activity_Child selectedActivity;
		
		private bool isRefreshing;
		private User selectedUser;
		private List<User> userResult;
		private List<Entered_History> users;
		private string nickName;
		private bool isRunning;
		private ObservableCollection<User> userName;
		private string activityName;
		private string categoryName;
		private string description;
		private string locationTown;
		private bool isService;
		private string color;
		private string start;
		private string end;
		private string creator;
		#region property

		public bool IsService
		{
			get { return this.isService; }
			set
			{

				SetValue(ref this.isService, value);

			}
		}
		public string End
		{
			get { return this.end; }
			set
			{

				SetValue(ref this.end, value);

			}
		}
		public string Start
		{
			get { return this.start; }
			set
			{

				SetValue(ref this.start, value);

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
		public string LocationTown
		{
			get { return this.locationTown; }
			set
			{

				SetValue(ref this.locationTown, value);

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
		public string Creator
		{
			get { return this.creator; }
			set
			{

				SetValue(ref this.creator, value);

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

		public bool IsRefreshing
		{
			get { return this.isRefreshing; }
			set { SetValue(ref this.isRefreshing, value); }
		}

		public string Nickname
		{
			get { return this.nickName; }
			set { SetValue(ref this.nickName, value); }
		}

		public string ActivityName
		{
			get { return this.activityName; }
			set { SetValue(ref this.activityName, value); }
		}

		public bool IsRunning
		{

			get { return this.isRunning; }
			set { SetValue(ref this.isRunning, value); }
		}



		public List<Entered_History> User
		{


			get { return this.users; }
			set { SetValue(ref this.users, value); }


		}

		public List<User> UserResult
		{


			get { return this.userResult; }
			set { SetValue(ref this.userResult, value); }


		}

		public ObservableCollection<User> UserName
		{
			get { return this.userName; }
			set { SetValue(ref this.userName, value); }
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
					Assign();
				}

			}


		}



		#endregion

		public HistoryDetailsViewModel(List<User> user, Activity_Child selectedActivity)
		{
			this.user = user;
			this.selectedActivity = selectedActivity;
			this.ActivityName = selectedActivity.Name;
			this.CategoryName = selectedActivity.CategoryName;
			this.Description = selectedActivity.Description;
			this.LocationTown = selectedActivity.LocationPlaceName;
			this.IsService = selectedActivity.IsService;
			this.Color = this.IsService ? "Red" : "Gray";
			this.Start = selectedActivity.Start_Act_Datetime.ToString();
			this.End = selectedActivity.End_Act_Datetime.ToString();
			getCreator(selectedActivity.Creator);


			LoadParticipants();
		}
		public HistoryDetailsViewModel()
		{

		}

		#region commandos
		public ICommand RefreshCommand
		{
			get
			{
				return new RelayCommand(LoadParticipants);
			}
		}
		public ICommand SearchCommand
		{
			get
			{
				return new RelayCommand(LoadParticipants);
			}

		}


		public async void LoadParticipants()
		{

			//Activities = ActivityData.Activities;
			try
			{
				var querry = await App.MobileService.GetTable<Entered_History>().Where(p => p.Activity_Code_FK2 == selectedActivity.Id).ToListAsync();
				User = new List<Entered_History>();
				var arr = querry.ToArray();
				for (int idx = 0; idx < arr.Length; idx++)
				{

					User.Add(new Entered_History
					{
						Id = arr[idx].Id,
						UserCreator = arr[idx].UserCreator,
						UserJoin = arr[idx].UserJoin,
						Status = arr[idx].Status
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
				UserName = new ObservableCollection<User>();
				var arr = querry.ToArray();
				for (int idx = 0; idx < arr.Length; idx++)
				{

					UserName.Add(new User
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

			this.IsRefreshing = true;
			//Categories = Activity_CategoryData.Categories;

			//this.ActivityResult;
			//var query
			
			this.User = User
					.GroupBy(p => p.UserJoin)
					.Select(g => g.First())
					.ToList();

			var query = from us in User
						join u in UserName on us.UserJoin equals u.Id

						select new User

						{
							Id = us.UserJoin == us.UserCreator ? us.UserCreator : us.UserJoin,
							Nickname = u.Nickname

						};
			this.UserResult = query.ToList();
			//UserResult.Add(new User
			//{
			//	Id = User[0].UserCreator,
			//	Nickname = User[0].UserCreator
			//});
			this.IsRefreshing = false;
		}
		public async void Assign()
		{
			this.IsRunning = true;
			var querry = await App.MobileService.GetTable<User>().Where(p => p.Id == SelectedUser.Id).ToListAsync();
			MainViewModel.GetInstance().Profile = new ProfileViewModel(querry);
			await Application.Current.MainPage.Navigation.PushAsync(new ProfilePage());
			LoadParticipants();

			this.IsRunning = false;



			// note name is property in my model (say : GeneralDataModel )
		}
		private async void getCreator(string id)
		{
			var querry = await App.MobileService.GetTable<User>().Where(p => p.Id == id).ToListAsync();

			this.Creator = querry[0].Nickname;
		}
		#endregion
	}
}
