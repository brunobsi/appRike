using System;
using System.ComponentModel;

namespace BNR_ComputerClass.Models
{
    public class HorarioModel : IdentificadorModel
    {
        public string Dia { get; set; }
        [DisplayName("Inicio")]
        public string HoraInicial { get; set; }
        [DisplayName("Fim")]
        public string HoraFinal { get; set; }
        public int Ordem { get; set; }
        public string HorarioSelect { get; set; }
    }
}