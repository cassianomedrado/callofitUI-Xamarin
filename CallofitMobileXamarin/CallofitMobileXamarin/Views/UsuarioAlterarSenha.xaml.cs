using CallofitMobileXamarin.Models.Login;
using CallofitMobileXamarin.Services;
using CallofitMobileXamarin.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CallofitMobileXamarin.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UsuarioAlterarSenha : ContentPage
	{
		public UsuarioAlterarSenha ()
		{
			InitializeComponent ();
		}

        private async void SalvarAlteracaoBurronClick(object sender, EventArgs e)
        {
            LoginService loginService = new LoginService();
            bool result = await DisplayAlert("Alterar Senha", $"Deseja seguir com a alteração da senha?", "Sim", "Não");

            if (result)
            {
                try
                {
                    loading.IsVisible = true;
                    var response = await loginService.AlterarSenhaAsync(new RequestAlterarSenhaUsuario()
                    {
                        username = await SecureStorage.GetAsync("username"),
                        email = emailInput.Text,
                        senhaAtual = senhaAtualinput.Text,
                        senhaNova = senhaNovainput.Text,
                        confirmaNovaSenha = repitaSenhainput.Text
                    });

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var chamadosEmAberto = JsonConvert.DeserializeObject(responseContent);
                        loading.IsVisible = false;
                        await DisplayAlert("SUCESSO!!", "Senha alterada com sucesso!", "OK");
                        await Navigation.PopModalAsync();
                    }
                    else
                    {
                        loading.IsVisible = false;
                        var errorTratado = await ErrorsHandler.TratarMenssagemErro(response);
                        await DisplayAlert(errorTratado.status.ToString(), errorTratado.errors, "OK");
                    }
                }
                catch (Exception ex)
                {
                    loading.IsVisible = false;
                    await DisplayAlert("Erro 500", ex.Message, "OK");
                }
            }
        }

        private async void CancelarButtonClick(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}