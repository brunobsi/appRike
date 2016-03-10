
using System.ComponentModel;

namespace BNR_ComputerClass.Models
{
    public class ChamadaModel : IdentificadorModel
    {
        [DisplayName("Presença")]
        public bool Presenca { get; set; }
        public int AulaId { get; set; }
        public int AgendaId { get; set; }
        public AulaModel Aula { get; set; }
        public AgendaModel Agenda { get; set; }
    }
}