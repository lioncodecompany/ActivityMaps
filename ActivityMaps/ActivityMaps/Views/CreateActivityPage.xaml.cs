using ActivityMaps.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ActivityMaps.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateActivityPage : ContentPage
	{
		public CreateActivityPage ()
		{
			InitializeComponent ();
            //var createActivityVM = CreateActivityViewModel.GetInstance();
             //createActivityVM.PickerEvent += (someParameter) => PickerIndex(someParameter);
            


        }

        private void PickerIndex(int pickerIndex)
        {
            PickerCat.SelectedIndex = pickerIndex;
        }
    }
}