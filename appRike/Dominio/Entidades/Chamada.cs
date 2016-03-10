

namespace Dominio.Entidades
{
    public class Chamada : Identificador
    {
        public bool Presenca { get; set; }
        public int AulaId { get; set; }
        public int AgendaId { get; set; }
        public Aula Aula { get; set; }
        public Agenda Agenda { get; set; }
    }
}
