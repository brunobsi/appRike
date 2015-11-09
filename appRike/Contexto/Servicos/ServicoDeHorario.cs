using Dominio.Entidades;
using Infra.Contexto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Servicos
{
    public class ServicoDeHorario
    {
        private DbContexto Db = new DbContexto();

        public bool Adicionar(Horario horario)
        {
            Db.Horarios.Add(horario);
            return Db.SaveChanges() > 0;
        }

        public bool Alterar(Horario horario)
        {
            Db.Entry(horario).State = EntityState.Modified;
            return Db.SaveChanges() > 0;
        }

        public bool Excluir(Horario horario)
        {
            Db.Horarios.Remove(horario);
            return Db.SaveChanges() > 0;
        }

        public Horario GetById(int id)
        {
            return Db.Set<Horario>().Find(id);
        }

        public List<Horario> Get(Func<Horario, bool> filtro, params string[] predicate)
        {
            return GetAll(predicate).Where(filtro).ToList();
        }

        public List<Horario> GetAll(params string[] predicate)
        {
            IQueryable<Horario> query = Db.Set<Horario>();

            foreach (var item in predicate)
            {
                query = query.Include(item);
            }

            return query.ToList();
        }
    }
}
