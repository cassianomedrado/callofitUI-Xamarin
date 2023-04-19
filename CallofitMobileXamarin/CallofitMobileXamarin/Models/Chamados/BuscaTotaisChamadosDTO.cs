using System;
using System.Collections.Generic;
using System.Text;

namespace CallofitMobileXamarin.Models.Chamados
{
    public class BuscaTotaisChamadosDTO
    {
        public int chamadosEmAberto { get; set; }
        public int chamadosPendentes { get; set; }
        public int chamadosFinalizados { get; set; }
        public int chamadosAtrasados { get; set; }
    }
}