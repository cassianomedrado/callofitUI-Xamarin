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
using Xamarin.Essentials;
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
                await Navigation.PushAsync(new MainPage());
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
                    var token = JsonConvert.DeserializeObject<TokenDTO>(responseContent);

                    if (token != null)
                    {
                        await AuthToken.SetTokenAsync(token.Token);
                        await AuthToken.SetExpirationAsync(DateTime.Now.AddSeconds(token.expires_in));

                        var responseRecuperaUsuario = await loginService.RecuperarDadosUsuarioAsync(usuarioinput.Text);
                        if (responseRecuperaUsuario.IsSuccessStatusCode)
                        {
                            var responseRecuperaUsuarioContent = await responseRecuperaUsuario.Content.ReadAsStringAsync();
                            var usuarioModel = JsonConvert.DeserializeObject<UsuarioDTO>(responseRecuperaUsuarioContent);
                            if (usuarioModel != null)
                            {
                                loginSuccessful = true;
                                await SecureStorage.SetAsync("id", usuarioModel.id.ToString());
                                await SecureStorage.SetAsync("data_criacao", usuarioModel.data_criacao.ToString());
                                await SecureStorage.SetAsync("nome", usuarioModel.nome);
                                await SecureStorage.SetAsync("email", usuarioModel.email);
                                await SecureStorage.SetAsync("tipo_usuario_id", usuarioModel.tipo_usuario_id.ToString());
                                await SecureStorage.SetAsync("username", usuarioModel.username);
                                await SecureStorage.SetAsync("status", usuarioModel.status.ToString());
                            }
                            else
                            {
                                loading.IsVisible = false;
                                await AuthToken.ClearTokenAsync();
                                await DisplayAlert("Error", "Não foi possível carregar dados do usuário.", "OK");
                            }
                        }
                        else
                        {
                            loading.IsVisible = false;
                            await AuthToken.ClearTokenAsync();
                            var errorTratado = await ErrorsHandler.TratarMenssagemErro(responseRecuperaUsuario);
                            await DisplayAlert(errorTratado.status.ToString(), errorTratado.errors, "OK");
                        }
                    }
                    else
                    {
                        loading.IsVisible = false;
                        await AuthToken.ClearTokenAsync();
                        await DisplayAlert("Error", "Token de acesso não gerado.", "OK");
                    }
                }
                else
                {
                    loading.IsVisible = false;
                    await AuthToken.ClearTokenAsync();
                    var errorTratado =  await ErrorsHandler.TratarMenssagemErro(response);
                    await DisplayAlert(errorTratado.status.ToString(), errorTratado.errors, "OK");
                }
            }
            catch (Exception ex)
            {
                loading.IsVisible = false;
                await AuthToken.ClearTokenAsync();
                await DisplayAlert("Erro 500", ex.Message, "OK");
            }
            
            if (loginSuccessful)
            {
                loading.IsVisible = false;
                await Navigation.PushAsync(new MainPage());
            }
        }
    }
}