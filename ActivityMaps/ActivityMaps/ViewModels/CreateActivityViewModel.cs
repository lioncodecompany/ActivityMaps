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

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Xamarin.Forms.Maps;
    using Xamarin.Essentials;
    using System.Linq;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Views;
    using Xamarin.Forms;
    using System.Collections.ObjectModel;
    using ActivityMaps.Models;
    using Plugin.Permissions;
    using Plugin.Permissions.Abstractions;

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
        private DateTime startDay = DateTime.Today;
        private DateTime finishDay = DateTime.Today;
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
		private bool isService;
        private DateTime todayNow;




        public delegate void PickerCatEvent(int PickerIndex);
        public event PickerCatEvent PickerEvent;
        #endregion
        #region Propieades
        public DateTime TodayNow
        {
            get { return this.todayNow; }
            set { SetValue(ref this.todayNow, value); }
        }
        public bool IsService
		{
			get { return this.isService; }
			set { SetValue(ref this.isService, value); }
		}
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
            set {
                if (this.startHour != value)
                {
                    
                    SetValue(ref this.startHour, value);
                    
                    TimeSpan time1 = TimeSpan.FromHours(1);
                    if (ChangeFinishHour())
                    {
                                        //this.startHour.Add(time1);//verificar cuando la suma de al otro dia
                        this.FinishHour = this.startHour;//Fix Error 11:00-11:59PM                     
                    }
                    
                    ValidateDatetime();
                }

            }
        }



        public TimeSpan FinishHour
        {
            get { 
                return this.finishHour; }
            set {
                if (this.finishHour != value)
                {
                    
                    SetValue(ref this.finishHour, value);
                    ValidateDatetime();
                }
                
            }

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
            set
            {
                if (this.startDay != value)
                {
                    
                    SetValue(ref this.startDay, value);
                    //DateTime date1 = DateTime.FromDays(1);
                    if (this.FinishDay < this.startDay){
                        this.FinishDay = this.startDay;
                        ValidateDatetime();
                    }
                    
                }
            }
        }

        public DateTime FinishDay
        {
            get { return this.finishDay; }
            set
            {
                if (this.finishDay != value)
                {
                    
                    SetValue(ref this.finishDay, value);
                    ValidateDatetime();
                }
            }
        }

        #endregion


        #region Contrusctores
        public CreateActivityViewModel()
        {
            instance = this;
            this.ButtonText = "Select Location";
            this.ButtonColor = "Red";
            this.TodayNow = DateTime.Now;
            //this.StartDay = DateTime.Today;
            //this.FinishDay = DateTime.Today;
            //this.StartHour = DateTime.Now.TimeOfDay;
            //this.FinishHour = DateTime.Now.TimeOfDay;
        }

        public CreateActivityViewModel(List<User> userQuery, User_Log userLog, ObservableCollection<Activity_Category> categories)
        {
            instance = this;
            this.ButtonText = "Select Location";
            this.ButtonColor = "Red";
            this.StartDay = DateTime.Today;
            this.FinishDay = DateTime.Today;
            this.StartHour = DateTime.Now.TimeOfDay;
            this.FinishHour = DateTime.Now.TimeOfDay;
            this.TodayNow = DateTime.Now;

            this.userQuery = userQuery;
            this.userLog = userLog;
            this.Categories = categories;


        }
        public CreateActivityViewModel(List<User> userQuery, User_Log userLog, ObservableCollection<Activity_Category> categories, 
            Activity act, Location loc, Location origLoc,int pickerCatIndex)
        {
            
            this.ButtonText = "Select Location";
            this.ButtonColor = "Red";


            this.userQuery = userQuery;
            this.userLog = userLog;
            this.Categories = categories;
            this.ActivityName = act.Name;
            this.StartDay = act.Start_Act_Datetime;
            this.FinishDay = act.End_Act_Datetime;
            this.StartHour = act.Start_Act_Datetime.TimeOfDay;
            this.FinishHour = act.End_Act_Datetime.TimeOfDay;
            this.Description = act.Description;
            this.IsService = act.IsService;
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
            instance = this;


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

            //Permiso para usar el Mapa y GPS
            var LocationStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);

            if (LocationStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Location });
                LocationStatus = results[Permission.Location];
            }

            if (LocationStatus != PermissionStatus.Granted)
            {
                await Application.Current.MainPage.DisplayAlert("Permissions Denied", "Unable to choose Location.", "OK");
                
            }



            Activity act = new Activity()
            {

                Name = this.ActivityName,
                Start_Act_Datetime = this.StartDay + this.StartHour,
                End_Act_Datetime = this.FinishDay + this.FinishHour,
                Description = this.Description,
                IsService = this.IsService,
                Activity_Cat_Code = SelectedCategory.Id


            };

            MainViewModel.GetInstance().Location = new LocationViewModel(userQuery, userLog, categories, act, this.PickerCatIndex);
            await Application.Current.MainPage.Navigation.PushAsync(new LocationPage());




        }

        private async void CreateActivity()
        {
            CheckConnectionInternet.checkConnectivity();

            //if ((this.StartDay.Date + this.StartHour) < DateTime.Now)
            //{
            //    await Application.Current.MainPage.DisplayAlert(
            //        "Message",
            //        "Past date is not allowed.",
            //        "Ok");
            //    return;
            //}

            //if (this.StartDay.Date + this.StartHour > this.FinishDay.Date + this.FinishHour)
            //{
            //    await Application.Current.MainPage.DisplayAlert(
            //        "Message",
            //        "The Start Date is greater than Finish Date",
            //        "Ok");
            //    this.FinishDay = this.StartDay;
            //    this.FinishHour = this.StartHour;
            //    return;

            //}
            ValidateDatetime();

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

            if ((this.StartDay.Date + this.StartHour) < DateTime.Now)
            {
                await Application.Current.MainPage.DisplayAlert(
                   "Message",
                   "Past date is not allowed.",
                   "Ok");
                this.StartHour = DateTime.Now.TimeOfDay;
                return;
            }


            string[] addrSplit = this.Placename.Split(',');
            for (int i = 0; i < addrSplit.Length; i++)
            {
                addrSplit[i] = addrSplit[i].Trim();
            }
            int len = RandomId.length.Next(5, 10);
            Activity_Location activity_location = new Activity_Location()
            {
                Id = RandomId.RandomString(len),
                Nameplace = addrSplit[0],
                City = addrSplit.Length == 5 ? addrSplit[2] : addrSplit[1],
                State = "PR",//Cambiarlo luego con IF dinamico
                Country = addrSplit.Length == 5 ? addrSplit[4] : addrSplit[3],
                ZipCode = addrSplit.Length == 5 ? addrSplit[3] : addrSplit[2],
                IsSecure = false,
                Latitude = (decimal)this.loc.Latitude,
				Longitude = (decimal)this.loc.Longitude,
				CreatorOriginalPinLatitude = (decimal)this.origLoc.Latitude,
				CreatorOriginalPinLongitude = (decimal)this.origLoc.Longitude


			};
            try
            {
                //find duplicate
                var query = await App.MobileService.GetTable<Activity_Location>().Where(
                    p => p.Nameplace == activity_location.Nameplace
                    && p.City == activity_location.City
                    && p.ZipCode == activity_location.ZipCode
                    ).ToListAsync();
                if (query.Count == 0)
                {
                    await App.MobileService.GetTable<Activity_Location>().InsertAsync(activity_location);
                }
                else
                {

                    var location = query?.FirstOrDefault();
                    activity_location.Id = location.Id;
                }
               
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }

            Activity activity = new Activity()
            {
                Id = RandomId.RandomString(len),
                Name = this.ActivityName,
                Created_Date = DateTime.Now,
                IsPrivate = false,//todo
                Start_Act_Datetime = this.StartDay.Date + this.StartHour,
                End_Act_Datetime = this.FinishDay.Date + this.FinishHour,
                Description = this.Description.TrimEnd().TrimStart(),
                Status = 1,//check
                IsService = this.IsService,
                Activity_Cat_Code = SelectedCategory.Id,
                Activity_Loc_Id = activity_location.Id 
            };

            //ctivity history
            Activity_History activityHistory = new Activity_History()
            {
                Id = RandomId.RandomString(len),
                Activity_Code_Id = activity.Id,
                Name = this.ActivityName,
                Created_Date = DateTime.Now,
                IsPrivate = false,//todo
                Start_Act_Date = this.StartDay.Date + this.StartHour,
                End_Act_Date = this.FinishDay.Date + this.FinishHour,
                Description = this.Description,
                Status = 1,//check
                IsService = this.IsService,
                Activity_Cat_code = SelectedCategory.Id,
                Activity_Loc_Id_FK = activity_location.Id
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

            try
            {

                await App.MobileService.GetTable<Activity>().InsertAsync(activity);
                await App.MobileService.GetTable<Activity_History>().InsertAsync(activityHistory);
                await App.MobileService.GetTable<User_Log>().InsertAsync(userCreating);
                await App.MobileService.GetTable<User_Entered>().InsertAsync(entry);
                await App.MobileService.GetTable<Entered_History>().InsertAsync(entryHistory);
            


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
            
            //if (possibleAddresses.size() > 0)
            //{
            //    System.out.println(addresses.get(0).getLocality());
            //    System.out.println(addresses.get(0).getCountryName());
            //}


        }

        public async void PickerIndex()
        {
            //await Application.Current.MainPage.DisplayAlert(
            //        "Error",
            //        this.PickerCatIndex.ToString()+" test1",
            //        "Accept");

            //this.PickerCatIndex = pickerCatIndex;
            //await Test();
            await Application.Current.MainPage.DisplayAlert(
                    "Message",
                    "Pin has been saved.",
                    "Ok");
            PickerEvent?.Invoke(this.PickerCatIndex);


        }

        public async Task Test()
        {
            PickerEvent?.Invoke(this.PickerCatIndex);

        }

        public async void ValidateDatetime()
        {
            //if (this.StartDay != null && this.FinishDay != null
            //  && this.StartHour != null && this.FinishHour != null) {
            var myTodayNow = this.TodayNow;
            var DatetimeBegin = this.StartDay.Date + this.StartHour;
            var DatetimeEnd = this.FinishDay.Date + this.FinishHour;
            if (DatetimeBegin > DatetimeEnd)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Message",
                    "The Start Date is greater than Finish Date",
                    "Ok");
                this.FinishDay = this.StartDay;
                this.FinishHour = this.StartHour;
                return;

            }
            if (DatetimeBegin < myTodayNow)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Message",
                    "Past date is not allowed.",
                    "Ok");
               this.StartHour = DateTime.Now.TimeOfDay;
                return;
            }
        }

            //public async void ValidateDatetime()
            //{
            //    if ((this.StartDay.Date + this.StartHour) < DateTime.Now)
            //    {
            //        await Application.Current.MainPage.DisplayAlert(
            //            "Message",
            //            "Past date is not allowed.",
            //            "Ok");
            //        return;
            //    }

            //    if (this.StartDay.Date + this.StartHour > this.FinishDay.Date + this.FinishHour)
            //    {
            //        await Application.Current.MainPage.DisplayAlert(
            //            "Message",
            //            "The Start Date is greater than Finish Date",
            //            "Ok");
            //        this.FinishDay = this.StartDay;
            //        this.FinishHour = this.StartHour;
            //        return;

            //    }
            //}




            //	//}
            //}
            private bool ChangeFinishHour()
        {
            var DatetimeBegin = this.StartDay + this.StartHour;
            var DatetimeEnd = this.FinishDay + this.FinishHour;
            return (DatetimeBegin >= DatetimeEnd);

        }
    }
}
