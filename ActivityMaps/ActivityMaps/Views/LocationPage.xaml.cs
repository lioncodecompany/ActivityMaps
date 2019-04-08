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

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LocationPage : ContentPage
    {
		public LocationPage ()
		{
			InitializeComponent ();
            //MyPosition();

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
            MyMap.Pins.Add(pin);

        }
  

        public void OnMapClicked(object sender, EventArgs args)
        {
            Console.WriteLine("****TEST CLICKED*****");
        }
        //void OnMapLongClick(object sender, MapLongClickEventArgs e)
        //{
        //    if (MyMap == null) return;

        //    var position = e.Point.ToPosition();

        //}

    }
}