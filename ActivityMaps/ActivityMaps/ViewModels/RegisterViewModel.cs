using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using ActivityMaps.Models;
using ActivityMaps.Views;
using ActivityMaps.Helpers;
using Microsoft.WindowsAzure.MobileServices;
using System.Linq;

namespace ActivityMaps.ViewModels
{
    public class RegisterViewModel : BaseViewModel
	{
		#region Atributos

		private string name;
		private string lastName;
		private string nickname;
		Gender selectedGender;
		private DateTime birthdate;
		private string phone;
		private string address;
		private string city;
		private string state;
		Country selectedCountry;
		private string zipCode;
		private bool isRunning;

		#endregion

		#region Propiedades

		public bool IsRunning
		{
			get { return this.isRunning; }
			set { SetValue(ref this.isRunning, value); }
		}

		public string Name
		{
			get { return this.name; }
			set { SetValue(ref this.name, value); }
		}

		public string Last_Name
		{
			get { return this.lastName; }
			set { SetValue(ref this.lastName, value); }
		}

		public string Nickname
		{
			get { return this.nickname; }
			set { SetValue(ref this.nickname, value); }
		}

		public IList<Gender> Genders { get { return GenderData.Genders; } }


		public Gender SelectedGender
		{
			get { return this.selectedGender; }
			set { SetValue(ref this.selectedGender, value); }
		}

		public DateTime Birthdate
		{
			get { return this.birthdate; }
			set { SetValue(ref this.birthdate, value); }
		}

		public string Phone
		{
			get { return this.phone; }
			set { SetValue(ref this.phone, value); }
		}

		public string Address
		{
			get { return this.address; }
			set { SetValue(ref this.address, value); }
		}

		public string City
		{
			get { return this.city; }
			set { SetValue(ref this.city, value); }
		}

		public string State
		{
			get { return this.state; }
			set { SetValue(ref this.state, value); }
		}

		public IList<Country> Countries { get { return CountryData.Countries; } }

		public Country SelectedCountry
		{
			get { return this.selectedCountry; }
			set { SetValue(ref this.selectedCountry, value); }
		}

		public string Zip_Code
		{
			get { return this.zipCode; }
			set { SetValue(ref this.zipCode, value); }
		}

		#endregion

		#region Contrusctores

		public RegisterViewModel()
		{
			
		}

		#endregion
		#region Comandos

		public ICommand NextCommand
		{
			get
			{
				return new RelayCommand(Next);
			}
		}

		

		private async void Next()
		{
			this.IsRunning = true;
			CheckConnectionInternet.checkConnectivity();

			if (string.IsNullOrEmpty(this.Name))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter an Name.",
					"Accept");
				this.IsRunning = false;
				return;
			}

			if (string.IsNullOrEmpty(this.Last_Name))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter an Last Name.",
					"Accept");
				this.IsRunning = false;
				return;
			}

			if (string.IsNullOrEmpty(this.Nickname))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter an Nickname.",
					"Accept");
				this.IsRunning = false;
				return;
			}
			if (this.Nickname.Length < 4)
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"The length of Nickname most be greather than 3 letters.",
					"Accept");
				this.IsRunning = false;
				return;
			}

			var nick = await App.MobileService.GetTable<User>().Where(p => p.Nickname == this.Nickname ).ToListAsync();
			if(nick.Count > 0)
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"This nickname has been taken.",
					"Try Another");
				this.IsRunning = false;
				return;

			}


			if (string.IsNullOrEmpty(this.Phone))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter an Phone.",
					"Accept");
				this.IsRunning = false;
				return;
			}
			if (this.Phone.Length < 10)
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter a Phone with 10 digits.",
					"Accept");
				this.IsRunning = false;
				return;
			}

			if (string.IsNullOrEmpty(this.Address))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter an Address.",
					"Accept");
				this.IsRunning = false;
				return;
			}

			if (string.IsNullOrEmpty(this.City))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter an City.",
					"Accept");
				return;
			}

			if (string.IsNullOrEmpty(this.State))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter an State.",
					"Accept");
				return;
			}

			if (string.IsNullOrEmpty(this.Zip_Code))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter an Zip Code.",
					"Accept");
				this.IsRunning = false;
				return;
			}
			
			if(SelectedGender == null)
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must select an Gender.",
					"Accept");
				this.IsRunning = false;
				return;
			}
			
			
			if (SelectedCountry == null)
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must select an Country.",
					"Accept");
				this.IsRunning = false;
				return;
			}


			int len = RandomId.length.Next(5, 10);

			Address newAddress = new Address()
			{
				
				Id = RandomId.RandomString(len),
				Phone = this.Phone.TrimEnd(),
				Address1 = this.Address.TrimEnd(),
				Address2 = string.Empty,
				City = this.City.TrimEnd(),
				State = this.State.TrimEnd(),
				Country = this.SelectedCountry.Name.TrimEnd(),
				Zipcode = this.Zip_Code.TrimEnd()

			};

			User newUSer = new User()
			{
				Id = RandomId.RandomString(len),
				Name = this.Name.TrimEnd(),
				Last_Name = this.Last_Name.TrimEnd(),
				Nickname = this.Nickname.TrimEnd(),
				Gender = this.SelectedGender.Name.Substring(0,1),
				Birthdate = this.Birthdate.Date,
				IsAdmin = false,
				Locked = false,
				Address_Id_FK = newAddress.Id

			};

			/*
			try
			{
				
				await App.MobileService.GetTable<Address>().InsertAsync(newAddress);
				await Application.Current.MainPage.DisplayAlert("Exito", "El contacto fue insertado", "Ok");
				await Application.Current.MainPage.Navigation.PushAsync(new EndRegisterPage());
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error ", ex.Message, "Ok");
			}
			*/

			//new EndRegisterViewModel(newAddress);
			//MainViewModel.GetInstance().EndRegister = new EndRegisterViewModel();
			//await Application.Current.MainPage.Navigation.PushAsync(new EndRegisterPage(newAddress, newUSer));

			MainViewModel.GetInstance().EndRegister = new EndRegisterViewModel(newAddress, newUSer);
			await Application.Current.MainPage.Navigation.PushAsync(new EndRegisterPage());
			this.IsRunning = false;
		}
		#endregion
	}
}
