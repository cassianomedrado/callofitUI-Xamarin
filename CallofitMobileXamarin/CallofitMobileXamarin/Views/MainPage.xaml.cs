using CallofitMobileXamarin.Models.Chamados;
using CallofitMobileXamarin.Services;
using CallofitMobileXamarin.Utils;
using CallofitMobileXamarin.ViewModels;
using CallofitMobileXamarin.Views;
using Newtonsoft.Json;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CallofitMobileXamarin
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            // Ação que será executada quando uma página que sair da pilha direcionar ação para está página
            MessagingCenter.Subscribe<object>(this, "PáginaRemovida", (sender) =>
            {
                AtualizarTotais();
            });

            BindingContext = new ViewModels.MainPageViewModel();
            NavigationPage.SetHasNavigationBar(this, false);
            Initialize();
        }

        private async void Initialize()
        {
            if (!await AuthToken.IsAuthenticatedAsync())
            {
                await AuthToken.ClearTokenAsync();
                await Navigation.PopToRootAsync();
            }
            else
            {
                NavigationPage.SetHasBackButton(this, false);

                AtualizarTotais();

                var nomeUser = await SecureStorage.GetAsync("nome");
                labelBemVindoUser.Text = $"Olá, {nomeUser}";
            }
        }

        private async void UserIcon_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UsuarioPage());
        }

        private async void AddChamado(object sender, EventArgs e)
        {
            AdicionarChamado modalPage = new AdicionarChamado();
            await Navigation.PushModalAsync(modalPage);
        }

        private void FrameChamadosEmAberto_Tapped(object sender, EventArgs e)
        {
            // Adicione aqui a ação desejada para quando o usuário tocar no Frame
        }

        private void FrameChamadosPendentes_Tapped(object sender, EventArgs e)
        {
            // Adicione aqui a ação desejada para quando o usuário tocar no Frame
        }

        private void FrameChamadosFinalizados_Tapped(object sender, EventArgs e)
        {
            // Adicione aqui a ação desejada para quando o usuário tocar no Frame
        }

        private void FrameChamadosAtrasados_Tapped(object sender, EventArgs e)
        {
            // Adicione aqui a ação desejada para quando o usuário tocar no Frame
        }

        private async void AtualizarTotais()
        {
            ChamadosService chamadosService = new ChamadosService();
            try
            {
                //loading.IsVisible = true;
                var response = await chamadosService.RecuperarTotaisChamadosAsync(await SecureStorage.GetAsync("id"));
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var totaisChamados = JsonConvert.DeserializeObject<BuscaTotaisChamadosDTO>(responseContent);
                    if (totaisChamados != null)
                    {
                        labelEmAberto.Text = $"Em Aberto ({totaisChamados.chamadosEmAberto})";
                        labelPendentes.Text = $"Pendentes ({totaisChamados.chamadosPendentes})";
                        labelFinalizados.Text = $"Finalizados ({totaisChamados.chamadosFinalizados})";
                        labelAtrasados.Text = $"Atrasados ({totaisChamados.chamadosAtrasados})";
                    }
                }
                else
                {
                    var errorTratado = await ErrorsHandler.TratarMenssagemErro(response);
                    await DisplayAlert(errorTratado.status.ToString(), errorTratado.errors, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro 500", ex.Message, "OK");
            }
        }
    }
}
