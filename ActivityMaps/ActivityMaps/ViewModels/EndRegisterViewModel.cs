using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ActivityMaps.Helpers;
using ActivityMaps.Models;
using ActivityMaps.Views;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;

namespace ActivityMaps.ViewModels
{
    public class EndRegisterViewModel : BaseViewModel
    {
		#region Atributos

		public static bool allValidated = false;
		private Address currentAddress;
		private User currentUser;
		private string email;
		private string password;
		private string reEnterPassword;
		

		#endregion

		#region Propiedades

		public string Email
		{
			get { return this.email; }
			set { SetValue(ref this.email, value); }
		}

		public string Password
		{
			get { return this.password; }
			set { SetValue(ref this.password, value); }
		}

		public string ReEnterPassword
		{
			get { return this.reEnterPassword; }
			set { SetValue(ref this.reEnterPassword, value); }
		}

		public Address CurrentAddress
		{
			get { return this.currentAddress; }
			set { SetValue(ref this.currentAddress, value); }

		}

		public User CurrentUser
		{
			get { return this.currentUser; }
			set { SetValue(ref this.currentUser, value); }

		}



		#endregion

		#region Contrusctores


		public EndRegisterViewModel(Address newAddress, User newUSer)
		{
			this.CurrentAddress = newAddress;
			this.CurrentUser = newUSer;
		}

		public EndRegisterViewModel()
		{
		}

		#endregion
		#region Comandos

		public ICommand NextCommand
		{
			get
			{
				return new RelayCommand(NextActivityPage);
			}
		}



		private async void NextActivityPage()
		{

			
			

			if (string.IsNullOrEmpty(this.Email))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter an Email.",
					"Accept");
				return;
			}

			if (string.IsNullOrEmpty(this.Password))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter an Password.",
					"Accept");
				return;
			}

			if (string.IsNullOrEmpty(this.ReEnterPassword))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter an Password.",
					"Accept");
				return;
			}

			if (!(Password.Equals(ReEnterPassword)))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"Passwords not match.",
					"Accept");
				return;
			}

			CurrentUser.Email = Email;
			CurrentUser.IsActive = true;
			CurrentUser.Created_Date = DateTime.Now;


			byte[] encryted = System.Text.Encoding.Unicode.GetBytes(Password);
			var result = Convert.ToBase64String(encryted);
			int len = RandomId.length.Next(5, 10);

			User_Password password = new User_Password
			{
				Id = RandomId.RandomString(len),
				Password = result,
				User_Id_FK = CurrentUser.Id
			};

			try
			{
				await App.MobileService.GetTable<Address>().InsertAsync(CurrentAddress);
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
			}

			try
			{
				await App.MobileService.GetTable<User>().InsertAsync(CurrentUser);
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
			}

			try
			{
				await App.MobileService.GetTable<User_Password>().InsertAsync(password);
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
			}

			MainViewModel.GetInstance().Login = new LoginViewModel(CurrentUser.Email);
			await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());

		}
		#endregion
	}
}
