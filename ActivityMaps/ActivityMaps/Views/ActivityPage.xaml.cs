using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ActivityMaps.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivityPage : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }

        public ActivityPage()
        {
            InitializeComponent();

            Items = new ObservableCollection<string>
            {
                "Item 1",
                "Item 2",
                "Item 3",
                "Item 4",
                "Item 5",
				"Item 6",
				"Item 7",
				"Item 8",
				"Item 9",
				"Item 10",
				"Item 11",
				"Item 12",
				"Item 13",
				"Item 14",
				"Item 15",
				"Item 16",

			};
			
			MyListView.ItemsSource = Items;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped. " + e.Item.ToString(), "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
