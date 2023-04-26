using System;
using System.Collections.Generic;
using System.Text;

namespace CallofitMobileXamarin.Models.Login
{
    public class RequestAlterarSenhaUsuario
    {
        public string username { get; set; }
        public string email { get; set; }
        public string senhaAtual { get; set; }
        public string senhaNova { get; set; }
        public string confirmaNovaSenha { get; set; }
    }
}
