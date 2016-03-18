using Dominio.Entidades;
using Dominio.Tools;
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
        private readonly DbContexto _db;

        public ServicoDeHorario()
        {
            _db = new DbContexto();
        }

        public void Adicionar(Horario horario)
        {
            if (VerificaExistente(horario)) return;
            VerificaOrdem(horario);
            _db.Horarios.Add(horario);
            _db.SaveChanges();
        }

        public bool Alterar(Horario horario)
        {
            VerificaOrdem(horario);
            _db.Entry(horario).State = EntityState.Modified;
            return _db.SaveChanges() > 0;
        }

        public bool Excluir(int horarioId)
        {
            _db.Horarios.Remove(_db.Horarios.Find(horarioId));
            return _db.SaveChanges() > 0;
        }

        public Horario GetById(int id)
        {
            return _db.Set<Horario>().Find(id);
        }

        public List<Horario> Get(Func<Horario, bool> filtro, params string[] predicate)
        {
            return GetAll(predicate).Where(filtro).ToList();
        }

        public List<Horario> GetAll(params string[] predicate)
        {
            IQueryable<Horario> query = _db.Set<Horario>();
            query = predicate.Aggregate(query, (current, item) => current.Include(item));
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
            var horarioBanco = Get(x => x.Dia.Equals(horario.Dia) &&
                                        x.HoraInicial.Equals(horario.HoraInicial) &&
                                        x.HoraFinal.Equals(horario.HoraFinal));
            var first = horarioBanco.FirstOrDefault();
            if (first == null) return false;
            horario.Id = first.Id;
            return true;
        }



        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
