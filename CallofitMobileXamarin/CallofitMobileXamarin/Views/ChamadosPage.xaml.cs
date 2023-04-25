using CallofitMobileXamarin.Models.Chamados;
using CallofitMobileXamarin.Models.Login;
using CallofitMobileXamarin.Models.Requests;
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
    public partial class ChamadosPage : ContentPage
    {
        ChamadoDTO _chamado;
        public ChamadosPage(ChamadoDTO chamado)
        {
            InitializeComponent();
            Initialize(chamado);
            _chamado = chamado;
        }

        private async void Initialize(ChamadoDTO chamado)
        {
            ChamadosService chamadosService = new ChamadosService();
            LoginService loginService = new LoginService();
            SistemasSuportadosService sistemasSuportadosService = new SistemasSuportadosService();

            try
            {
                loading.IsVisible = true;
                var responseStatus = await chamadosService.RecuperarStatusChamadosAsync();
                if (responseStatus.IsSuccessStatusCode)
                {
                    var responseStatusContent = await responseStatus.Content.ReadAsStringAsync();
                    var listStatusChamados = JsonConvert.DeserializeObject<List<StatusChamadosDTO>>(responseStatusContent);
                    if (listStatusChamados != null)
                    {
                        StatusChamadoInput.ItemsSource = listStatusChamados;
                        StatusChamadoInput.ItemDisplayBinding = new Binding("descricao");
                        StatusChamadoInput.FontAttributes = FontAttributes.Bold;
                        StatusChamadoInput.TextTransform = TextTransform.Uppercase;
                        StatusChamadoInput.TextColor = Color.White;

                        StatusChamadoInput.SelectedItem = listStatusChamados.FirstOrDefault(o => o.id == chamado.status_chamado_id); //PADRÃO EM ABERTO
                        if(chamado.status_chamado_id == 1)
                        {
                            labelDescricaoSolucaoInput.IsVisible = false;
                            descricaoSolucaoInput.IsVisible = false;    
                            titleChamado.Text = $"Chamado Em Aberto: #{chamado.id}";
                            titleChamado.FontAttributes = FontAttributes.Bold;
                            titleChamado.FontSize = 26;
                            titleChamado.TextColor = Color.Black;
                            StatusChamadoInput.Background = Color.Blue;
                            gridButtonsEmAberto.IsVisible = true;
                            gridButtons.IsVisible = false;
                        }
                        if (chamado.status_chamado_id == 2)
                        {
                            labelDescricaoSolucaoInput.IsVisible = true;
                            descricaoSolucaoInput.IsVisible = true;
                            titleChamado.Text = $"Chamado Pendente: #{chamado.id}";
                            titleChamado.FontAttributes = FontAttributes.Bold;
                            titleChamado.FontSize = 26;
                            titleChamado.TextColor = Color.Black;
                            StatusChamadoInput.Background = Color.Yellow;
                            gridButtonsEmAberto.IsVisible = false;
                            gridButtons.IsVisible = true;
                        }
                        if (chamado.status_chamado_id == 3)
                        {
                            labelDescricaoSolucaoInput.IsVisible = true;
                            descricaoSolucaoInput.IsVisible = true;
                            titleChamado.Text = $"Chamado Finalizado: #{chamado.id}";
                            titleChamado.FontAttributes = FontAttributes.Bold;
                            titleChamado.FontSize = 26;
                            titleChamado.TextColor = Color.Black;
                            StatusChamadoInput.Background = Color.LightGreen;
                            gridButtonsEmAberto.IsVisible = false;
                            gridButtons.IsVisible = true;
                        }
                        if (chamado.status_chamado_id == 4)
                        {
                            labelDescricaoSolucaoInput.IsVisible = true;
                            descricaoSolucaoInput.IsVisible = true;
                            titleChamado.Text = $"Chamado Atrasado: #{chamado.id}";
                            titleChamado.FontAttributes = FontAttributes.Bold;
                            titleChamado.FontSize = 26;
                            titleChamado.TextColor = Color.Black;
                            StatusChamadoInput.Background = Color.Red;
                            gridButtonsEmAberto.IsVisible = false;
                            gridButtons.IsVisible = true;
                        }
                    }
                }
                else
                {
                    loading.IsVisible = false;
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
                        OpcoesSistemaSuportadoInput.SelectedItem = listSistemasSuportados.FirstOrDefault(o => o.id == chamado.sistema_suportado_id);
                    }
                }
                else
                {
                    loading.IsVisible = false;
                    var errorTratado = await ErrorsHandler.TratarMenssagemErro(responseSistemas);
                    await DisplayAlert(errorTratado.status.ToString(), errorTratado.errors, "OK");
                }

                var responseTecnicoResponsavel = await loginService.RecuperarDadosUsuarioPorIdAsync(new RequestUsuarioPorId(){ id = chamado.tecnico_usuario_id });
                if (responseSistemas.IsSuccessStatusCode)
                {
                    var responseTecnicoResponsavelContent = await responseTecnicoResponsavel.Content.ReadAsStringAsync();
                    var userTecnico = JsonConvert.DeserializeObject<UsuarioDTO>(responseTecnicoResponsavelContent);
                    tecnicoResponsavelInput.Text = (userTecnico != null || userTecnico.id != 0 ? userTecnico.nome : "Não Definido");
                }
                else
                {
                    loading.IsVisible = false;
                    var errorTratado = await ErrorsHandler.TratarMenssagemErro(responseTecnicoResponsavel);
                    await DisplayAlert(errorTratado.status.ToString(), errorTratado.errors, "OK");
                }

                solicitanteInput.Text = !String.IsNullOrEmpty(chamado.solicitante) ? chamado.solicitante : "";

                dataLimiteInput.Date = chamado.data_limite;
                descricaoProblemaInput.Text = !String.IsNullOrEmpty(chamado.descricao_problema) ? chamado.descricao_problema : "";
                descricaoSolucaoInput.Text = !String.IsNullOrEmpty(chamado.descricao_solucao) ? chamado.descricao_solucao : "";
                loading.IsVisible = false;
            }
            catch (Exception ex)
            {
                loading.IsVisible = false;
                await DisplayAlert("Erro 500", ex.Message, "OK");
            }
        }

        // Método para quando fechar a página e direcionar uma ação a página anterior da pilha
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send<object>(this, "PáginaRemovida");
        }

        private async void VoltarClick(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void DeletarClick(object sender, EventArgs e)
        {
            ChamadosService chamadosService = new ChamadosService();
            bool result = await DisplayAlert("Confirmação", $"Você tem certeza que deseja excluir o chamado #{_chamado.id} ?", "Sim", "Não");

            if (result)
            {
                loading.IsVisible = true;
                var responseDelete = await chamadosService.DeleteChamadosAsync(new DeleteChamadoRequest() { 
                    usuario_id = int.Parse(await SecureStorage.GetAsync("id")), 
                    chamado_id = _chamado.id});

                if (responseDelete.IsSuccessStatusCode)
                {
                    var responseDeleteContent = await responseDelete.Content.ReadAsStringAsync();
                    var listSistemasSuportados = JsonConvert.DeserializeObject(responseDeleteContent);
                    loading.IsVisible = false;
                    await DisplayAlert("SUCESSO!!", "Chamado deletado com sucesso!", "OK");
                    await Navigation.PopModalAsync();
                }
                else
                {
                    loading.IsVisible = false;
                    var errorTratado = await ErrorsHandler.TratarMenssagemErro(responseDelete);
                    await DisplayAlert(errorTratado.status.ToString(), errorTratado.errors, "OK");
                }
            }
        }

        private void OnPickerClicked(object sender, EventArgs e)
        {
            ((Picker)sender).Focus();
        }

    }
}