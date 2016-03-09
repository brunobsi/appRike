using Dominio.Entidades;
using Infra.Contexto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Infra.Servicos
{
    public class ServicoDeAluno
    {
        private readonly DbContexto _db;

        public ServicoDeAluno()
        {
            _db = new DbContexto();
        }

        public bool Adicionar(Aluno aluno)
        {
            _db.Alunos.Add(aluno);
            return _db.SaveChanges() > 0;
        }

        public bool Alterar(Aluno aluno)
        {
            _db.Entry(aluno).State = EntityState.Modified;
            return _db.SaveChanges() > 0;
        }

        public bool Excluir(int alunoId)
        {
            _db.Alunos.Remove(_db.Alunos.Find(alunoId));
            return _db.SaveChanges() > 0;
        }

        public Aluno GetById(int id)
        {
            return _db.Set<Aluno>().Find(id);
        }

        public List<Aluno> Get(Func<Aluno, bool> filtro, params string[] predicate)
        {
            return GetAll(predicate).Where(filtro).ToList();
        }

        public List<Aluno> GetAll(params string[] predicate)
        {
            IQueryable<Aluno> query = _db.Set<Aluno>();
            query = predicate.Aggregate(query, (current, item) => current.Include(item));

            return query.OrderBy(x => x.Nome).ToList();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
