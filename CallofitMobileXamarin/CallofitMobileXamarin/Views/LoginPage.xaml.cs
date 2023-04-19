using CallofitMobileXamarin.Models.Login;
using CallofitMobileXamarin.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CallofitMobileXamarin.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            Initialize();
        }

        private async void Initialize()
        {
            if (await AuthToken.IsAuthenticatedAsync())
            {
                await AuthToken.ClearTokenAsync();
                Application.Current.MainPage = new NavigationPage(new MainPage());
                await Navigation.PushAsync(new MainPage());
                Navigation.RemovePage(this);
            }
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            LoginService loginService = new LoginService();
            bool loginSuccessful = false;

            var token = await loginService.LoginAsync(usuarioinput.Text, senhainput.Text);

            if (token != null)
            {
                loginSuccessful = true;

                await AuthToken.SetTokenAsync(token.Token);
                await AuthToken.SetExpirationAsync(DateTime.Now.AddSeconds(token.expires_in));
            }

            if (loginSuccessful)
            {
                Application.Current.MainPage = new NavigationPage(new MainPage());
                await Navigation.PushAsync(new MainPage());
                Navigation.RemovePage(this);
            }
            else
            {
                await DisplayAlert("Erro", "Nome de usuário ou senha inválidos", "OK");
            }
        }
    }
}