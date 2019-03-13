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
    public class ActivityViewModel :BaseViewModel
    {
        #region Atributos

        private string activitytxt;
        private bool isRefreshing;
        private Activity selectedActivity;
        private IEnumerable<Activity> activityResult;
        private ObservableCollection<Activity> activities;
        #endregion

        #region Propiedades
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
        public IEnumerable<Activity> ActivityResult
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
        #endregion

        #region Contrusctores
        public ActivityViewModel()
        {

             this.Activitytxt = "";
             this.IsRefreshing =false;
             LoadActivity();

        }
        #endregion

        #region Metodos
        public void LoadActivity()
        {
            this.IsRefreshing = true;

            //ObservableCollection<DocumentList> DocumentLists = DocumentListData.DocumentLists;
            activities = new ObservableCollection<Activity>();
            {

                activities.Add(new Activity
                {
                    Name = "BasketBall 3pa3",
                    Location = "Las Piedras"
                });

                activities.Add(new Activity
                {
                    Name = "BasketBall Doble",
                    Location = "Caguas"
                });
                activities.Add(new Activity
                {
                    Name = "Ping Pong de 4 Sets",
                    Location = "Humacao"
                });
                activities.Add(new Activity
                {
                    Name = "BasketBall 3pa3",
                    Location = "Las Piedras"
                });

                activities.Add(new Activity
                {
                    Name = "BasketBall Doble",
                    Location = "Caguas"
                });
                activities.Add(new Activity
                {
                    Name = "Ping Pong de 4 Sets",
                    Location = "Humacao"
                });
                activities.Add(new Activity
                {
                    Name = "BasketBall 3pa3",
                    Location = "Las Piedras"
                });

                activities.Add(new Activity
                {
                    Name = "BasketBall Doble",
                    Location = "Caguas"
                });
                activities.Add(new Activity
                {
                    Name = "Ping Pong de 4 Sets",
                    Location = "Humacao"
                });
            };

            this.ActivityResult = from act
                                in Activities
                                where (act.Name.ToUpper().Contains(this.Activitytxt.ToUpper()))
                                ||
                                (act.Location.ToUpper().Contains(this.Activitytxt.ToUpper()))
                                select act;

            this.IsRefreshing = false;
        }
        public async void Assign()
        {

            var actID = this.SelectedActivity.Id;
            SetValue(ref this.selectedActivity, null);
            LoadActivity();

           await Application.Current.MainPage.DisplayAlert("Alert", this.SelectedActivity.Id, "OK");

            // await Application.Current.MainPage.Navigation.PushAsync(new ActivityDetail(this.SelectedActivity.Id));


            // note name is property in my model (say : GeneralDataModel )
        }
        #endregion

    }


}
