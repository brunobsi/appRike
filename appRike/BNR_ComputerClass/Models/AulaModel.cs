using System;
using System.Collections.Generic;
using System.ComponentModel;
using Dominio.Entidades;

namespace BNR_ComputerClass.Models
{
    public class AulaModel : IdentificadorModel
    {
        private DateTime _dataAula;
        public DateTime DataAula
        {
            get { return _dataAula; }
            set
            {
                _dataAula = value;
                Data = value.ToShortDateString();
            }
        }

        [DisplayName("Data da Aula: ")]
        public string Data { get; set; }

        [DisplayName("Alunos Presentes: ")]
        public int AlunosPresentes { get; set; }

        private List<ChamadaModel> _chamadasModel;
        public List<ChamadaModel> ChamadasModel
        {
            get { return _chamadasModel; }
            set
            {
                _chamadasModel = value;

                foreach (var item in value)
                {
                    AgendasId += item.AgendaId + ",";
                }

                AgendasId = AgendasId != null ? AgendasId.Substring(0, AgendasId.Length - 1) : null;
            }
        }

        public string AgendasId { get; set; }
    }
}