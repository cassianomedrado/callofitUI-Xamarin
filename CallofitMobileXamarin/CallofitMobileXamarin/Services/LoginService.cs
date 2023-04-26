using CallofitMobileXamarin.Models.Login;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System;
using CallofitMobileXamarin.Models.Requests;
using System.Net.Http.Headers;
using CallofitMobileXamarin.Utils;
using CallofitMobileXamarin.Models.Usuario;
using CallofitMobileXamarin.Models.Chamados;

namespace CallofitMobileXamarin.Services
{
    class LoginService
    {
        public async Task<HttpResponseMessage> LoginAsync(string username, string senha)
        {
            var response = new HttpResponseMessage();

            var loginModel = new LoginRequest
            {
                username = username,
                senha = senha
            };
            try
            {
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
                throw new ArgumentException(e.Message);
            }

            return response;
        }

        public async Task<HttpResponseMessage> RecuperarDadosUsuarioAsync(string username)
        {
            var response = new HttpResponseMessage();
            var token = await AuthToken.GetTokenAsync();
            var recuperarDadosRequest = new RecuperarDadosRequest()
            {
                username = username
            };
            try
            {
                var handler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                };
                var httpClient = new HttpClient(handler);
                var content = new StringContent(JsonConvert.SerializeObject(recuperarDadosRequest), Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(HttpMethod.Post, Configuration.ApiUrl + "/Usuario");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                request.Content = content;
                response = await httpClient.SendAsync(request);

            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }

            return response;
        }

        public async Task<HttpResponseMessage> RecuperarDadosUsuarioPorIdAsync(RequestUsuarioPorId userID)
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
                var content = new StringContent(JsonConvert.SerializeObject(userID), Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(HttpMethod.Post, Configuration.ApiUrl + "/Usuario/Usuario-por-id");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                request.Content = content;
                response = await httpClient.SendAsync(request);

            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }

            return response;
        }

        public async Task<HttpResponseMessage> AlterarSenhaAsync(RequestAlterarSenhaUsuario alterarSenha)
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
                var content = new StringContent(JsonConvert.SerializeObject(alterarSenha), Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), Configuration.ApiUrl + "/Usuario/alterarSenha");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                request.Content = content;
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
