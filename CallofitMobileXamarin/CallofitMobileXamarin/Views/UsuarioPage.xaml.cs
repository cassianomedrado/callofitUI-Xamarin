using CallofitMobileXamarin.Utils;
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


            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetBackButtonTitle(this, ""); // Remove o texto "Voltar" do botão de navegação

            NavigationPage.SetTitleView(this, new Label { Text = "Usuário", TextColor = Color.White, FontAttributes = FontAttributes.Bold }); // Define o título da página

            NavigationPage navPage = (NavigationPage)Application.Current.MainPage;
            navPage.BarBackgroundColor = Color.FromHex("#212529");

            Initialize();
        }

        private async void Initialize()
        {
            if (!await AuthToken.IsAuthenticatedAsync())
            {
                await AuthToken.ClearTokenAsync();
                await Navigation.PushAsync(new LoginPage());
            }
        }

        private async void LogoutButton_Clicked(object sender, EventArgs e)
        {
            await AuthToken.ClearTokenAsync();
            await Navigation.PushAsync(new LoginPage());
        }

        private async void ChangePasswordButton_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new ChangePasswordPage());
        }
    }
}