using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;

namespace ActivityMaps.Views
{
    using ViewModels;
    using Xamarin.Essentials;

    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LocationPage : ContentPage
    {
        private bool MovePinAllowed;
		public LocationPage ()
		{
			InitializeComponent ();
            MovePinAllowed = false;
            //Default location: Puerto Rico
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(18.2, -66), Distance.FromMiles(15)));
            MyPosition();

            var locationVM = LocationViewModel.GetInstance();
            // locationVM.DoSomething += DoSomething;

            locationVM.MyEvent += (someParameter) => DoSomething(someParameter);
            locationVM.movePinEvent += (someParameter) => AllowMovePin(someParameter);



        }

        private void AllowMovePin(bool moveBinAllowed)
        {
            MovePinAllowed = moveBinAllowed;
        }

        private async void DoSomething(string str)
        {
            await LoadSearchPin();
        }

        async void MyPosition()
        {

            await LoadPin();
        }

        async Task LoadPin()
        {
            var locationVM = LocationViewModel.GetInstance();
            await locationVM.LoadPin();
            var pin = locationVM.CreatorPin;
            var location = locationVM.Loc;


            MyMap.Pins.Add(pin);
            if (MyMap.Pins.Count == 0)
            {
                locationVM.CreatorPin = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(18.2,-66 ),
                    Label = "Activity Location"


                };
                var pin2 = locationVM.CreatorPin;
                MyMap.Pins.Add(pin2);
            }

                //move Screen
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                new Position(location.Latitude, location.Longitude), Distance.FromMiles(3)));

            //Enable Save Pin Button
            locationVM.SavePinEnabled = true;


        }


        async Task LoadSearchPin()
        {
            
            var locationVM = LocationViewModel.GetInstance();
            await locationVM.LoadSearchPin();
            var pin = locationVM.CreatorPin;
            var location = locationVM.Loc;
            var position = new Position(location.Latitude, location.Longitude);
            MyMap.Pins[0].Position = position;

            //move Screen
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                new Position(location.Latitude, location.Longitude), Distance.FromMiles(3)));

        }

        void MovePin()
        {
            if (MyMap.Pins.Count > 0)
            {
                
                MyMap.Pins[0].Position = MyMap.VisibleRegion.Center;
                Console.WriteLine(MyMap.VisibleRegion.Center.Latitude + "    ****TEST CLICKED*****");
                Console.WriteLine(MyMap.VisibleRegion.Center.Longitude + "    ****TEST CLICKED*****");
                var locationVM = LocationViewModel.GetInstance();
                locationVM.Loc.Latitude = MyMap.Pins[0].Position.Latitude;
                locationVM.Loc.Longitude = MyMap.Pins[0].Position.Longitude;
                //MyMap.Pins[0].Position.Latitude;
                // MyMap.Pins[0].Position.Longitude;
            }
            else
            {
                Console.WriteLine(MyMap.VisibleRegion.Center + "    ****TEST CLICKED*****");
            }
        }

        private void MyMap_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            if (e.PropertyName == "VisibleRegion" && MyMap.VisibleRegion != null && MovePinAllowed)
            {
                MovePin();
            }

        }

        //public void LoadSearchLocation()
        //{

        //}


        //public void OnMapClicked(object sender, EventArgs args)
        //{
        //    Console.WriteLine("****TEST CLICKED*****");
        //}
        //void OnMapLongClick(object sender, MapLongClickEventArgs e)
        //{
        //    if (MyMap == null) return;

        //    var position = e.Point.ToPosition();

        //}

        //private void MyMap_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    Console.WriteLine("****TEST CLICKED*****");
        //}
    }
}