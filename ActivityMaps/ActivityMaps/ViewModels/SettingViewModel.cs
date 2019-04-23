using ActivityMaps.Helpers;
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
		private string catPlaceHolder;
		private string currentPassword;
		private string newPassword;
		private string reEnterPassword;
		private bool isSave;
		#endregion

		#region Propiedades
		public string ReEnterPassword
		{
			get { return this.reEnterPassword; }
			set { SetValue(ref this.reEnterPassword, value); }
		}
		public string NewPassword
		{
			get { return this.newPassword; }
			set { SetValue(ref this.newPassword, value); }
		}
		public string CurrentPassword
		{
			get { return this.currentPassword; }
			set { SetValue(ref this.currentPassword, value); }
		}
		public string CatPlaceHolder
		{
			get { return this.catPlaceHolder; }
			set { SetValue(ref this.catPlaceHolder, value); }
		}
		public bool IsNotification
		{
			get { return this.isNotification; }
			set { SetValue(ref this.isNotification, value); }
		}
		public bool IsSave
		{
			get { return this.isSave; }
			set { SetValue(ref this.isSave, value); }
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
			load();
			this.IsSave = false;
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
				var query = await App.MobileService.GetTable<User_Setting>().Where(p => p.User_Id_FK2 == user[0].Id).ToListAsync();
				var fav = await App.MobileService.GetTable<Activity_Preference>().Where(p => p.User_Id_FK == user[0].Id).ToListAsync();
				string catCode = "";
				if (query.Count > 0 && fav.Count > 0)
				{
					this.IsNotification = query[0].SendNotification;
					catCode= fav[0].Activity_Cat_code;

					var cat = await App.MobileService.GetTable<Activity_Category>().Where(p => p.Id == catCode).ToListAsync();
					if (cat.Count > 0) { 
					this.CatPlaceHolder = cat[0].Name;
						}
				}
				else
				{
					this.CatPlaceHolder = "Select Activity...";
					return;
				}
					
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
			}
		}
		private async void save()
		{
			int len = RandomId.length.Next(5, 10);
			if (!this.isVisible)
			{
				try
				{
					isNotChangePassword(len);

				}
				catch (Exception ex)
				{
					await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
				}

			}
			else
			{
				try
				{
					isNotChangePassword(len);
					changePass();
				}
				catch (Exception ex)
				{
					await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
				}
			}

			this.IsSave = true;
		}

		private async void changePass()
		{
			var res = "";
			var passwordQuerry = await App.MobileService.GetTable<User_Password>().Where(p => p.User_Id_FK == user[0].Id).ToListAsync();
			if (passwordQuerry.Count > 0)
			{

				byte[] decryted = System.Convert.FromBase64String(passwordQuerry[0].Password);

				res = System.Text.Encoding.Unicode.GetString(decryted);

			}

			if (string.IsNullOrEmpty(this.CurrentPassword))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter an Password.",
					"Accept");
				return;
			}

			if (!res.Equals(this.CurrentPassword))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"Its not your current password.",
					"Try Again");
				return;
			}

			if (string.IsNullOrEmpty(this.NewPassword))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"You must enter an Password.",
					"Accept");
				return;
			}
			if (this.NewPassword.Length < 7)
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"Your Password length must be greather than 6.",
					"Accept");
				return;
			}
			if (!(NewPassword.Equals(ReEnterPassword)))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"Passwords not match.",
					"Accept");
				return;
			}

			byte[] encryted = System.Text.Encoding.Unicode.GetBytes(NewPassword);
			var result = System.Convert.ToBase64String(encryted);
	

			User_Password password = new User_Password
			{
				Id = passwordQuerry[0].Id,
				Password = result,
				User_Id_FK = user[0].Id
			};

			await App.MobileService.GetTable<User_Password>().UpdateAsync(password);

		}

		private async void isNotChangePassword (int len)
		{
			var query = await App.MobileService.GetTable<User_Setting>().Where(p => p.User_Id_FK2 == user[0].Id).ToListAsync();
			if (query.Count > 0)
			{

				User_Setting setting = new User_Setting
				{
					Id = query[0].Id,
					AlertSound = query[0].AlertSound,
					SendNotification = this.IsNotification,
					Lang_Id_FK1 = query[0].Lang_Id_FK1,
					User_Id_FK2 = query[0].User_Id_FK2

				};

				await App.MobileService.GetTable<User_Setting>().UpdateAsync(setting);
			}
			else
			{

				User_Setting setting = new User_Setting
				{
					Id = RandomId.RandomString(len),
					AlertSound = true,
					SendNotification = this.IsNotification,
					Lang_Id_FK1 = "1",
					User_Id_FK2 = user[0].Id

				};

				await App.MobileService.GetTable<User_Setting>().InsertAsync(setting);
			}

			var fav = await App.MobileService.GetTable<Activity_Preference>().Where(p => p.User_Id_FK == user[0].Id).ToListAsync();
			if (query.Count > 0)
			{
				Activity_Preference act = new Activity_Preference();
				if (this.SelectedCategory == null)
				{
					act = new Activity_Preference
					{
						Id = fav[0].Id,
						Activity_Cat_code = fav[0].Activity_Cat_code,
						User_Id_FK = fav[0].User_Id_FK

					};
				}
				else
				{
					act = new Activity_Preference
					{
						Id = fav[0].Id,
						Activity_Cat_code = this.SelectedCategory.Id,
						User_Id_FK = fav[0].User_Id_FK

					};
				}

				await App.MobileService.GetTable<Activity_Preference>().UpdateAsync(act);
			}
			else
			{
				Activity_Preference act = new Activity_Preference
				{
					Id = RandomId.RandomString(len),
					Activity_Cat_code = this.SelectedCategory.Id,
					User_Id_FK = user[0].Id

				};

				await App.MobileService.GetTable<Activity_Preference>().InsertAsync(act);
			}
		}

		private void enablePass()
		{
			this.IsVisible = true;
			this.IsSave = false;
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
