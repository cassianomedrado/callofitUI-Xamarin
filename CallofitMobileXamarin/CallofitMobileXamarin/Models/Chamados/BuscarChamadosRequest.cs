using System;
using System.Collections.Generic;
using System.Text;

namespace CallofitMobileXamarin.Models.Chamados
{
    public class BuscarChamadosRequest
    {
        public int  usuario_id { get; set; }
        public int status_chamado_id { get; set; }
        public int tecnico_usuario_id { get; set; }
    }
}
