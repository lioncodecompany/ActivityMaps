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

		private string usertxt;
		Rating selectedUserRating;
		private string userDescription;
		private bool isUserVisible;
		private bool isUserRunning;
		private bool isUserEnable;
		private List<User> query;
		private string userNick;

		private string locationtxt;
		Rating selectedLocationRating;
		private string locationDescription;
		private bool isLocationVisible;
		private bool isLocationRunning;
		private bool isLocationEnable;
		private List<Activity_Location> queryLocation;
		private string locationNick;

		#region propiedades
		public string Usertxt
		{

			get { return this.usertxt; }
			set
			{

				SetValue(ref this.usertxt, value);

				// SearchLocation();

			}
		}

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

		//------------------------------------------------------------------User
		public IList<Rating> UserRating { get { return RatingData.Ratings; } }

		public bool IsUserEnable
		{
			get { return this.isUserEnable; }
			set { SetValue(ref this.isUserEnable, value); }
		}



		public bool IsUserVisible
		{
			get { return this.isUserVisible; }
			set { SetValue(ref this.isUserVisible, value); }
		}
		public bool IsUserRunning
		{
			get { return this.isUserRunning; }
			set { SetValue(ref this.isUserRunning, value); }
		}

		public Rating SelectedUserRating
		{
			get { return this.selectedUserRating; }
			set { SetValue(ref this.selectedUserRating, value); }
		}

		public string UserDescription
		{
			get { return this.userDescription; }
			set { SetValue(ref this.userDescription, value); }
		}

		public string UserNick
		{
			get { return this.userNick; }
			set { SetValue(ref this.userNick, value); }
		}
		//------------------------------------------------------------------Location
		public IList<Rating> LocationRating { get { return RatingData.Ratings; } }

		public bool IsLocationEnable
		{
			get { return this.isLocationEnable; }
			set { SetValue(ref this.isLocationEnable, value); }
		}



		public bool IsLocationVisible
		{
			get { return this.isLocationVisible; }
			set { SetValue(ref this.isLocationVisible, value); }
		}
		public bool IsLocationRunning
		{
			get { return this.isLocationRunning; }
			set { SetValue(ref this.isLocationRunning, value); }
		}

		public Rating SelectedLocationRating
		{
			get { return this.selectedLocationRating; }
			set { SetValue(ref this.selectedLocationRating, value); }
		}

		public string LocationDescription
		{
			get { return this.locationDescription; }
			set { SetValue(ref this.locationDescription, value); }
		}

		public string LocationNick
		{
			get { return this.locationNick; }
			set { SetValue(ref this.locationNick, value); }
		}

		public string Locationtxt
		{

			get { return this.locationtxt; }
			set
			{

				SetValue(ref this.locationtxt, value);

				// SearchLocation();

			}
		}


		#endregion

		#endregion

		#region Contructores
		public FeedbackViewModel(List<User> user)
		{
			this.user = user;
			this.IsFeedbackVisible = false;
			this.IsFeedbackEnable = true;
			this.IsUserVisible = false;
			this.IsFeedbackEnable = true;
			this.Usertxt = "";
			this.Locationtxt = "";
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

		public ICommand SearchCommand
		{
			get
			{
				return new RelayCommand(getUser);
			}

		}
		public ICommand SearchLocationCommand
		{
			get
			{
				return new RelayCommand(getLocation);
			}

		}
		public ICommand UserNextCommand
		{
			get
			{
				return new RelayCommand(sendFeedback);
			}

		}
		public ICommand LocationNextCommand
		{
			get
			{
				return new RelayCommand(sendFeedbackLocation);
			}

		}


		public async void sendFeedbackLocation()
		{
			this.IsLocationRunning = true;
			this.IsLocationEnable = false;
			if (string.IsNullOrEmpty(this.LocationNick))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter a real Location.",
					"Accept");
				this.IsLocationRunning = false;
				this.IsLocationEnable = true;
				return;
			}
			if (string.IsNullOrEmpty(this.Locationtxt))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter an Location in Search Bar.",
					"Accept");
				this.IsLocationRunning = false;
				this.IsLocationEnable = true;
				return;
			}
			if (string.IsNullOrEmpty(this.LocationDescription))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter a Description.",
					"Accept");
				this.IsLocationRunning = false;
				this.IsLocationEnable = true;
				return;
			}


			if (SelectedLocationRating == null)
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must select a Rating.",
					"Accept");
				this.IsLocationRunning = false;
				this.IsLocationEnable = true;
				return;
			}


			int len = RandomId.length.Next(5, 10);

			Location_Rating feedback = new Location_Rating()
			{

				Id = RandomId.RandomString(len),
				Comment = this.LocationDescription.TrimEnd(),
				Rating = this.SelectedLocationRating.FeedbackName,
				User_Id = user[0].Id,
				Activity_Loc_Id_FK = queryLocation[0].Id
			};

			try
			{

				await App.MobileService.GetTable<Location_Rating>().InsertAsync(feedback);
				await Application.Current.MainPage.DisplayAlert("Exito", "El Feedback fue insertado", "Ok");
				this.IsLocationVisible = true;


			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error ", ex.Message, "Ok");
			}

			this.IsLocationRunning = false;

		}

		private async void getLocation()
		{
			var txt = this.Locationtxt;

			try
			{
				CheckConnectionInternet.checkConnectivity();
				queryLocation = await App.MobileService.GetTable<Activity_Location>().Where(p => p.Nameplace == txt).ToListAsync();
				if (queryLocation.Count == 0)
				{
					await Application.Current.MainPage.DisplayAlert("Warning", "The location was not found ", "Ok");
					return;
				}
				

				this.LocationNick = queryLocation[0].Nameplace;

			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error ", ex.Message, "Ok");
			}

		}

		public async void sendFeedback()
		{
			this.IsUserRunning = true;
			this.IsUserEnable = false;
			if (string.IsNullOrEmpty(this.UserNick))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter a real User.",
					"Accept");
				this.IsUserRunning = false;
				this.IsUserEnable = true;
				return;
			}
			if (string.IsNullOrEmpty(this.Usertxt))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter an User in Search Bar.",
					"Accept");
				this.IsUserRunning = false;
				this.IsUserEnable = true;
				return;
			}
			if (string.IsNullOrEmpty(this.UserDescription))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter a Description.",
					"Accept");
				this.IsUserRunning = false;
				this.IsUserEnable = true;
				return;
			}


			if (SelectedUserRating == null)
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must select a Rating.",
					"Accept");
				this.IsUserRunning = false;
				this.IsUserEnable = true;
				return;
			}


			int len = RandomId.length.Next(5, 10);

			User_Rating feedback = new User_Rating()
			{

				Id = RandomId.RandomString(len),
				Comment = this.UserDescription.TrimEnd(),
				Rating = this.SelectedUserRating.FeedbackName,
				User_IdReporter_FK1 = user[0].Id,
				User_IdReported_FK2 = query[0].Id
			};

			try
			{

				await App.MobileService.GetTable<User_Rating>().InsertAsync(feedback);
				await Application.Current.MainPage.DisplayAlert("Exito", "El Feedback fue insertado", "Ok");
				this.IsUserVisible = true;


			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error ", ex.Message, "Ok");
			}

			this.IsUserRunning = false;

		}

		private async void getUser()
		{
			var txt = this.Usertxt;

			try
			{
				CheckConnectionInternet.checkConnectivity();
				query = await App.MobileService.GetTable<User>().Where(p => p.Nickname == txt).ToListAsync();
				if(query.Count == 0)
				{
					await Application.Current.MainPage.DisplayAlert("Warning", "The user was not found ", "Ok");
					return;
				}
				if (query[0].Nickname.Equals(user[0].Nickname))
				{
					await Application.Current.MainPage.DisplayAlert("Warning", "You can't rating yourself", "Ok");
					return;
				}

				this.UserNick = query[0].Nickname;

			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error ", ex.Message, "Ok");
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
				this.IsFeedbackRunning = false;
				this.IsFeedbackEnable = true;
				return;
			}


			if (SelectedFeedbcakRating == null)
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must select a Rating.",
					"Accept");
				this.IsFeedbackRunning = false;
				this.IsFeedbackEnable = true;
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
