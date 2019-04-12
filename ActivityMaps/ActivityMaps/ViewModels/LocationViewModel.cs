

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

    public class LocationViewModel:BaseViewModel
    {
        #region attributes
        public delegate void MyEventAction(string someParameter);
        public event MyEventAction MyEvent;
        private Pin creatorPin;
        private Location loc;
        private string locationtxt;



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


        public Pin CreatorPin
        {
            get { return this.creatorPin; }
            set { SetValue(ref this.creatorPin, value); }
        }
        #endregion

        #region Commando
        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(CallSearchLocationPin);
            }

        }
        #endregion

        #region Constructors
        public LocationViewModel()
        {
            instance = this;
            this.Locationtxt = "";
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
        public async Task LoadPin()
        {
           
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            this.Loc = await Geolocation.GetLocationAsync(request);

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
