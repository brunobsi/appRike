
namespace BNR_ComputerClass.Models
{
    public class AgendaModel : IdentificadorModel
    {
        public int AlunoId { get; set; }
        public AlunoModel Aluno { get; set; }
        public int ComputadorId { get; set; }
        public ComputadorModel Computador { get; set; }
        public int HorarioId { get; set; }
        public HorarioModel Horario { get; set; }
        public string Observacao { get; set; }
    }
}