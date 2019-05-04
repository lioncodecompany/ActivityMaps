

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

    public class LocationViewModel:BaseViewModel
    {
        #region attributes
        public delegate void MyEventAction(string someParameter);
        public event MyEventAction MyEvent;

        public delegate void mySafeEvent();
        public event mySafeEvent SafeEvent;

        public delegate void MovePinEvent(bool AllowMovePin);
        public event MovePinEvent movePinEvent;
        private Activity_Location selectedFilter;
        private Pin creatorPin;
        private Location loc; //Activity Location
        private string locationtxt;
        private bool movePinAllowed;
        private string movePinButtonText;
        private Location origLoc; //Original Creator Locatoin
        private List<User> userQuery;
        private User_Log userLog;
        private ObservableCollection<Activity_Category> categories;
        private ObservableCollection<Activity_Location> filters; //pickers safe location
        private Activity activity;
        private int pickerCatIndex;
        private bool savePinEnabled;
        private ObservableCollection<Activity_Location> locations; //pickers safe location
        private Activity_Location selectedSafeLoc;




        #endregion

        #region Properties
        public string Locationtxt
        {

            get { return this.locationtxt; }
            set
            {

                SetValue(ref this.locationtxt, value);

              // SearchLocation();

            }
        }
        public bool SavePinEnabled
        {

            get { return this.savePinEnabled; }
            set
            {

                SetValue(ref this.savePinEnabled, value);

                // SearchLocation();

            }
        }
        public Location Loc
        {
            get { return this.loc; }
            set { SetValue(ref this.loc, value); }
        }
        public Location OrigLoc
        {
            get { return this.origLoc; }
            set { SetValue(ref this.origLoc, value); }
        }

        public bool MovePinAllowed
        {
            get { return this.movePinAllowed; }
            set { SetValue(ref this.movePinAllowed, value); }
        }
        public string MovePinButtonText
        {
            get { return this.movePinButtonText; }
            set { SetValue(ref this.movePinButtonText, value); }
        }

        public ObservableCollection<Activity_Location> Filters
        {


            get { return this.filters; }
            set { SetValue(ref this.filters, value); }


        }
        public ObservableCollection<Activity_Location> Locations
        {


            get { return this.locations; }
            set { SetValue(ref this.locations, value); }


        }

        public Activity_Location SelectedSafeLoc
        {
            get { return this.selectedSafeLoc; }
            set
            {
                
               SetValue(ref this.selectedSafeLoc, value);
                this.Loc.Latitude = (double)this.selectedSafeLoc.Latitude;
                this.Loc.Longitude = (double)this.selectedSafeLoc.Longitude;
                SafeEvent?.Invoke();
                //SafePin();

            }
        }

        //private async void SafePin()
        //{
        //     LoadSafePin();
        //}

        public Activity_Location SelectedFilter
        {
            get { return this.selectedFilter; }
            set { SetValue(ref this.selectedFilter, value); }
        }


        public Pin CreatorPin
        {
            get { return this.creatorPin; }
            set { SetValue(ref this.creatorPin, value); }
        }
        #endregion

        #region Commando
        public ICommand SavePinCommand
        {
            get
            {
                return new RelayCommand(SavePin);
            }

        }
        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(CallSearchLocationPin);
            }

        }

        public ICommand AllowMovePinCommand
        {
            get
            {
                return new RelayCommand(AllowMovePin);
            }

        }
        #endregion

        #region Constructors
        public LocationViewModel(List<User> userQuery, User_Log userLog, ObservableCollection<Activity_Category> categories, Activity act, int pickerCatIndex)
        {
            instance = this;
            this.Locationtxt = "";
            this.SavePinEnabled = false;
            this.MovePinAllowed = false;
            this.MovePinButtonText = "Allow Move Pin";

            this.userQuery = userQuery;
            this.userLog = userLog;
            this.categories = categories;
            this.activity = act;
            this.pickerCatIndex = pickerCatIndex;

            LoadSafeLocation();
        }

        private async void LoadSafeLocation()
        {
            try
            {
                
                var query = await App.MobileService.GetTable<Activity_Location>().Where(p => p.IsSecure == false).ToListAsync();
                this.Locations = new ObservableCollection<Activity_Location>();

                var arr2 = query.ToArray();
                for (int idx = 0; idx < arr2.Length; idx++)
                {

                    this.Locations.Add(new Activity_Location
                    {
                        Id = arr2[idx].Id,
                        Nameplace = arr2[idx].Nameplace,
                        City = arr2[idx].City,
                        State = arr2[idx].State,
                        Country = arr2[idx].Country,
                        Latitude = arr2[idx].Latitude,
                        Longitude = arr2[idx].Longitude
                    });
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }

            //var query = from act
            //                         in Activities
            //            join cat in Categories on act.Activity_Cat_Code equals cat.Id
            //            join loc in Locations on act.Activity_Loc_Id equals loc.Id
            //            where (act.Name.ToUpper().StartsWith(this.Activitytxt.ToUpper()))
            //            ||
            //            (cat.Name.ToUpper().StartsWith(this.Activitytxt.ToUpper()))
            //            //||
            //            //(loc.City.ToUpper().StartsWith(this.Activitytxt.ToUpper()))
            //            select new Activity_Location

            //            {
            //                Id = act.Id,
            //                Name = act.Name,
            //                CategoryName = cat.Name,
            //                Description = act.Description,
            //                LocationTown = loc.City != "" ? "Location: " + loc.City : "Location: Unknown",
            //                Created_Date = act.Created_Date,
            //                Activity_Loc_Id = act.Activity_Loc_Id,
            //                IsService = act.IsService,
            //                Start_Act_Datetime = act.Start_Act_Datetime,
            //                End_Act_Datetime = act.End_Act_Datetime,
            //                Color = act.IsService ? "Green" : "Gray",
            //                CountPeople = (from users in users_entered
            //                               where (users.Activity_Code_FK2.Equals(act.Id))
            //                               select users).Count()


            //            };
            //this.ActivityResult = query.ToList();
        }

        public LocationViewModel()
        {
            instance = this;
            this.Locationtxt = "";
            this.SavePinEnabled = false;
            this.MovePinAllowed = false;
            this.MovePinButtonText = "Allow Move Pin";
           // this.CreatorPin = new Pin();
        }
        #endregion

        #region SignLeton
        private static LocationViewModel instance;

        public static LocationViewModel GetInstance()
        {
            if (instance == null)
            {
                return new LocationViewModel();
            }
            return instance;
        }
        #endregion


        #region Methods
        public async void SavePin()
        {

            MainViewModel.GetInstance().CreateActivity = 
                new CreateActivityViewModel(userQuery, userLog,categories,this.activity, this.Loc, this.OrigLoc, this.pickerCatIndex);
            await Application.Current.MainPage.Navigation.PushAsync(new CreateActivityPage());
        }
        public async Task LoadPin()
        {
           
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            this.Loc = await Geolocation.GetLocationAsync(request);
            this.OrigLoc = this.Loc;

            var position = new Position(this.Loc.Latitude, this.Loc.Longitude); // Latitude, Longitude
            if (position != null)
            {
                this.CreatorPin = new Pin
                {
                    Type = PinType.Place,
                    Position = position,
                    Label = "Activity Location"

                };
            }
            else
            {
                var pos = new Position(18.2, -66); // Latitude, Longitude
                this.CreatorPin = new Pin
                {
                    Type = PinType.Place,
                    Position = pos,
                    Label = "Activity Location"

                };
            }
            
        }

        public async Task LoadSearchPin()
        {
            
            var position = new Position(this.Loc.Latitude, this.Loc.Longitude); // Latitude, Longitude
            this.CreatorPin.Position = position;


        }

        public async Task LoadSafePin()
        {

            
            var position = new Position(this.Loc.Latitude, this.Loc.Longitude); // Latitude, Longitude
            this.CreatorPin.Position = position;


        }

        public async void CallSearchLocationPin()
        {
            
            SearchLocation();
            

        }


        public async void AllowMovePin()
        {
            this.MovePinAllowed = !this.MovePinAllowed;
            if (this.MovePinAllowed)
            {
                this.MovePinButtonText = "Lock Move Pin";
            }
            else
            {
                this.MovePinButtonText = "Allow Move Pin";
            }
            
            movePinEvent?.Invoke(this.MovePinAllowed);


        }

        //public delegate void MyEventAction(string someParameter);
        //public event MyEventAction MyEvent;

        //// rise event when you need to
        //MyEvent?.Invoke("123");




        //public async void SafeLocation()
        //{

        //}



            public async void SearchLocation()
        {
            //Search Location
            try
            {
                var address = this.locationtxt + " Puerto Rico"; //Get Country from user address database
                var locations = await Geocoding.GetLocationsAsync(address);
                
                
                var location = locations?.FirstOrDefault();

                //test
                //geoCoder = new Geocoder();
                //var fortMasonPosition = new Position(location.Latitude, location.Longitude);
                //var possibleAddresses = await geoCoder.GetAddressesForPositionAsync(fortMasonPosition);
                //var placetest = possibleAddresses?.FirstOrDefault();
                //await Application.Current.MainPage.DisplayAlert("Test", placetest, "Ok");
                //return;

                if (location != null)
                {
                    // Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    this.Loc = location;
                    MyEvent?.Invoke(this.Locationtxt); // el parametro es innecesario luego se elimina
                    //await LoadSearchPin();
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Handle exception that may have occurred in geocoding
            }
        }

        public void  Test()
        {
            //Console.WriteLine(this.Locationtxt);
            MyEvent?.Invoke(this.Locationtxt);
        }
        #endregion

    }
}
