using CallofitMobileXamarin.Models.Login;
using CallofitMobileXamarin.Models.Requests;
using CallofitMobileXamarin.Services;
using CallofitMobileXamarin.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

            try
            {
                loading.IsVisible = true;
                var response = await loginService.LoginAsync(usuarioinput.Text, senhainput.Text);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var token = JsonConvert.DeserializeObject<TokenModel>(responseContent);

                    if (token != null)
                    {
                        loginSuccessful = true;

                        await AuthToken.SetTokenAsync(token.Token);
                        await AuthToken.SetExpirationAsync(DateTime.Now.AddSeconds(token.expires_in));
                    }
                }
                else
                {
                    loading.IsVisible = false;
                    var errorTratado =  await ErrorsHandler.TratarMenssagemErro(response);
                    await DisplayAlert(errorTratado.status.ToString(), errorTratado.errors, "OK");
                }
            }
            catch (Exception ex)
            {
                loading.IsVisible = false;
                await DisplayAlert("Erro 500", ex.Message, "OK");
            }
            
            if (loginSuccessful)
            {
                loading.IsVisible = false;
                Application.Current.MainPage = new NavigationPage(new MainPage());
                await Navigation.PushAsync(new MainPage());
                Navigation.RemovePage(this);
            }
        }
    }
}