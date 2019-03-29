using System;
using System.Collections.Generic;
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
			this.Name = selectedActivity.Name;
			this.CategoryName = "Category: "+selectedActivity.CategoryName;
			this.Description = selectedActivity.Description;
			//this.UserName = "me " + userQuery[0].Nickname;
		}
		public ActivityJoinViewModel()
		{

		}

		private async void setUserCreated()
		{
			//todo
		}
	}
}
