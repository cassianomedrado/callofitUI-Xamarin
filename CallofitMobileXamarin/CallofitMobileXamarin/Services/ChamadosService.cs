﻿using CallofitMobileXamarin.Utils;
using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CallofitMobileXamarin.Models.Chamados;

namespace CallofitMobileXamarin.Services
{
    public class ChamadosService
    {
        public async Task<HttpResponseMessage> RecuperarDadosUsuarioAsync(string id)
        {
            var response = new HttpResponseMessage();
            var token = await AuthToken.GetTokenAsync();
            var requestTotaisChamados = new RequestTotaisChamados()
            {
                usuario_id = int.Parse(id)
            };

            try
            {
                var handler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                };
                var httpClient = new HttpClient(handler);
                var content = new StringContent(JsonConvert.SerializeObject(requestTotaisChamados), Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(HttpMethod.Post, Configuration.ApiUrl + "/Chamado/totais");
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