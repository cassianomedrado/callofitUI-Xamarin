using CallofitMobileXamarin.Utils;
using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CallofitMobileXamarin.Models.Chamados;

namespace CallofitMobileXamarin.Services
{
    public class SistemasSuportadosService
    {  
        public async Task<HttpResponseMessage> RecuperarSistemasSuportadosAsync()
        {
            var response = new HttpResponseMessage();
            var token = await AuthToken.GetTokenAsync();

            try
            {
                var handler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                };
                var httpClient = new HttpClient(handler);
                var request = new HttpRequestMessage(HttpMethod.Get, Configuration.ApiUrl + "/SistemaSuportado");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await httpClient.SendAsync(request);
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }

            return response;
        }
    }
}
