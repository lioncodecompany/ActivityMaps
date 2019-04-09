﻿using System;
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

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LocationPage : ContentPage
    {
		public LocationPage ()
		{
			InitializeComponent ();
            //Default location: Puerto Rico
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(18.2, -66), Distance.FromMiles(15)));
            MyPosition();
           

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
            }
            else
            {
                Console.WriteLine(MyMap.VisibleRegion.Center + "    ****TEST CLICKED*****");
            }
        }

        private void MyMap_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            if (e.PropertyName == "VisibleRegion" && MyMap.VisibleRegion != null)
            {
                MovePin();
            }

        }


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