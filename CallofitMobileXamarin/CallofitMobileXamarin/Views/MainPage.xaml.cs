using CallofitMobileXamarin.Models.Login;
using CallofitMobileXamarin.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CallofitMobileXamarin
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
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

        private async void UserIcon_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UsuarioPage());
        }

        private async void HandleAddClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UsuarioPage());
        }
    }
}
