using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMaps.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Views;
    using System.Windows.Input;
    using Xamarin.Forms;
    using System.Collections.ObjectModel;
    using Models;
    using System.Linq;
    using System.Collections;

    public class ActivityViewModel :BaseViewModel
    {
        #region Atributos

        private string activitytxt;
        private bool isRefreshing;
        private Activity selectedActivity;
        private IEnumerable activityResult;
		private IEnumerable<Activity_Category> activityCatResult;
		private ObservableCollection<Activity> activities;
        private ObservableCollection<Activity_Location> locations;
        private ObservableCollection<Activity_Category> categories;
		private string categoryName;


        

        private List<User> userQuerry;
        #endregion

        #region Propiedades


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

        public ObservableCollection<Activity> Activities
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

        public Activity SelectedActivity
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
        public IEnumerable ActivityResult
        {


            get { return this.activityResult; }
            set { SetValue(ref this.activityResult, value); }


        }
		public IEnumerable<Activity_Category> ActivityCatResult
		{


			get { return this.activityCatResult; }
			set { SetValue(ref this.activityCatResult, value); }


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
		public ICommand CreateCommand
		{
			get
			{
				return new RelayCommand(CreateActivity);
			}
		}
		#endregion

		#region Contrusctores
		public ActivityViewModel()
        {

             this.Activitytxt = "";
             this.IsRefreshing =false;
             LoadActivity();

        }

		public ActivityViewModel(List<User> userQuerry)
		{
			this.Activitytxt = "";
			this.IsRefreshing = false;
			LoadActivity();
			this.userQuerry = userQuerry;
		}
		#endregion

		#region Metodos
		private async void CreateActivity()
		{
			MainViewModel.GetInstance().CreateActivity = new CreateActivityViewModel();
			await Application.Current.MainPage.Navigation.PushAsync(new CreateActivityPage());
			LoadActivity();
		}
		private async void LoadActivity()
        {
            this.IsRefreshing = true;

            Activities = ActivityData.Activities;
            Locations = Activity_LocationData.Locations;
            Categories = Activity_CategoryData.Categories;

            //this.ActivityResult;
            //var query
            var query = from act
                                   in Activities
                                  join cat in Categories on act.Activity_Cat_Code equals cat.Id
                                  join loc in Locations on act.Activity_Loc_Id equals loc.Id
                                  where (act.Name.ToUpper().StartsWith(this.Activitytxt.ToUpper()))
                                  ||
                                  (cat.Name.ToUpper().StartsWith(this.Activitytxt.ToUpper()))
                                  //||
                                  //(loc.City.ToUpper().StartsWith(this.Activitytxt.ToUpper()))
                                  select new

                                  {
                                      act,             
                                      act.Name,
                                      CategoryName = cat.Name
                                    };

            this.ActivityResult = query.ToList();
            //this.Activities = new ObservableCollection<ActivityViewModel>(ActivityResult);

            //var query = ActivityResult.ToArray();

            //for (int i = 0; i < query.Length; i++)
            //{
            //	this.ActivityCatResult = from cat in Categories
            //							 where (cat.Id.Equals(query[i].Activity_Cat_Code))
            //							 select cat;
            //	var catName = ActivityCatResult.ToArray();
            //	Category_Name = catName[i].Name

            //}





            this.IsRefreshing = false;
        }
        public async void Assign()
        {

            //var actID = this.SelectedActivity.Id;
            string actName = this.SelectedActivity.Name;
            SetValue(ref this.selectedActivity, null);
            LoadActivity();

           await Application.Current.MainPage.DisplayAlert("Alert", actName, "OK");

            // await Application.Current.MainPage.Navigation.PushAsync(new ActivityDetail(this.SelectedActivity.Id));


            // note name is property in my model (say : GeneralDataModel )
        }
        #endregion

    }


}
