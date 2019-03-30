using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActivityMaps.Helpers;
using ActivityMaps.Models;

namespace ActivityMaps.ViewModels
{
	public class ActivityJoinViewModel : BaseViewModel
	{
		
		
		private Activity selectedActivity;
		private string userName;
		private string name;
		private string categoryName;
		private string description;
		private bool isRunning;

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
			
			this.selectedActivity = selectedActivity;
			this.name = selectedActivity.Name;
			this.categoryName = "Category: "+selectedActivity.CategoryName;
			this.description = selectedActivity.Description;
			setUserCreated();
			//this.UserName = "me " + userQuery[0].Nickname;
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
			UserName = userName[0].Nickname;
			this.Name = this.name;
			this.Description = this.description;
			this.CategoryName = this.categoryName;
			this.IsRunning = false;

		}
	}
}
