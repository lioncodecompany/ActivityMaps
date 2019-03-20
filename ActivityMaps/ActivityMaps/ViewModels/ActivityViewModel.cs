﻿using System;
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
        private Activity_Child selectedActivity;
        private List<Activity_Child> activityResult;
        //private IEnumerable<Activity_Category> activityCatResult;
		private ObservableCollection<Activity> activities;
        private ObservableCollection<Activity_Location> locations;
        private ObservableCollection<Activity_Category> categories;
		private string categoryName;
        private bool isFilterEmpty = true;

        Filter selectedFilter;

        private List<User> userQuery;
		#endregion

		#region Propiedades

		public IList<Filter> Filters { get { return FilterData.Filters; }   }

		public Filter SelectedFilter
		{
			get { return this.selectedFilter; }
			set {
                
                SetValue(ref this.selectedFilter, value);
                //Console.WriteLine("TEXT: {0}", this.SelectedFilter.Name);
                if(this.SelectedFilter.Name == "NO FILTER")
                {
                    this.IsFilterEmpty = true;
                }else
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

        public bool IsFilterEmpty
        {
            get { return this.isFilterEmpty; }
            set { SetValue(ref this.isFilterEmpty, value); }
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
             this.IsFilterEmpty = true;
             LoadActivity();

        }

		public ActivityViewModel(List<User> userQuerry)
		{
			this.Activitytxt = "";
			this.IsRefreshing = false;
			LoadActivity();
			this.userQuery = userQuerry;
		}
		#endregion

		#region Metodos
		private async void CreateActivity()
		{
			MainViewModel.GetInstance().CreateActivity = new CreateActivityViewModel();
			await Application.Current.MainPage.Navigation.PushAsync(new CreateActivityPage());
			LoadActivity();
		}
		public void LoadActivity()
        {
            this.IsRefreshing = true;

            Activities = ActivityData.Activities;
            Locations = Activity_LocationData.Locations;
            Categories = Activity_CategoryData.Categories;

            //this.ActivityResult;
            //var query
            if (this.IsFilterEmpty) {
                var query = from act
                                      in Activities
                            join cat in Categories on act.Activity_Cat_Code equals cat.Id
                            join loc in Locations on act.Activity_Loc_Id equals loc.Id
                            where (act.Name.ToUpper().StartsWith(this.Activitytxt.ToUpper()))
                            ||
                            (cat.Name.ToUpper().StartsWith(this.Activitytxt.ToUpper()))
                            //||
                            //(loc.City.ToUpper().StartsWith(this.Activitytxt.ToUpper()))
                            select new Activity_Child

                            {
                                Name = act.Name,
                                CategoryName = cat.Name,
                                Description = act.Description
                            };
                this.ActivityResult = query.ToList();
            }else
            {

                var query = from act
                                      in Activities
                            join cat in Categories on act.Activity_Cat_Code equals cat.Id
                            join loc in Locations on act.Activity_Loc_Id equals loc.Id
                            where (act.Name.ToUpper().Contains(this.Activitytxt.ToUpper()))
                            &&
                            (cat.Name.ToUpper().StartsWith(this.SelectedFilter.Name.ToUpper()))
                            //||
                            //(loc.City.ToUpper().StartsWith(this.Activitytxt.ToUpper()))
                            select new Activity_Child

                            {
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

            //var actID = this.SelectedActivity.Id;
            string actName = this.SelectedActivity.Name;
			
            //SetValue(ref this.selectedActivity, null);
            LoadActivity();

           

			MainViewModel.GetInstance().ActivityJoin = new ActivityJoinViewModel(this.SelectedActivity, this.userQuery);
			await Application.Current.MainPage.Navigation.PushAsync(new ActivityJoinPage());


            // note name is property in my model (say : GeneralDataModel )
        }
        #endregion

    }


}
