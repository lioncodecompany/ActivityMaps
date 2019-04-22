using ActivityMaps.Models;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ActivityMaps.ViewModels
{
    public class SettingViewModel : BaseViewModel
    {
		#region Atributos
		private List<User> user;
		private bool isVisible;
		Activity_Category selectedCategory;
		private ObservableCollection<Activity_Category> categories;
		private bool isNotification;
		#endregion

		#region Propiedades
		public bool IsNotification
		{
			get { return this.isNotification; }
			set { SetValue(ref this.isNotification, value); }
		}
		public bool IsVisible
		{
			get { return this.isVisible; }
			set { SetValue(ref this.isVisible, value); }
		}

		public ObservableCollection<Activity_Category> Categories
		{


			get { return this.categories; }
			set { SetValue(ref this.categories, value); }


		}
		public Activity_Category SelectedCategory
		{
			get { return this.selectedCategory; }
			set { SetValue(ref this.selectedCategory, value); }
		}
		#endregion
		#region Contructores
		public SettingViewModel()
		{
		}

		public SettingViewModel(List<User> user)
		{
			this.user = user;
			this.IsVisible = false;
			getCategories();
		}
		#endregion

		#region Command
		public ICommand ChangePasswordCommand
		{
			get
			{
				return new RelayCommand(enablePass);
			}
		}
		public ICommand SaveCommand
		{
			get
			{
				return new RelayCommand(save);
			}
		}
		private async void load()
		{
			try
			{
				var query = await App.MobileService.GetTable<User_Setting>().ToListAsync();
				if (query.Count > 0)
				{
					this.isNotification = query[0].SendNotification;
					//aqui me quede
				}
					
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
			}
		}
		private async void save()
		{
			if (!this.isVisible)
			{
				
			}
		}

		private void enablePass()
		{
			this.IsVisible = true;
		}
		private async void getCategories()
		{
			try
			{
				var querry = await App.MobileService.GetTable<Activity_Category>().ToListAsync();
				Categories = new ObservableCollection<Activity_Category>();
				var arr = querry.ToArray();
				for (int idx = 0; idx < arr.Length; idx++)
				{
					Categories.Add(new Activity_Category
					{
						Id = arr[idx].Id,
						Parent = arr[idx].Parent,
						Name = arr[idx].Name
					});
				}
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
			}
		}
		#endregion
	}
}
