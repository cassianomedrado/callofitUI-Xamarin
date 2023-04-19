using CallofitMobileXamarin.Models.RequestsErrors;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CallofitMobileXamarin.Utils
{
    public static class ErrorsHandler
    {
        public static async Task<RequestErrorsdDTO> TratarMenssagemErro(HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            if (String.IsNullOrEmpty(responseContent))
            {
                return new RequestErrorsdDTO() { status = (int)response.StatusCode, errors = response.ReasonPhrase };
            }

            var jObject = JObject.Parse(responseContent);
            var menssagesError = new StringBuilder("");
            int status = 0;
            if (responseContent.Contains("errors"))
            {
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
            else if(responseContent.Contains("error"))
            {
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

            return new RequestErrorsdDTO() { status = status, errors = menssagesError.ToString()};
        }
    }
}
