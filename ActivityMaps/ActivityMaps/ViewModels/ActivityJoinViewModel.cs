using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using ActivityMaps.AzureStorage;
using ActivityMaps.Helpers;
using ActivityMaps.Models;
using Xamarin.Forms;

namespace ActivityMaps.ViewModels
{
	public class ActivityJoinViewModel : BaseViewModel
	{

		private Timer TickTimer = null;
		private Activity selectedActivity;
		private string userName;
		private string name;
		private string categoryName;
		private string description;
		private ImageSource image;
		private bool isRunning;

		[Xamarin.Forms.TypeConverter(typeof(Xamarin.Forms.ImageSourceConverter))]
		public Xamarin.Forms.ImageSource Image
		{
			get { return this.image; }
			set { SetValue(ref this.image, value); }
		}

		public bool IsRunning
		{
			get { return this.isRunning; }
			set
			{

				SetValue(ref this.isRunning, value);

			}
		}

		public string UserName
		{
			get { return this.userName; }
			set
			{

				SetValue(ref this.userName, value);

			}
		}

		public string Description
		{
			get { return this.description; }
			set
			{

				SetValue(ref this.description, value);

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

		public string CategoryName
		{
			get { return this.categoryName; }
			set
			{

				SetValue(ref this.categoryName, value);

			}
		}
		public ActivityJoinViewModel(Activity_Child selectedActivity, List<User> userQuery)
		{
			TickTimer = new Timer(5000);
			TickTimer.Elapsed += new ElapsedEventHandler(_Elapsed);
			this.selectedActivity = selectedActivity;
			setUserCreated();
			this.name = selectedActivity.Name;
			this.categoryName = "Category: "+selectedActivity.CategoryName;
			this.description = selectedActivity.Description;
			
			//this.UserName = "me " + userQuery[0].Nickname;
		}

		private void _Elapsed(object sender, ElapsedEventArgs e)
		{
			this.Name = this.name;
			this.CategoryName = this.categoryName;
			this.Description = this.description;
			TickTimer.Stop();
		}

		public ActivityJoinViewModel()
		{

		}

		private async void setUserCreated()
		{
			this.IsRunning = true;
			var user = await App.MobileService.GetTable<User_Entered>().Where(p => p.Activity_Code_FK2 == selectedActivity.Id && p.IsCreator).ToListAsync();
			var userCreator = await App.MobileService.GetTable<User_Log>().Where(p => p.Id == user[0].User_Log_Id_FK1).ToListAsync();
			var userName = await App.MobileService.GetTable<User>().Where(p => p.Id == userCreator[0].User_Id_FK2).ToListAsync();
			var filePath = await App.MobileService.GetTable<File_Path>().Where(p => p.User_Id_FK == userCreator[0].User_Id_FK2).ToListAsync();

			if (filePath.Count > 0)
			{
				var imageData = await AzureStorage.AzureStorage.GetFileAsync(ContainerType.Image, filePath[0].Path);
				Image = ImageSource.FromStream(() => new MemoryStream(imageData));
			}
			UserName = "Creator: " + userName[0].Nickname;
	
			this.IsRunning = false;

		}

	}
}
