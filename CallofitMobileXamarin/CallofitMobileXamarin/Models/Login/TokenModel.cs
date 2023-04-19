using System;
using System.Collections.Generic;
using System.Text;

namespace CallofitMobileXamarin.Models.Login
{
    class TokenModel
    {
        public int status { get; set; }
        public string Token { get; set; }
        public int expires_in { get; set; } = 21600; // 6 horas de tempo 
        public DateTime ExpirationTime { get; set; } = DateTime.Now.AddSeconds(21600); // tempo que expira o token
    }
}
