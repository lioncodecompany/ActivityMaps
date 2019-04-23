using ActivityMaps.Helpers;
using ActivityMaps.Models;
using ActivityMaps.Views;
using GalaSoft.MvvmLight.Command;
using Plugin.Messaging;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ActivityMaps.ViewModels
{
    public class ResetPasswordViewModel : BaseViewModel
    {
		#region atributos
		private string userEmail;
		private string email;
		private string token;
		private bool step2;
		private bool step3;
		private string code;
		private bool firstButton;
		private bool secondButton;
		private bool thirdButton;
		private bool isRunning;
		List<User> userQuery;
		private string password;
		private string rePassword;
		#endregion

		#region Propiedades
		public string RePassword
		{
			get { return this.rePassword; }
			set { SetValue(ref this.rePassword, value); }
		}
		public string Password
		{
			get { return this.password; }
			set { SetValue(ref this.password, value); }
		}
		public string Email
		{
			get { return this.email; }
			set{ SetValue(ref this.email, value); }
		}
		public string Code
		{
			get { return this.code; }
			set { SetValue(ref this.code, value); }
		}
		public bool IsRunning
		{
			get { return this.isRunning; }
			set { SetValue(ref this.isRunning, value); }
		}

		public bool ThirdButton
		{
			get { return this.thirdButton; }
			set { SetValue(ref this.thirdButton, value); }
		}
		public bool SecondButton
		{
			get { return this.secondButton; }
			set { SetValue(ref this.secondButton, value); }
		}
		public bool FirstButton
		{
			get { return this.firstButton; }
			set { SetValue(ref this.firstButton, value); }
		}
		public bool Step2
		{
			get { return this.step2; }
			set { SetValue(ref this.step2, value); }
		}
		public bool Step3
		{
			get { return this.step3; }
			set { SetValue(ref this.step3, value); }
		}

		#endregion

		#region Contructores
		public ResetPasswordViewModel()
		{

		}

		public ResetPasswordViewModel(string email)
		{
			this.userEmail = email;
			this.Email = email;
			
			this.Step2 = false;
			this.Step3 = false;
		}

		#endregion

		#region Command
		public ICommand CodeCommand
		{
			get
			{
				return new RelayCommand(sendCode);
			}
		}

		public ICommand LastCommand
		{
			get
			{
				return new RelayCommand(getToken);
			}
		}
		public ICommand DoneCommand
		{
			get
			{
				return new RelayCommand(setPassword);
			}
		}

		#endregion
		private async void setPassword()
		{
			this.IsRunning = true;
			
			var passwordQuerry = await App.MobileService.GetTable<User_Password>().Where(p => p.User_Id_FK == userQuery[0].Id).ToListAsync();

			if (string.IsNullOrEmpty(this.Password))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter an Password.",
					"Accept");
				this.IsRunning = false;
				return;
			}
			if (this.Password.Length < 7)
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"Your Password length must be greather than 6.",
					"Accept");
				this.IsRunning = false;
				return;
			}
			if (!(Password.Equals(RePassword)))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"Passwords not match.",
					"Accept");
				this.IsRunning = false;
				return;
			}

			byte[] encryted = System.Text.Encoding.Unicode.GetBytes(Password);
			var result = System.Convert.ToBase64String(encryted);


			User_Password password = new User_Password
			{
				Id = passwordQuerry[0].Id,
				Password = result,
				User_Id_FK = userQuery[0].Id
			};

			await App.MobileService.GetTable<User_Password>().UpdateAsync(password);
			await Application.Current.MainPage.DisplayAlert(
					"Done",
					"Password has been changed.",
					"Accept");
			this.ThirdButton = false;
			this.IsRunning= false;

			MainViewModel.GetInstance().Login = new LoginViewModel(this.Email);
			await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());

		}
		private async void getToken()
		{
			this.IsRunning = true;
			

			if (this.Code.TrimEnd().Equals(token.TrimEnd()))
			{
				var tokenUsed= await App.MobileService.GetTable<Reset_Password_Token>().Where(p => p.User_Id_FK == userQuery[0].Id).ToListAsync();
				Reset_Password_Token updateTokenInfo = new Reset_Password_Token
				{
					Id = tokenUsed[0].Id,
					IsUsed = true,
					Token = tokenUsed[0].Token,
					User_Id_FK = tokenUsed[0].User_Id_FK
					
				};
				await App.MobileService.GetTable<Reset_Password_Token>().UpdateAsync(updateTokenInfo);
				this.IsRunning= false;
				this.Step3 = true;
			}
			else
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"Invalid Token.",
					"Accept");
				this.IsRunning = false;
				return;
			}
			this.IsRunning = false;
			this.SecondButton = false;
		}
		private async void sendCode()
		{
			this.IsRunning = true;
			
			int len = RandomId.length.Next(5, 10);
			token = RandomId.RandomString(len);
			 userQuery = await App.MobileService.GetTable<User>().Where(p => p.Email == Email).ToListAsync();
			if (userQuery.Count > 0)
			{
				try
				{
					SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

					SmtpServer.Port = 587;
					SmtpServer.Host = "smtp.gmail.com";
					SmtpServer.EnableSsl = true;
					SmtpServer.UseDefaultCredentials = false;
					SmtpServer.Credentials = new System.Net.NetworkCredential("lioncodecompany@gmail.com", "leones1234");

					SmtpServer.SendAsync("lioncodecompany@gmail.com", this.Email, "Reset Password", "Copy this token and go back to ActivityMaps!: " + token, "xyz123d");
				}
				catch (Exception ex)
				{
					await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
				}
				
				
				Reset_Password_Token reset = new Reset_Password_Token
				{
					Id = RandomId.RandomString(len),
					Token = token,
					IsUsed = false,
					User_Id_FK = userQuery[0].Id

				};
				await App.MobileService.GetTable<Reset_Password_Token>().InsertAsync(reset);
				this.IsRunning = false;
				this.Step2 = true;
			}
			else
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"Invalid Email.",
					"Accept");
				this.IsRunning = false;
				return;
			}
			this.FirstButton = false;


		}
	}
}
