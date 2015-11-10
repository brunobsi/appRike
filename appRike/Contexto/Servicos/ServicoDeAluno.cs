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
    public class ServicoDeAluno
    {
        private DbContexto Db;

        public ServicoDeAluno()
        {
            Db = new DbContexto();
        }

        public bool Adicionar(Aluno aluno)
        {
            Db.Alunos.Add(aluno);
            return Db.SaveChanges() > 0;
        }

        public bool Alterar(Aluno aluno)
        {
            Db.Entry(aluno).State = EntityState.Modified;
            return Db.SaveChanges() > 0;
        }

        public bool Excluir(Aluno aluno)
        {
            Db.Alunos.Remove(aluno);
            return Db.SaveChanges() > 0;
        }

        public Aluno GetById(int id)
        {
            return Db.Set<Aluno>().Find(id);
        }

        public List<Aluno> Get(Func<Aluno, bool> filtro, params string[] predicate)
        {
            return GetAll(predicate).Where(filtro).ToList();
        }

        public List<Aluno> GetAll(params string[] predicate)
        {
            IQueryable<Aluno> query = Db.Set<Aluno>();

            foreach (var item in predicate)
            {
                query = query.Include(item);
            }

            return query.OrderBy(x => x.Nome).ToList();
        }

        public void Dispose()
        {
            Db.Dispose();
        }
    }
}
