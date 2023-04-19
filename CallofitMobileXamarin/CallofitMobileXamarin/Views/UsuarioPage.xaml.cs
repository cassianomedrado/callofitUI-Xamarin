using CallofitMobileXamarin.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CallofitMobileXamarin.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UsuarioPage : ContentPage
	{
		public UsuarioPage()
		{
			InitializeComponent ();
            Initialize();

        }
        private async void Initialize()
        {
            if (await AuthToken.IsAuthenticatedAsync())
            {
                await AuthToken.ClearTokenAsync();
                Application.Current.MainPage = new NavigationPage(new LoginPage());
                await Navigation.PopToRootAsync();
            }
        }

        private async void LogoutButton_Clicked(object sender, EventArgs e)
        {
            await AuthToken.ClearTokenAsync();
            Application.Current.MainPage = new NavigationPage(new LoginPage());
            await Navigation.PopToRootAsync();
        }
        private async void ChangePasswordButton_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new ChangePasswordPage());
        }
    }
}