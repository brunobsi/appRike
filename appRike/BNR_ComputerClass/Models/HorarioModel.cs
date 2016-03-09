﻿using System.ComponentModel;

namespace BNR_ComputerClass.Models
{
    public class HorarioModel : IdentificadorModel
    {
        public string Dia { get; set; }
        [DisplayName("Hora Inicial")]
        public string HoraInicial { get; set; }
        [DisplayName("Hora Final")]
        public string HoraFinal { get; set; }
        public int Ordem { get; set; }
    }
}