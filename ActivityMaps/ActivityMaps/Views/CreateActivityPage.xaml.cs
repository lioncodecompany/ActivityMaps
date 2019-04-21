
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
            //Application.Current.MainPage.DisplayAlert(
            //            "Error",
            //            pickerIndex.ToString() + " Test2",
            //            "Accept");

            PickerCat.SelectedIndex = pickerIndex;

            //await Test(pickerIndex);
            //Console.WriteLine("****$@$#TEST$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$: "+ pickerIndex.ToString());

        }

        async Task Test(int pickerCatIndex)
        {
            await Application.Current.MainPage.DisplayAlert(
                         "Error",
                         pickerCatIndex.ToString() + " Test2",
                         "Accept");
        }

        //async Task LoadPicketCat(int pickerIndex)
        //{
        //    await Application.Current.MainPage.DisplayAlert(
        //            "Error",
        //            pickerIndex.ToString() + " Test2",
        //            "Accept");
        //    //PickerCat.SelectedIndex = pickerIndex;
        //}

        private void PickerCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            var createActivityVM = CreateActivityViewModel.GetInstance();
            createActivityVM.PickerCatIndex = PickerCat.SelectedIndex;
        }

        void setPickCatIndex()
        {
            var createActivityVM = CreateActivityViewModel.GetInstance();
            createActivityVM.PickerCatIndex = PickerCat.SelectedIndex;
        }
    }
}