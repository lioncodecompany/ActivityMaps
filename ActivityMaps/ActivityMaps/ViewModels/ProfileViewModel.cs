using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ActivityMaps.AzureStorage;
using ActivityMaps.Models;
using ActivityMaps.ViewModels;
using Xamarin.Forms;

namespace ActivityMaps.ViewModels
{
	public class ProfileViewModel : BaseViewModel
	{
		#region Atributos
		private List<User> user;
		private string nickName;
		private string name;
		private string gender;
		private string email;
		private string country;
		private string count;
		private bool isRunning;
		private ImageSource image = null;
		#endregion

		#region Propieades

		public bool IsRunning
		{
			get { return this.isRunning; }
			set
			{
				SetValue(ref this.isRunning, value);
			}
		}

		public string Country
		{
			get { return this.country; }
			set
			{
				SetValue(ref this.country, value);
			}
		}

		public string Count
		{
			get { return this.count; }
			set
			{
				SetValue(ref this.count, value);
			}
		}

		[Xamarin.Forms.TypeConverter(typeof(Xamarin.Forms.ImageSourceConverter))]
		public Xamarin.Forms.ImageSource Image
		{
			get { return this.image; }
			set { SetValue(ref this.image, value); }
		}
		public string Email
		{
			get { return this.email; }
			set
			{
				SetValue(ref this.email, value);
			}
		}
		public string Gender
		{
			get { return this.gender; }
			set
			{
				SetValue(ref this.gender, value);
			}
		}
		public string Name
		{
			get { return this.name; }
			set
			{
				SetValue(ref this.name, value);
			}
		}
		public string NickName
		{
			get { return this.nickName; }
			set
			{
				SetValue(ref this.nickName, value);
			}
		}
		#endregion
		#region Contructores
		public ProfileViewModel(List<User> user)
		{
			this.user = user;
			setValues();
			
		}
		public ProfileViewModel()
		{

		}
		#endregion

		private async void setValues()
		{
			this.IsRunning = true;
			this.NickName = user[0].Nickname;
			this.Email =  user[0].Email;
			this.Name = user[0].Name + " " + user[0].Last_Name;
			if (user[0].Gender.Equals("M"))
				this.Gender = "Male";
			else
				this.Gender = "Female";


			try
			{
				var querry = await App.MobileService.GetTable<File_Path>().Where(p => p.User_Id_FK == user[0].Id).ToListAsync();

				byte[] imageData = null;

				Image = null;
				imageData = await AzureStorage.AzureStorage.GetFileAsync(ContainerType.Image, querry[0].Path);
				this.Image = ImageSource.FromStream(() => new MemoryStream(imageData));
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
			}

			try
			{
				var querry = await App.MobileService.GetTable<Address>().Where(p => p.Id == user[0].Address_Id_FK).ToListAsync();
				this.Country = querry[0].Country;
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
			}
			try
			{
				var querry = await App.MobileService.GetTable<Entered_History>().Where(p => (p.UserCreator == user[0].Id || p.UserJoin == user[0].Id) && p.Status == "Out").ToListAsync();
				if(querry.Count == 0)
				{
					this.Count = 0.ToString() ;
				}
				else
				{
					this.Count = (querry.Count).ToString();
				}
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
			}
			this.IsRunning = false;
		}
	}
}
