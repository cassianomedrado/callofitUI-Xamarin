using CallofitMobileXamarin.Models.Login;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System;

namespace CallofitMobileXamarin.Services
{
    class LoginService
    {
        public async Task<HttpResponseMessage> LoginAsync(string username, string senha)
        {
            var response = new HttpResponseMessage();

            var loginModel = new LoginModel
            {
                username = username,
                senha = senha
            };
            try
            {
                var api = Configuration.ApiUrl;
                var handler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                };
                var httpClient = new HttpClient(handler);
                var content = new StringContent(JsonConvert.SerializeObject(loginModel), Encoding.UTF8, "application/json");
                response = await httpClient.PostAsync(Configuration.ApiUrl + "/Usuario/login", content);
            }
            catch (Exception e)
            {
                throw new ArgumentException("teste");
            }

            return response;
        }

    }
}
