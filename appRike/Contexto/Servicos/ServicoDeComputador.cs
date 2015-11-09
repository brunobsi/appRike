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
    public class ServicoDeComputador
    {
        private DbContexto Db = new DbContexto();

        public bool Adicionar(Computador computador)
        {
            Db.Computadores.Add(computador);
            return Db.SaveChanges() > 0;
        }

        public bool Alterar(Computador computador)
        {
            Db.Entry(computador).State = EntityState.Modified;
            return Db.SaveChanges() > 0;
        }

        public bool Excluir(Computador computador)
        {
            Db.Computadores.Remove(computador);
            return Db.SaveChanges() > 0;
        }

        public Computador GetById(int id)
        {
            return Db.Set<Computador>().Find(id);
        }

        public List<Computador> Get(Func<Computador, bool> filtro, params string[] predicate)
        {
            return GetAll(predicate).Where(filtro).ToList();
        }

        public List<Computador> GetAll(params string[] predicate)
        {
            IQueryable<Computador> query = Db.Set<Computador>();

            foreach (var item in predicate)
            {
                query = query.Include(item);
            }

            return query.OrderBy(x => x.Descricao).ToList();
        }
    }
}
