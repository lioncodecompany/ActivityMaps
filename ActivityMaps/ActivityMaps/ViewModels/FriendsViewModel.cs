﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ActivityMaps.Helpers;
using ActivityMaps.Models;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;

namespace ActivityMaps.ViewModels
{
	public class FriendsViewModel : BaseViewModel
	{
		#region Atributos
		private List<User> user;
		private string usertxt;
		private bool isRefreshing;
		private User selectedUser;
		private List<User> userResult;
		private ObservableCollection<User> users;
		private string nickName;
		private bool isRunning;



		#endregion

		#region property






		public string Usertxt
		{

			get { return this.usertxt; }
			set
			{

				SetValue(ref this.usertxt, value);

				if(!string.IsNullOrEmpty(this.Usertxt) || !Usertxt.Equals(""))
					LoadActivity();

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

		public bool IsRunning
		{

			get { return this.isRunning; }
			set { SetValue(ref this.isRunning, value); }
		}

		

		public ObservableCollection<User> User
		{


			get { return this.users; }
			set { SetValue(ref this.users, value); }


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

		#region Contructores
		public FriendsViewModel(List<User> user)
		{
			this.user = user;
			this.Usertxt = "";
			LoadActivity();
		}
		public FriendsViewModel()
		{
			
		}

		public List<User> UserResult
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
				return new RelayCommand(LoadActivity);
			}
		}
		public ICommand SearchCommand
		{
			get
			{
				return new RelayCommand(LoadActivity);
			}

		}
		public async void LoadActivity()
		{

			//Activities = ActivityData.Activities;
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
							
							where (act.Nickname.ToUpper().StartsWith(this.Usertxt.ToUpper()))
		
							&&
							(!act.Nickname.Equals(user[0].Nickname))
							select new User

							{
								Id = act.Id,
								Nickname = act.Nickname
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
			if(!action)//yess
			{
				var check = await App.MobileService.GetTable<Pending_Friend>().Where(p => p.Requested_By_FK1 == user[0].Id &&
				p.Requested_To_FK2 == SelectedUser.Id).ToListAsync();

				if(check.Count > 0)
				{
					await Application.Current.MainPage.DisplayAlert("Warning", "You have already sent a friend request to  " + SelectedUser.Nickname, "ok");
					this.Usertxt = "";
					LoadActivity();
					return;
				}

				var checkFriend = await App.MobileService.GetTable<Friend>().Where(p => p.User_Id_FK1 == user[0].Id &&
			p.User_Friend_Id_FK2 == SelectedUser.Id).ToListAsync();

				if (checkFriend.Count > 0)
				{
					await Application.Current.MainPage.DisplayAlert("Warning", "You have already be a friend of  " + SelectedUser.Nickname, "ok");
					this.Usertxt = "";
					LoadActivity();
					return;
				}

				int len = RandomId.length.Next(5, 10);
				Pending_Friend pending = new Pending_Friend
				{
					Id = RandomId.RandomString(len),
					Requested_By_FK1 = user[0].Id,
					Requested_To_FK2 = SelectedUser.Id,
					Status = "Requested"
				};

				try
				{
					await App.MobileService.GetTable<Pending_Friend>().InsertAsync(pending);
					await Application.Current.MainPage.DisplayAlert("Succesfuly!", "DONE!", "ok");
				}
				catch (Exception ex)
				{
					await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
				}

				
			}
			else
			{
				this.Usertxt = "";
				LoadActivity();
				return;
			}
			this.IsRunning = false;
		


			// note name is property in my model (say : GeneralDataModel )
		}
		#endregion
	}
}
