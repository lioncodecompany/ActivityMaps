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
	public class FeedbackViewModel : BaseViewModel
	{
		#region Atributos
		private List<User> user;
		Rating selectedFeedbcakRating;
		private string feedbackDescription;
		private bool isFeedbackVisible;
		private bool isFeedbackRunning;
		private bool isFeedbackEnable;

		#region propiedades
		public IList<Rating> FeedbackRating { get { return RatingData.Ratings; } }

		public bool IsFeedbackEnable
		{
			get { return this.isFeedbackEnable; }
			set { SetValue(ref this.isFeedbackEnable, value); }
		}

		public bool IsFeedbackVisible
		{
			get { return this.isFeedbackVisible; }
			set { SetValue(ref this.isFeedbackVisible, value); }
		}
		public bool IsFeedbackRunning
		{
			get { return this.isFeedbackRunning; }
			set { SetValue(ref this.isFeedbackRunning, value); }
		}

		public Rating SelectedFeedbcakRating
		{
			get { return this.selectedFeedbcakRating; }
			set { SetValue(ref this.selectedFeedbcakRating, value); }
		}

		public string FeedbackDescription
		{
			get { return this.feedbackDescription; }
			set { SetValue(ref this.feedbackDescription, value); }
		}


		#endregion

		#endregion

		#region Contructores
		public FeedbackViewModel(List<User> user)
		{
			this.user = user;
			this.IsFeedbackVisible = false;
			this.IsFeedbackEnable = true;
		}
		
		public FeedbackViewModel()
		{

		}
		#endregion

		#region Comandos

		public ICommand FeedbackNextCommand
		{
			get
			{
				return new RelayCommand(Next);
			}
		}



		private async void Next()
		{
			this.IsFeedbackRunning = true;
			this.IsFeedbackEnable = false;
			CheckConnectionInternet.checkConnectivity();

			if (string.IsNullOrEmpty(this.FeedbackDescription))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter a Description.",
					"Accept");
				return;
			}


			if (SelectedFeedbcakRating == null)
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must select a Rating.",
					"Accept");
				return;
			}


			int len = RandomId.length.Next(5, 10);

			Feedback feedback = new Feedback()
			{

				Id = RandomId.RandomString(len),
				Comment = this.FeedbackDescription.TrimEnd(),
				Rating = this.SelectedFeedbcakRating.FeedbackName,
				User_Id_FK = user[0].Id
			};
			
			try
			{
				
				await App.MobileService.GetTable<Feedback>().InsertAsync(feedback);
				await Application.Current.MainPage.DisplayAlert("Exito", "El Feedback fue insertado", "Ok");
				this.IsFeedbackVisible = true;
				
			
				//MainViewModel.GetInstance().Feedback = new FeedbackViewModel(user);
				//await Application.Current.MainPage.Navigation.PushAsync(new FeedbackPage());
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error ", ex.Message, "Ok");
			}

			this.IsFeedbackRunning = false;
		}
		#endregion

	}
}
