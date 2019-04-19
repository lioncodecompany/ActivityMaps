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
    public class MenuViewModel : BaseViewModel
    {
		#region Atributos
		private List<User> user;
		private string nickName;
		#endregion

		#region Propieades
		public string NickName
		{
			get { return this.nickName; }
			set
			{
				SetValue(ref this.nickName, value);
			}
		}
		#endregion

		#region Contrusctores
		public MenuViewModel()
		{
			
		}

		public MenuViewModel(List<User> userQuery, User_Log userLog)
		{

			this.user = userQuery;
			NickName = userQuery[0].Nickname;
		}
		#endregion

		#region Comandos
		public ICommand ProfileCommand
		{
			get
			{
				return new RelayCommand(Profile);
			}

		}


		public ICommand ActivityHistoryCommand
		{
			get
			{
				return new RelayCommand(History);
			}

		}


		public ICommand FriendsCommand
		{
			get
			{
				return new RelayCommand(Friends);
			}

		}

		public ICommand FeedbackCommand
		{
			get
			{
				return new RelayCommand(Feedback);
			}

		}
		public ICommand StatisticsCommand
		{
			get
			{
				return new RelayCommand(Statistics);
			}

		}
		public ICommand SettingCommand
		{
			get
			{
				return new RelayCommand(Setting);
			}

		}

		public ICommand AboutCommand
		{
			get
			{
				return new RelayCommand(About);
			}

		}

		public ICommand SignOutCommand
		{
			get
			{
				return new RelayCommand(SignOut);
			}

		}



		#endregion

		#region Metodos
		private async void Profile()
		{
			CheckConnectionInternet.checkConnectivity();
			MainViewModel.GetInstance().Profile = new ProfileViewModel(user);
			await Application.Current.MainPage.Navigation.PushAsync(new ProfilePage());
		}
		private async void Friends()
		{
			CheckConnectionInternet.checkConnectivity();
			MainViewModel.GetInstance().FriendList = new FriendListViewModel(user);
			MainViewModel.GetInstance().PendingFriends = new PendingFriendsViewModel(user);
			MainViewModel.GetInstance().Friends = new FriendsViewModel(user);
			await Application.Current.MainPage.Navigation.PushAsync(new FriendsPage());
		}
		private async void History()
		{
			CheckConnectionInternet.checkConnectivity();
			MainViewModel.GetInstance().History = new HistoryViewModel(user);
			await Application.Current.MainPage.Navigation.PushAsync(new HistoryPage());
		}
		private async void About()
		{
			CheckConnectionInternet.checkConnectivity();
			MainViewModel.GetInstance().About = new AboutViewModel();
			await Application.Current.MainPage.Navigation.PushAsync(new AboutPage());
		}

		private async void Setting()
		{
			CheckConnectionInternet.checkConnectivity();
			MainViewModel.GetInstance().Setting = new SettingViewModel(user);
			await Application.Current.MainPage.Navigation.PushAsync(new SettingPage());
		}

		private async void Statistics()
		{
			CheckConnectionInternet.checkConnectivity();
			MainViewModel.GetInstance().Statistics = new StatisticsViewModel();
			await Application.Current.MainPage.Navigation.PushAsync(new StatisticsPage());
		}

		private async void Feedback()
		{
			CheckConnectionInternet.checkConnectivity();
			MainViewModel.GetInstance().Feedback = new FeedbackViewModel(user);
			await Application.Current.MainPage.Navigation.PushAsync(new FeedbackPage());
		}

		private async void SignOut()
		{
			CheckConnectionInternet.checkConnectivity();
			
			await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
			
		}

		#endregion



	}
}