
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ActivityMaps.Views
{
    using ViewModels;
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateActivityPage : ContentPage
	{
        private int pickerCatIndex;
		public CreateActivityPage ()
		{
			InitializeComponent ();
            var createActivityVM = CreateActivityViewModel.GetInstance();
            createActivityVM.PickerEvent += (someParameter) => PickerIndex(someParameter);
            


        }

        private void PickerIndex(int pickerIndex)
        {
            Console.WriteLine("****$@$#TEST: "+ pickerIndex.ToString());
           // await LoadPicketCat(); 
        }

        async Task LoadPicketCat()
        {
            //PickerCat.SelectedIndex = pickerIndex;
        }

            private void PickerCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            setPickCatIndex();
        }

        void setPickCatIndex()
        {
            var createActivityVM = CreateActivityViewModel.GetInstance();
            createActivityVM.PickerCatIndex = PickerCat.SelectedIndex;
        }
    }
}