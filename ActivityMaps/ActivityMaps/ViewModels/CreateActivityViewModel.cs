using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ActivityMaps.Models;
using ActivityMaps.Views;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;

namespace ActivityMaps.ViewModels
{
    public class CreateActivityViewModel : BaseViewModel
    {
		#region Atributos
		private string colorButton;
		private string buttonText;
		Activity_Category selectedCategory;
		private string activityName;
		private string description;
		private TimeSpan startHour;
		private TimeSpan finishHour;
		private DateTime startDay;
		private DateTime finishDay;
		private bool isVisible;
		#endregion
		#region Propieades

		public string ButtonText
		{
			get { return this.buttonText; }
			set { SetValue(ref this.buttonText, value); }
		}

		public string ButtonColor
		{
			get { return this.colorButton; }
			set { SetValue(ref this.colorButton, value); }
		}

		public bool IsVisible
		{
			get { return this.isVisible; }
			set { SetValue(ref this.isVisible, value); }
		} 

		public string ActivityName
		{
			get { return this.activityName; }
			set { SetValue(ref this.activityName, value); }
		}

		public string Description
		{
			get { return this.description; }
			set { SetValue(ref this.description, value); }
		}

		public TimeSpan StartHour
		{
			get { return this.startHour; }
			set { SetValue(ref this.startHour, value); }
		}

		public TimeSpan FinishHour
		{
			get { return this.finishHour; }
			set { SetValue(ref this.finishHour, value); }
		}

		public IList<Activity_Category> Categories { get { return Activity_CategoryData.Categories; } }


		public Activity_Category SelectedCategory
		{
			get { return this.selectedCategory; }
			set { SetValue(ref this.selectedCategory, value); }
		}

		public DateTime StartDay
		{
			get { return this.startDay; }
			set { SetValue(ref this.startDay, value); }
		}

		public DateTime FinishDay
		{
			get { return this.finishDay; }
			set { SetValue(ref this.finishDay, value); }
		}

		#endregion
		#region Contrusctores
		public CreateActivityViewModel()
		{
			this.ButtonText = "Select Location";
			this.ButtonColor = "Red";
		}
		#endregion


		public ICommand LCommand
		{
			get
			{
				return new RelayCommand(Location);
			}
		}

		private async void Location()
		{
			MainViewModel.GetInstance().Location = new LocationViewModel();
			await Application.Current.MainPage.Navigation.PushAsync(new LocationPage());

			this.IsVisible = true;
			this.ButtonColor = "Green";
			this.ButtonText = "Modify Location";


		}
	}
}
