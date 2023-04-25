using System;
using System.Collections.Generic;
using System.Text;

namespace CallofitMobileXamarin.Models.Chamados
{
    public class DeleteChamadoRequest
    {
        public int usuario_id { get; set; }
        public int chamado_id { get; set; }
    }
}
