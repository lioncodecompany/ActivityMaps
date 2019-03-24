using System;
using System.Collections.Generic;
using System.Text;


namespace ActivityMaps.ViewModels
{
	using GalaSoft.MvvmLight.Command;
	using System.ComponentModel;
	using Views;
	using System.Windows.Input;
	using Xamarin.Forms;
	using ActivityMaps.Models;

	public class LoginViewModel : BaseViewModel
	{
		#region Eventos
		
		#endregion

		#region Atributos

		private string email;
		private bool isEnabled;
		private bool isRunning;
		private bool isRemembered;
		private string password;

		#endregion

		#region Propiedades

		public string Email
		{
			get { return this.email; }
			set { SetValue(ref this.email, value);
				ActivityMaps.Utils.Settings.LastUsedEmail = value;
			}
		}

		public string Password
		{
			get { return this.password; }
			set { SetValue(ref this.password, value);
				
			}
		}

		public bool IsRunning
		{
			get { return this.isRunning; }
			set { SetValue(ref this.isRunning, value); }
		}

		public bool IsRemembered
		{
			get { return this.isRemembered; }
			set { SetValue(ref this.isRemembered, value); }
		}

		public bool IsEnabled
		{
			get { return this.isEnabled; }
			set { SetValue(ref this.isEnabled, value); }
		}

		#endregion

		#region Contrusctores
		public LoginViewModel()
		{
			this.IsRemembered = true;
			this.IsEnabled = true;
			this.Email = ActivityMaps.Utils.Settings.LastUsedEmail;


		}

		public LoginViewModel(string email)
		{
			this.IsRemembered = true;
			this.IsEnabled = true;

			this.Email = email;
			this.Email = ActivityMaps.Utils.Settings.LastUsedEmail;


		}

		#endregion
		#region Comandos

		public ICommand LoginCommand
		{
			get
			{
				return new RelayCommand(Login);
			}
		}

		public ICommand RegisterCommand
		{
			get
			{
				return new RelayCommand(Register);
			}
		}

		private async void Register()
		{

			MainViewModel.GetInstance().Register = new RegisterViewModel();
			await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());
			this.IsRunning = false;
			this.IsEnabled = true;

			//this.Email = string.Empty;
			this.Password = string.Empty;
		}

		private async void Login()
		{
			if (this.IsRemembered)
				this.Email = ActivityMaps.Utils.Settings.LastUsedEmail;

			if (string.IsNullOrEmpty(this.Email))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter an email.",
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

			this.IsRunning = true;
			this.IsEnabled = false;
			var userQuerry = await App.MobileService.GetTable<User>().Where(p => p.Email == Email).ToListAsync();

			var result = string.Empty;
			if (userQuerry.Count > 0)
			{
				var passwordQuerry = await App.MobileService.GetTable<User_Password>().Where(p => p.User_Id_FK == userQuerry[0].Id).ToListAsync();
				if (passwordQuerry.Count > 0)
				{

					byte[] decryted = System.Convert.FromBase64String(passwordQuerry[0].Password);

					result = System.Text.Encoding.Unicode.GetString(decryted);

				}
			}
			else
			{
				this.IsRunning = false;
				this.IsEnabled = true;
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"Email or Password is incorrect.",
					"Accept");
				this.Password = string.Empty;
				return;
			}


			if (this.Email != userQuerry[0].Email || this.Password != result)
			{
				this.IsRunning = false;
				this.IsEnabled = true;
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"Email or Password is incorrect.",
					"Accept");
				this.Password = string.Empty;
				return;
			}

			this.IsRunning = false;
			this.IsEnabled = true;

			//this.Email = string.Empty;
			this.Password = string.Empty;

			MainViewModel.GetInstance().Activity_Child = new ActivityViewModel(userQuerry);

			await Application.Current.MainPage.Navigation.PushAsync(new ActivityPage());

		}

		#endregion
	}
}
