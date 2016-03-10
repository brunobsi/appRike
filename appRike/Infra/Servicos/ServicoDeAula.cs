using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dominio.Entidades;
using Infra.Contexto;

namespace Infra.Servicos
{
    public class ServicoDeAula
    {
        private readonly DbContexto _db;

        public ServicoDeAula()
        {
            _db = new DbContexto();
        }

        public bool Adicionar(Aula aula)
        {
            _db.Aulas.Add(aula);
            return _db.SaveChanges() > 0;
        }

        public bool Alterar(Aula aula)
        {
            _db.Entry(aula).State = EntityState.Modified;
            return _db.SaveChanges() > 0;
        }

        public bool Excluir(int aulaId)
        {
            var chamadas = _db.Chamadas.Where(x => x.AulaId == aulaId);
            _db.Chamadas.RemoveRange(chamadas);
            _db.Aulas.Remove(_db.Aulas.Find(aulaId));
            return _db.SaveChanges() > 0;
        }

        public Aula GetById(int id)
        {
            return _db.Set<Aula>().Find(id);
        }

        public List<Aula> Get(Func<Aula, bool> filtro, params string[] predicate)
        {
            return GetAll(predicate).Where(filtro).ToList();
        }

        public List<Aula> GetAll(params string[] predicate)
        {
            IQueryable<Aula> query = _db.Set<Aula>();
            query = predicate.Aggregate(query, (current, item) => current.Include(item));
            return query.OrderBy(x => x.DataAula).ToList();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
