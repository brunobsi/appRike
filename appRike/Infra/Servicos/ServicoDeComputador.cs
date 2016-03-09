using Dominio.Entidades;
using Infra.Contexto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Infra.Servicos
{
    public class ServicoDeComputador
    {
        private readonly DbContexto _db;

        public ServicoDeComputador()
        {
            _db = new DbContexto();
        }

        public bool Adicionar(Computador computador)
        {
            _db.Computadores.Add(computador);
            return _db.SaveChanges() > 0;
        }

        public bool Alterar(Computador computador)
        {
            _db.Entry(computador).State = EntityState.Modified;
            return _db.SaveChanges() > 0;
        }

        public bool Excluir(int computadorId)
        {
            _db.Computadores.Remove(_db.Computadores.Find(computadorId));
            return _db.SaveChanges() > 0;
        }

        public Computador GetById(int id)
        {
            return _db.Set<Computador>().Find(id);
        }

        public List<Computador> Get(Func<Computador, bool> filtro, params string[] predicate)
        {
            return GetAll(predicate).Where(filtro).ToList();
        }

        public List<Computador> GetAll(params string[] predicate)
        {
            IQueryable<Computador> query = _db.Set<Computador>();
            query = predicate.Aggregate(query, (current, item) => current.Include(item));
            return query.OrderBy(x => x.Descricao).ToList();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
