using System;
using System.Collections.Generic;
using System.Text;

namespace CallofitMobileXamarin.Models.Requests
{
    public class UsuarioDTO
    {
        public int id { get; set; }
        public DateTime data_criacao { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public int tipo_usuario_id { get; set; }
        public string username { get; set; }
        public bool status { get; set; }
    }
}
