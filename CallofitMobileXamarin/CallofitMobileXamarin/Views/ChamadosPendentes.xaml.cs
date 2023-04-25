using CallofitMobileXamarin.Models.Chamados;
using CallofitMobileXamarin.Services;
using CallofitMobileXamarin.Utils;
using Newtonsoft.Json;
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
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChamadosPendentes : ContentPage
	{
		public ChamadosPendentes ()
		{
			InitializeComponent ();

            // Ação que será executada quando uma página que sair da pilha direcionar ação para está página
            MessagingCenter.Subscribe<object>(this, "PáginaRemovida", (sender) =>
            {
                loadValues();
            });

            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetBackButtonTitle(this, ""); // Remove o texto "Voltar" do botão de navegação

            //NavigationPage.SetTitleView(this, new Label { Text = "Chamados Em Aberto", TextColor = Color.White , FontAttributes = FontAttributes.Bold }); // Define o título da página

            var titleLabel = new Label
            {
                Text = "Chamados Pendentes",
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(0, 0, 0, 0),
            };

            var image = new Image
            {
                Source = "pendente.gif",
                WidthRequest = 50,
                HeightRequest = 45,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Margin = new Thickness(0, 0, 5, 7),
                IsAnimationPlaying = true
            };

            var titleView = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 5,
                Padding = new Thickness(5),
                Children = { titleLabel, image }
            };

            NavigationPage.SetTitleView(this, titleView);

            NavigationPage navPage = (NavigationPage)Application.Current.MainPage;
            navPage.BarBackgroundColor = Color.FromHex("#212529");

            loadValues();
        }

        // Método para quando fechar a página e direcionar uma ação a página anterior da pilha
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send<object>(this, "PáginaRemovida");
        }

        private async void loadValues()
        {
            ChamadosService chamadosService = new ChamadosService();

            try
            {
                loading.IsVisible = true;
                var response = await chamadosService.RecuperarChamadosAsync(new BuscarChamadosRequest()
                {
                    usuario_id = int.Parse(await SecureStorage.GetAsync("id")),
                    status_chamado_id = 2, //Pendentes
                    tecnico_usuario_id = 0
                });

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var chamadosPendentes = JsonConvert.DeserializeObject<List<ChamadoDTO>>(responseContent);
                    lstChamadosPendentes.ItemsSource = chamadosPendentes;
                    loading.IsVisible = false;
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

        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var itemSelecionado = e.Item as ChamadoDTO;

            await Navigation.PushModalAsync(new ChamadosPage(itemSelecionado));

            lstChamadosPendentes.SelectedItem = null;
        }
    }
}