using System;
using System.Collections.Generic;
using System.Text;
using ActivityMaps.Models;

namespace ActivityMaps.ViewModels
{
	public class ActivityJoinViewModel : BaseViewModel
	{
		
		
		private Activity selectedActivity;
		private string name;
		private string categoryName;
		private string description;

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
		public ActivityJoinViewModel(Activity_Child selectedActivity)
		{
			this.selectedActivity = selectedActivity;
			this.Name = selectedActivity.Name;
			this.CategoryName = selectedActivity.CategoryName;
			this.Description = selectedActivity.Description;
		}
		public ActivityJoinViewModel()
		{

		}
	}
}
