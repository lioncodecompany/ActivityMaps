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



		#endregion

		#region Metodos
		private async void Profile()
		{
			CheckConnectionInternet.checkConnectivity();
			MainViewModel.GetInstance().Profile = new ProfileViewModel(user);
			await Application.Current.MainPage.Navigation.PushAsync(new ProfilePage());
		}
		private void Friends()
		{
			throw new NotImplementedException();
		}
		private void History()
		{
			throw new NotImplementedException();
		}
		private void About()
		{
			throw new NotImplementedException();
		}

		private void Setting()
		{
			throw new NotImplementedException();
		}

		private void Statistics()
		{
			throw new NotImplementedException();
		}

		private void Feedback()
		{
			throw new NotImplementedException();
		}

		#endregion



	}
}