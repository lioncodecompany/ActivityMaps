using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ActivityMaps.Helpers;
using ActivityMaps.Models;
using ActivityMaps.Views;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;

namespace ActivityMaps.ViewModels
{
	public class HistoryViewModel : BaseViewModel
	{
		#region Atributos
		private List<User> user;

		private string activitytxt;
		private bool isRefreshing;
		private Activity_Child selectedActivity;
		private List<Activity_Child> activityResult;
		//private IEnumerable<Activity_Category> activityCatResult;
		private ObservableCollection<Activity_History> activities;
		private ObservableCollection<Activity_Location> locations;
		private ObservableCollection<Activity_Category> categories;
		private string categoryName;
		private bool isFilterEmpty = true;
		private string logType = "1"; //login
		private User_Log userLog;
		private ImageSource image;
		private bool isRunning;
		Filter selectedFilter;
		User_Equipment equipment;
		private List<User> userQuery;
		private User_Entered entryUser;
		private ObservableCollection<Entered_History> userQueue;
		#endregion

		#region properties
		[Xamarin.Forms.TypeConverter(typeof(Xamarin.Forms.ImageSourceConverter))]
		public Xamarin.Forms.ImageSource Image
		{
			get { return this.image; }
			set { SetValue(ref this.image, value); }
		}

		public IList<Filter> Filters { get { return FilterData.Filters; } }


		public Filter SelectedFilter
		{
			get { return this.selectedFilter; }
			set
			{

				SetValue(ref this.selectedFilter, value);
				//Console.WriteLine("TEXT: {0}", this.SelectedFilter.Name);
				if (this.SelectedFilter.Name == "NO FILTER")
				{
					this.IsFilterEmpty = true;
				}
				else
				{
					this.IsFilterEmpty = false;
				}

				LoadActivity();

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


		public string Activitytxt
		{

			get { return this.activitytxt; }
			set
			{

				SetValue(ref this.activitytxt, value);

				LoadActivity();

			}
		}

		public bool IsRefreshing
		{
			get { return this.isRefreshing; }
			set { SetValue(ref this.isRefreshing, value); }
		}

		public bool IsRunning
		{

			get { return this.isRunning; }
			set { SetValue(ref this.isRunning, value); }
		}

		public bool IsFilterEmpty
		{
			get { return this.isFilterEmpty; }
			set { SetValue(ref this.isFilterEmpty, value); }
		}

		public ObservableCollection<Activity_History> Activities
		{


			get { return this.activities; }
			set { SetValue(ref this.activities, value); }


		}
		public ObservableCollection<Activity_Location> Locations
		{


			get { return this.locations; }
			set { SetValue(ref this.locations, value); }


		}
		public ObservableCollection<Activity_Category> Categories
		{


			get { return this.categories; }
			set { SetValue(ref this.categories, value); }


		}

		public Activity_Child SelectedActivity
		{


			get
			{
				return this.selectedActivity;
			}
			set
			{

				if (this.selectedActivity != value)
				{
					SetValue(ref this.selectedActivity, value);
					Assign();
				}

			}


		}
		public List<Activity_Child> ActivityResult
		{


			get { return this.activityResult; }
			set { SetValue(ref this.activityResult, value); }


		}
		#endregion
		#region Contructores
		public HistoryViewModel()
		{
		
		}

		public HistoryViewModel(List<User> user)
		{
			this.user = user;
			this.Activitytxt = "";
			this.IsRefreshing = false;
			this.IsFilterEmpty = true;
			LoadActivity();
			// fillEquipment();
		}
		#endregion
		#region Commandos


		public ICommand SearchCommand
		{
			get
			{
				return new RelayCommand(LoadActivity);
			}

		}
		public ICommand RefreshCommand
		{
			get
			{
				return new RelayCommand(LoadActivity);
			}
		}
		#endregion

	

		#region Metodos

		
		
		private async void LoadActivity()
		{

			//Activities = ActivityData.Activities;
			try
			{
				var querry = await App.MobileService.GetTable<Activity_History>().ToListAsync();
				Activities = new ObservableCollection<Activity_History>();
				var arr = querry.ToArray();
				for (int idx = 0; idx < arr.Length; idx++)
				{
				
						Activities.Add(new Activity_History
						{
							Id = arr[idx].Id,
							Activity_Code_Id = arr[idx].Activity_Code_Id,
							Description = arr[idx].Description,
							Name = arr[idx].Name,
							Activity_Loc_Id_FK = arr[idx].Activity_Loc_Id_FK,
							Activity_Cat_code = arr[idx].Activity_Cat_code
						});
					}

				
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
			}

			try
			{
				var querry = await App.MobileService.GetTable<Entered_History>().Where(use => use.UserCreator == user[0].Id || use.UserJoin == user[0].Id).ToListAsync();
				userQueue = new ObservableCollection<Entered_History>();
				var arr = querry.ToArray();
				for (int idx = 0; idx < arr.Length; idx++)
				{

					userQueue.Add(new Entered_History
					{
						Id = arr[idx].Id,
						Status = arr[idx].Status,
						IsCreator = arr[idx].IsCreator,
						User_Log_Id_FK1 = arr[idx].User_Log_Id_FK1,
						Activity_Code_FK2 = arr[idx].Activity_Code_FK2,
						UserJoin = arr[idx].UserJoin,
						UserCreator = arr[idx].UserCreator
					});
				}


			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
			}
			Locations = Activity_LocationData.Locations;

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
			this.IsRefreshing = true;
			//Categories = Activity_CategoryData.Categories;

			//this.ActivityResult;
			//var query
			if (this.IsFilterEmpty)
			{
				var query = from act
									   in Activities
							join cat in Categories on act.Activity_Cat_code equals cat.Id
							join loc in Locations on act.Activity_Loc_Id_FK equals loc.Id
							join use in userQueue on act.Activity_Code_Id equals use.Activity_Code_FK2
							where (act.Name.ToUpper().Contains(this.Activitytxt.ToUpper()))
							
							
							select new Activity_Child

							{
								Id = act.Id,
								Name = act.Name,
								CategoryName = cat.Name,
								Description = act.Description

							};
				this.ActivityResult = query.ToList();
			}
			else
			{

				var query = from act
									  in Activities
							join cat in Categories on act.Activity_Cat_code equals cat.Id
							join loc in Locations on act.Activity_Loc_Id_FK equals loc.Id
							join use in userQueue on act.Activity_Code_Id equals use.Activity_Code_FK2
							where (act.Name.ToUpper().Contains(this.Activitytxt.ToUpper()))
							&&
							(cat.Name.ToUpper().StartsWith(this.SelectedFilter.Name.ToUpper()))
							
							//||
							//(loc.City.ToUpper().StartsWith(this.Activitytxt.ToUpper()))
							select new Activity_Child

							{
								Id = act.Id,
								Name = act.Name,
								CategoryName = cat.Name,
								Description = act.Description
							};

				this.ActivityResult = query.ToList();
			}



			this.IsRefreshing = false;
		}


		public async void Assign()
		{
			this.IsRunning = true;
			var actID = this.SelectedActivity.Id;
			string actName = this.SelectedActivity.Name;

			//SetValue(ref this.selectedActivity, null);
			LoadActivity();



			// note name is property in my model (say : GeneralDataModel )
		}

	
		#endregion
	}
}
