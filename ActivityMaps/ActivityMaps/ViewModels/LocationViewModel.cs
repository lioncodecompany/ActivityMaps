using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;
using Xamarin.Essentials;

namespace ActivityMaps.ViewModels
{
    public class LocationViewModel:BaseViewModel
    {
        #region attributes
        private Pin creatorPin;

        #endregion

        #region Properties
        public Pin CreatorPin
        {
            get { return this.creatorPin; }
            set { SetValue(ref this.creatorPin, value); }
        }
        #endregion

        #region Constructors
        public LocationViewModel()
        {
            instance = this;
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
            var location = await Geolocation.GetLocationAsync(request);
            var position = new Position(location.Latitude, location.Longitude); // Latitude, Longitude
            this.CreatorPin = new Pin
            {
                Type = PinType.Place,
                Position = position,
                Label = "Activity Location"
                
            };
        }
        #endregion

    }
}
