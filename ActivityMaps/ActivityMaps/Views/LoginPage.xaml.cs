
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ActivityMaps.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{

		public LoginPage ()
		{
			InitializeComponent();
			NavigationPage.SetHasBackButton(this, false);
		}

	}
}