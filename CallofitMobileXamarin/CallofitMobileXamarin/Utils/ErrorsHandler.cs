using CallofitMobileXamarin.Models.Requests;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CallofitMobileXamarin.Utils
{
    public static class ErrorsHandler
    {
        public static async Task<RequestErrorsModel> TratarMenssagemErro(HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(responseContent);
            var menssagesError = new StringBuilder("");
            int status = 0;
            if (responseContent.Contains("errors"))
            {
                //loading.IsVisible = false;
                // Acessa o dicionário de erros dinamicamente
                var errors = jObject["errors"].ToObject<Dictionary<string, IList<string>>>();
                status = (int)jObject["status"];

                if (errors.Count > 0)
                {
                    foreach (var item in errors)
                    {
                        string fieldName = item.Key;
                        IList<string> fieldErrors = item.Value;
                        menssagesError.AppendLine(string.Join(", ", fieldErrors));
                    }
                }
            }
            else
            {
                //loading.IsVisible = false;
                // Acessa o dicionário de erros dinamicamente
                var errors = jObject["error"];
                status = (int)jObject["status"];
                if (errors.Count() > 0)
                {
                    foreach (var item in errors)
                    {
                        menssagesError.AppendLine(item["mensagem"].ToString());
                    }
                }
            }

            return new RequestErrorsModel() { status = status, errors = menssagesError.ToString()};
        }
    }
}
