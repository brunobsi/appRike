
namespace Dominio.Entidades
{
    public class Agenda : Identificador
    {
        public int AlunoId { get; set; }
        public Aluno Aluno { get; set; }
        public int ComputadorId { get; set; }
        public Computador Computador { get; set; }
        public int HorarioId { get; set; }
        public Horario Horario { get; set; }
        public string Observacao { get; set; }
    }
}
