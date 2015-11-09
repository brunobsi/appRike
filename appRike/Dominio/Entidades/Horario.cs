using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class Horario : Identificador
    {
        public string Dia { get; set; }
        public string HoraInicial { get; set; }
        public string HoraFinal { get; set; }
        public int Ordem { get; set; }
    }
}
