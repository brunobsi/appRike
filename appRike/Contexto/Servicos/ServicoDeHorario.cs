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
        private DbContexto Db;

        public ServicoDeHorario()
        {
            Db = new DbContexto();
        }

        public bool Adicionar(Horario horario)
        {
            if (!VerificaExistente(horario))
            {
                VerificaOrdem(horario);
                Db.Horarios.Add(horario);
                return Db.SaveChanges() > 0;
            }

            return false;
        }

        public bool Alterar(Horario horario)
        {
            VerificaOrdem(horario);
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

            return query.OrderBy(x => x.Ordem).ToList(); ;
        }

        public void VerificaOrdem(Horario horario)
        {
            switch (horario.Dia)
            {
                case "Segunda": horario.Ordem = 1; break;
                case "Terça": horario.Ordem = 2; break;
                case "Quarta": horario.Ordem = 3; break;
                case "Quinta": horario.Ordem = 4; break;
                case "Sexta": horario.Ordem = 5; break;
            }
        }
        public bool VerificaExistente(Horario horario)
        {
            var horarioBanco = Get(x => x.Dia.Equals(horario.Dia) && x.HoraInicial.Equals(horario.HoraInicial) && x.HoraFinal.Equals(horario.HoraFinal));
            return horarioBanco.Any();
        }

        public void Dispose()
        {
            Db.Dispose();
        }
    }
}
