using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using ActivityMaps.Helpers;
using ActivityMaps.Models;
using ActivityMaps.Views;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityMaps.ViewModels
{
    public class CreateActivityViewModel : BaseViewModel
    {
        #region Atributos
        private string colorButton;
        private string buttonText;
        Activity_Category selectedCategory;
        private string activityName;
        private string description;
        private TimeSpan startHour;
        private TimeSpan finishHour;
        private DateTime startDay;
        private DateTime finishDay;
        private bool isVisible = false;
        private List<User> userQuery;
        private User_Log userLog;
        public static User_Log userCreating;
        private string usLog = "3"; //Create activity
        private ObservableCollection<Activity_Category> categories;
        private Location loc;
        private Location origLoc;
        private string placename;
        Geocoder geoCoder;
        private int pickerCatIndex;


        public delegate void PickerCatEvent(int PickerIndex);
        public event PickerCatEvent PickerEvent;
        #endregion
        #region Propieades

        public int PickerCatIndex
        {
            get { return this.pickerCatIndex; }
            set { SetValue(ref this.pickerCatIndex, value); }
        }
        public string ButtonText
        {
            get { return this.buttonText; }
            set { SetValue(ref this.buttonText, value); }
        }

        public string Placename
        {
            get { return this.placename; }
            set { SetValue(ref this.placename, value); }
        }

        public string ButtonColor
        {
            get { return this.colorButton; }
            set { SetValue(ref this.colorButton, value); }
        }

        public bool IsVisible
        {
            get { return this.isVisible; }
            set { SetValue(ref this.isVisible, value); }
        }

        public string ActivityName
        {
            get { return this.activityName; }
            set { SetValue(ref this.activityName, value); }
        }

        public string Description
        {
            get { return this.description; }
            set { SetValue(ref this.description, value); }
        }

        public TimeSpan StartHour
        {
            get { return this.startHour; }
            set { SetValue(ref this.startHour, value); }
        }

        public TimeSpan FinishHour
        {
            get { return this.finishHour; }
            set { SetValue(ref this.finishHour, value); }
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

        public DateTime StartDay
        {
            get { return this.startDay; }
            set { SetValue(ref this.startDay, value); }
        }

        public DateTime FinishDay
        {
            get { return this.finishDay; }
            set { SetValue(ref this.finishDay, value); }
        }

        #endregion


        #region Contrusctores
        public CreateActivityViewModel()
        {
            instance = this;
            this.ButtonText = "Select Location";
            this.ButtonColor = "Red";
        }

        public CreateActivityViewModel(List<User> userQuery, User_Log userLog, ObservableCollection<Activity_Category> categories)
        {
            instance = this;
            this.ButtonText = "Select Location";
            this.ButtonColor = "Red";

            this.userQuery = userQuery;
            this.userLog = userLog;
            this.Categories = categories;


        }
        public CreateActivityViewModel(List<User> userQuery, User_Log userLog, ObservableCollection<Activity_Category> categories, 
            Activity act, Location loc, Location origLoc,int pickerCatIndex)
        {
            instance = this;
            this.ButtonText = "Select Location";
            this.ButtonColor = "Red";


            this.userQuery = userQuery;
            this.userLog = userLog;
            this.Categories = categories;
            this.ActivityName = act.Name;
            this.StartDay = act.Start_Act_Datetime;
            this.FinishDay = act.End_Act_Datetime;
            this.Description = act.Description;
            //Console.WriteLine("***TEST: "+pickerCatIndex.ToString());
            this.PickerCatIndex = pickerCatIndex;
            PickerIndex();


            this.loc = loc; //Location
            this.origLoc = origLoc;
            getPlacename();

            if (this.loc != null)
            {
                this.IsVisible = true;
                this.ButtonColor = "Green";
                this.ButtonText = "Modify Location";
            }



        }
        #endregion
        #region SignLeton
        private static CreateActivityViewModel instance;

        public static CreateActivityViewModel GetInstance()
        {
            if (instance == null)
            {
                return new CreateActivityViewModel();
            }
            return instance;
        }
        #endregion


        public ICommand Create
        {
            get
            {
                return new RelayCommand(CreateActivity);
            }
        }
        public ICommand LCommand
        {
            get
            {
                return new RelayCommand(Location);
            }
        }



        private async void Location()
        {
            CheckConnectionInternet.checkConnectivity();
            if (string.IsNullOrEmpty(this.ActivityName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter an Activity Name.",
                    "Accept");
                return;
            }
            if (string.IsNullOrEmpty(this.Description))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a description.",
                    "Accept");
                return;
            }
            if (SelectedCategory == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must select a Category.",
                    "Accept");
                return;
            }



            Activity act = new Activity()
            {

                Name = this.ActivityName,
                Start_Act_Datetime = this.StartDay.Date,
                End_Act_Datetime = this.FinishDay.Date,
                Description = this.Description,
                Activity_Cat_Code = SelectedCategory.Id


            };

            MainViewModel.GetInstance().Location = new LocationViewModel(userQuery, userLog, categories, act, this.PickerCatIndex);
            await Application.Current.MainPage.Navigation.PushAsync(new LocationPage());




        }

        private async void CreateActivity()
        {
            CheckConnectionInternet.checkConnectivity();


            if (string.IsNullOrEmpty(this.ActivityName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter an Activity Name.",
                    "Accept");
                return;
            }
            if (string.IsNullOrEmpty(this.Description))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a description.",
                    "Accept");
                return;
            }
            if (SelectedCategory == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must select a Category.",
                    "Accept");
                return;
            }


            if (loc == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must select a Location.",
                    "Accept");
                return;
            }

            int len = RandomId.length.Next(5, 10);
            Activity activity = new Activity()
            {
                Id = RandomId.RandomString(len),
                Name = this.ActivityName,
                Created_Date = DateTime.Now,
                IsPrivate = false,//todo
                Start_Act_Datetime = this.StartDay.Date,
                End_Act_Datetime = this.FinishDay.Date,
                Description = this.Description,
                Status = 1,//check
                IsService = false,//todo
                Activity_Cat_Code = SelectedCategory.Id,
                Activity_Loc_Id = "1" //todo
            };

            //ctivity history
            Activity_History activityHistory = new Activity_History()
            {
                Id = RandomId.RandomString(len),
                Activity_Code_Id = activity.Id,
                Name = this.ActivityName,
                Created_Date = DateTime.Now,
                IsPrivate = false,//todo
                Start_Act_Date = this.StartDay.Date,
                End_Act_Date = this.FinishDay.Date,
                Description = this.Description,
                Status = 1,//check
                IsService = false,//todo
                Activity_Cat_code = SelectedCategory.Id,
                Activity_Loc_Id_FK = "1" //todo
            };
            userCreating = User_LogType.userLogTypesAsync(userQuery[0].Id, usLog);
            User_Entered entry = new User_Entered()
            {
                Id = RandomId.RandomString(len),
                Status = "in",
                IsCreator = true,
                User_Log_Id_FK1 = userCreating.Id,
                Activity_Code_FK2 = activityHistory.Activity_Code_Id
            };
            Entered_History entryHistory = new Entered_History()
            {
                Id = entry.Id,
                Status = "in",
                IsCreator = true,
                Activity_Code_FK2 = activityHistory.Activity_Code_Id,
                UserJoin = userQuery[0].Id,
                UserCreator = userQuery[0].Id
            };
            Activity_Location activity_location = new Activity_Location()
            {
                Id = RandomId.RandomString(len),
                Nameplace = this.Placename,
                City = "",
                State = "",
                Country = "",
                Latitude = (decimal)this.loc.Latitude,
                Longitude = (decimal)this.loc.Longitude,
                CreatorOriginalPinLatitude = (decimal)this.origLoc.Latitude,
                CreatorOriginalPinLongitude = (decimal)this.origLoc.Longitude


            };

            try
            {
                await App.MobileService.GetTable<Activity>().InsertAsync(activity);
                await App.MobileService.GetTable<Activity_History>().InsertAsync(activityHistory);
                await App.MobileService.GetTable<User_Log>().InsertAsync(userCreating);
                await App.MobileService.GetTable<User_Entered>().InsertAsync(entry);
                await App.MobileService.GetTable<Entered_History>().InsertAsync(entryHistory);
                await App.MobileService.GetTable<Activity_Location>().InsertAsync(activity_location);


            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }

            MainViewModel.GetInstance().Activity_Child = new ActivityViewModel(userQuery, entry);
            await Application.Current.MainPage.Navigation.PushAsync(new ActivityPage());
        }

        private async void ValidateFields()
        {

        }

        private async void getPlacename()
        {
            geoCoder = new Geocoder();
            var fortMasonPosition = new Position(this.loc.Latitude, this.loc.Longitude);
            var possibleAddresses = await geoCoder.GetAddressesForPositionAsync(fortMasonPosition);
            this.Placename = possibleAddresses?.FirstOrDefault();

        }

        public async void PickerIndex()
        {

            PickerEvent?.Invoke(this.PickerCatIndex);

        }
    }
}
