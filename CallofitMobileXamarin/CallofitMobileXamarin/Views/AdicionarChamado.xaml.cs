using CallofitMobileXamarin.Models.Chamados;
using CallofitMobileXamarin.Services;
using CallofitMobileXamarin.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace CallofitMobileXamarin.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AdicionarChamado : ContentPage
	{
		public AdicionarChamado()
		{
			InitializeComponent();
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
            SistemasSuportadosService sistemasSuportadosService = new SistemasSuportadosService();

            try
            {
                solicitanteInput.Text = await SecureStorage.GetAsync("nome");
                var responseStatus = await chamadosService.RecuperarStatusChamadosAsync();
                if (responseStatus.IsSuccessStatusCode)
                {
                    var responseStatusContent = await responseStatus.Content.ReadAsStringAsync();
                    var listStatusChamados = JsonConvert.DeserializeObject<List<StatusChamadosDTO>>(responseStatusContent);
                    if (listStatusChamados != null)
                    {
                        // definir origem de itens do Picker
                        StatusChamadoInput.ItemsSource = listStatusChamados;
                        StatusChamadoInput.ItemDisplayBinding = new Binding("descricao");
                        StatusChamadoInput.FontAttributes = FontAttributes.Bold;
                        StatusChamadoInput.TextColor = Color.White; 

                        // selecionar item com ID igual a 2
                        StatusChamadoInput.SelectedItem = listStatusChamados.FirstOrDefault(o => o.id == 1); //PADRÃO EM ABERTO
                        StatusChamadoInput.Background = Color.Blue;
                        StatusChamadoInput.IsEnabled = false;   
                    }
                }
                else
                {
                    var errorTratado = await ErrorsHandler.TratarMenssagemErro(responseStatus);
                    await DisplayAlert(errorTratado.status.ToString(), errorTratado.errors, "OK");
                }

                var responseSistemas = await sistemasSuportadosService.RecuperarSistemasSuportadosAsync();
                if (responseSistemas.IsSuccessStatusCode)
                {
                    var responseSistemasContent = await responseSistemas.Content.ReadAsStringAsync();
                    var listSistemasSuportados = JsonConvert.DeserializeObject<List<RequestSistemasContratados>>(responseSistemasContent);
                    if (listSistemasSuportados != null)
                    {
                        // definir origem de itens do Picker
                        OpcoesSistemaSuportadoInput.ItemsSource = listSistemasSuportados;
                        OpcoesSistemaSuportadoInput.ItemDisplayBinding = new Binding("nome");
                    }
                }
                else
                {
                    var errorTratado = await ErrorsHandler.TratarMenssagemErro(responseSistemas);
                    await DisplayAlert(errorTratado.status.ToString(), errorTratado.errors, "OK");
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro 500", ex.Message, "OK");
            }
        }

        private async void FecharAddChamado(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void SalvarChamado(object sender, EventArgs e)
        {
            loading.IsVisible = true;
            StringBuilder sb = new StringBuilder(""); 
            if (String.IsNullOrEmpty(solicitanteInput.Text))
            {
                sb.Append("* Solicitante deve ser informado.");
                sb.AppendLine();
            }

            var itemSistemasContratadosSelected = (RequestSistemasContratados)OpcoesSistemaSuportadoInput.SelectedItem;

            if (itemSistemasContratadosSelected == null)
            {
                sb.Append("* Sistema deve ser informado.");
                sb.AppendLine();
            }

            if (String.IsNullOrEmpty(descricaoProblemaInput.Text))
            {
                sb.Append("* Problema deve ser informado.");
                sb.AppendLine();
            }

            if (!String.IsNullOrEmpty(sb.ToString()))
            {
                loading.IsVisible = false;
                await DisplayAlert("Campos obrigatórios", sb.ToString(), "OK");
            }
            else
            {

                DateTime dataSelecionada = dataLimiteInput.Date;
                var itemStatusChamadoSelected = (StatusChamadosDTO)StatusChamadoInput.SelectedItem;

                ChamadoPostDTO chamado = new ChamadoPostDTO()
                {
                    solicitante = solicitanteInput.Text,
                    sistema_suportado_id = itemSistemasContratadosSelected.id,
                    status_chamado_id = itemStatusChamadoSelected.id,
                    data_limite = dataSelecionada,
                    descricao_problema = descricaoProblemaInput.Text,
                    descricao_solucao = string.Empty,
                    tipo_chamado_id = 6, //PADRÃO NORMAL,
                    usuario_id = int.Parse(await SecureStorage.GetAsync("id"))
                };

                try
                {
                    bool result = await DisplayAlert("Confirmação", $"Deseja realizar a abertura do chamado?", "Sim", "Não");

                    if (result)
                    {
                        ChamadosService chamadosService = new ChamadosService();
                        var response = await chamadosService.AbrirChamadoAsync(chamado);
                        if (response.IsSuccessStatusCode)
                        {
                            loading.IsVisible = false;
                            await DisplayAlert("SUCESSO!!", "Chamado aberto com sucesso!", "OK");
                            await Navigation.PopModalAsync();
                        }
                        else
                        {
                            loading.IsVisible = false;
                            var errorTratado = await ErrorsHandler.TratarMenssagemErro(response);
                            await DisplayAlert(errorTratado.status.ToString(), errorTratado.errors, "OK");
                        }
                    }
                    else
                    {
                        loading.IsVisible = false;
                    }                
                }
                catch (Exception ex)
                {
                    loading.IsVisible = false;
                    await DisplayAlert("Erro 500", ex.Message, "OK");
                }
            }
        }  
    }
}