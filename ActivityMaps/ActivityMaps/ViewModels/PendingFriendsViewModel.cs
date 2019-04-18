using ActivityMaps.Helpers;
using ActivityMaps.Models;
using ActivityMaps.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ActivityMaps.ViewModels
{
     public class PendingFriendsViewModel : BaseViewModel
    {
		#region Atributos
		private List<User> user;
		private bool isRefreshing;
		private Pending_Friend selectedUser;
		private List<Pending_Friend> userResult;
		private ObservableCollection<Pending_Friend> users;
		private string nickName;
		private bool isRunning;
		private ObservableCollection<User> userName;



		#endregion

		#region property



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

		public bool IsRunning
		{

			get { return this.isRunning; }
			set { SetValue(ref this.isRunning, value); }
		}



		public ObservableCollection<Pending_Friend> User
		{


			get { return this.users; }
			set { SetValue(ref this.users, value); }


		}

		public ObservableCollection<User> UserName
		{
			get { return this.userName; }
			set { SetValue(ref this.userName, value); }
		}


		public Pending_Friend SelectedUser
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

		#region Contructores
		public PendingFriendsViewModel(List<User> user)
		{
			this.user = user;
			LoadPendingFriends();
		}
		public PendingFriendsViewModel()
		{

		}

		public List<Pending_Friend> UserResult
		{


			get { return this.userResult; }
			set { SetValue(ref this.userResult, value); }


		}
		#endregion

		#region commandos
		public ICommand RefreshCommand
		{
			get
			{
				return new RelayCommand(LoadPendingFriends);
			}
		}
		public ICommand SearchCommand
		{
			get
			{
				return new RelayCommand(LoadPendingFriends);
			}

		}


		public async void LoadPendingFriends()
		{

			//Activities = ActivityData.Activities;
			try
			{
				var querry = await App.MobileService.GetTable<Pending_Friend>().ToListAsync();
				User = new ObservableCollection<Pending_Friend>();
				var arr = querry.ToArray();
				for (int idx = 0; idx < arr.Length; idx++)
				{

					User.Add(new Pending_Friend
					{
						Id = arr[idx].Id,
						Requested_By_FK1 = arr[idx].Requested_By_FK1,
						Requested_To_FK2 = arr[idx].Requested_To_FK2,
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

			var query = from act
								  in User
						join u in UserName on act.Requested_By_FK1 equals u.Id
						where (act.Requested_To_FK2.Equals(user[0].Id))

						&&
						(act.Status.Equals("Requested"))
						select new Pending_Friend
						{
							Id = act.Id,
							Requested_By_FK1 = act.Requested_By_FK1,
							Nickname = u.Nickname
						};
			this.UserResult = query.ToList();

			this.IsRefreshing = false;
		}
		public async void Assign()
		{
			this.IsRunning = true;

			//SetValue(ref this.selectedActivity, null);
			//LoadActivity();
			var action = await Application.Current.MainPage.DisplayAlert("Sure?", "Are you sure want to add " + SelectedUser.Nickname, "No", "Yes");
			if (!action)//yess
			{
				
				var checkFriend = await App.MobileService.GetTable<Friend>().Where(p => p.User_Id_FK1 == user[0].Id &&
			p.User_Friend_Id_FK2 == SelectedUser.Requested_By_FK1).ToListAsync();

				if (checkFriend.Count > 0)
				{
					await Application.Current.MainPage.DisplayAlert("Warning", "You have already be a friend of  " + SelectedUser.Nickname, "ok");
					
					return;
				}

				int len = RandomId.length.Next(5, 10);
				Friend pending = new Friend
				{
					Id = RandomId.RandomString(len),
					User_Id_FK1 = user[0].Id,
					User_Friend_Id_FK2 = SelectedUser.Requested_By_FK1,
					Type = 1
				};
				int nl = RandomId.length.Next(5, 10);
				Friend friend= new Friend
				{
					Id = RandomId.RandomString(nl),
					User_Id_FK1 = SelectedUser.Requested_By_FK1,
					User_Friend_Id_FK2 = user[0].Id,
					Type = 1
				};
				try
				{
					await App.MobileService.GetTable<Friend>().InsertAsync(pending);
					await App.MobileService.GetTable<Friend>().InsertAsync(friend);
					await App.MobileService.GetTable<Pending_Friend>().DeleteAsync(SelectedUser);
					await Application.Current.MainPage.DisplayAlert("Succesfuly!", "DONE!", "ok");
					LoadPendingFriends();
					MainViewModel.GetInstance().FriendList = new FriendListViewModel(user);
					await Application.Current.MainPage.Navigation.PushAsync(new FriendsPage());

				}
				catch (Exception ex)
				{
					await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
				}


			}
		
			this.IsRunning = false;



			// note name is property in my model (say : GeneralDataModel )
		}
		#endregion
	}
}
