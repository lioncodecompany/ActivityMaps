﻿using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using ActivityMaps.Models;
using ActivityMaps.Views;

namespace ActivityMaps.ViewModels
{
    public class RegisterViewModel : BaseViewModel
	{
		#region Atributos

		private string name;
		private string lastName;
		private string nickname;
		Gender selectedGender;
		private DatePicker birthdate;
		private string phone;
		private string address;
		private string city;
		private string state;
		Country selectedCountry;
		private string zipCode;

		#endregion

		#region Propiedades

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

		public DatePicker Birthdate
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
			this.Name = "kevin";
			this.Last_Name = "Leon";
			this.Nickname = "Kolb22";
			this.Phone = "7877333421";
			this.Address = "PO BOX";
			this.City = "Las Piedras";
			this.State = "PR";
			this.Zip_Code = "00771";
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
			
			if (string.IsNullOrEmpty(this.Name))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter an Name.",
					"Accept");
				return;
			}

			if (string.IsNullOrEmpty(this.Last_Name))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter an Last Name.",
					"Accept");
				return;
			}

			if (string.IsNullOrEmpty(this.Nickname))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter an Nickname.",
					"Accept");
				return;
			}

			if (string.IsNullOrEmpty(this.Phone))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter an Phone.",
					"Accept");
				return;
			}

			if (string.IsNullOrEmpty(this.Address))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter an Address.",
					"Accept");
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
				return;
			}
			
			if(SelectedGender == null)
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must select an Gender.",
					"Accept");
				return;
			}
			
			
			if (SelectedCountry == null)
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must select an Country.",
					"Accept");
				return;
			}


			await Application.Current.MainPage.Navigation.PushAsync(new EndRegisterPage());


		}
		#endregion
	}
}
