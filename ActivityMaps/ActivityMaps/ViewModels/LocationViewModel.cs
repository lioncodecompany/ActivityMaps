

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

        public delegate void MovePinEvent(bool AllowMovePin);
        public event MovePinEvent movePinEvent;
        private Pin creatorPin;
        private Location loc; //Activity Location
        private string locationtxt;
        private bool movePinAllowed;
        private string movePinButtonText;
        private Location origLoc; //Original Creator Locatoin
        private List<User> userQuery;
        private User_Log userLog;
        private ObservableCollection<Activity_Category> categories;
        private Activity activity;
        private int pickerCatIndex;
        


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
            this.MovePinAllowed = false;
            this.MovePinButtonText = "Allow Move Pin";

            this.userQuery = userQuery;
            this.userLog = userLog;
            this.categories = categories;
            this.activity = act;
            this.pickerCatIndex = pickerCatIndex;
        }
        public LocationViewModel()
        {
            instance = this;
            this.Locationtxt = "";
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
            this.CreatorPin = new Pin
            {
                Type = PinType.Place,
                Position = position,
                Label = "Activity Location"
                
            };
        }

        public async Task LoadSearchPin()
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








        public async void SearchLocation()
        {
            //Search Location
            try
            {
                var address = this.locationtxt + " Puerto Rico"; //Get Country from user address database
                var locations = await Geocoding.GetLocationsAsync(address);
                

                var location = locations?.FirstOrDefault();
                
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
