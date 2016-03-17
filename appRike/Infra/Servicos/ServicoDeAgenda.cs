using Dominio.Entidades;
using Dominio.Tools;
using Infra.Contexto;
using Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Infra.Servicos
{
    public class ServicoDeAgenda : IServicoDeAgenda
    {
        private readonly DbContexto _db = new DbContexto();

        public bool Adicionar(Agenda agenda)
        {
            agenda.Horario = null; 
            agenda.Computador = null; 
            agenda.Aluno = null;
            _db.Agendas.Add(agenda);
            return _db.SaveChanges() > 0;
        }

        public bool Excluir(Agenda agenda)
        {
            _db.Agendas.Remove(agenda);
            return _db.SaveChanges() > 0;
        }

        public Agenda GetById(int id)
        {
            return _db.Set<Agenda>().Find(id);
        }

        public List<Agenda> Get(Func<Agenda, bool> filtro, params string[] predicate)
        {
            return GetAll(predicate).Where(filtro).ToList();
        }

        public List<Agenda> GetAll(params string[] predicate)
        {
            IQueryable<Agenda> query = _db.Set<Agenda>();
            query.Include("Horario");
            query.Include("Computador");
            query = predicate.Aggregate(query, (current, item) => current.Include(item));
            return query.OrderBy(x => x.Horario.Ordem)
                        .ThenBy(x => x.Horario.HoraInicial)
                        .ThenBy(x => x.Computador.Descricao)
                        .ToList();
        }

        public bool VerificarSePodeAgendar(Agenda agenda)
        {
            var result = !Get(x => x.HorarioId.Equals(agenda.HorarioId)
                          && x.ComputadorId.Equals(agenda.ComputadorId)).Any();

            if (!result) return false;
            var horario = _db.Horarios.First(x => x.Id.Equals(agenda.HorarioId));
            var datas = Get(x => x.ComputadorId.Equals(agenda.ComputadorId) &&
                                 x.Horario.Dia.Equals(Converter.RemoverAcentos(horario.Dia)), "Horario");

            foreach (var item in datas)
            {
                var horaBanco = DateTime.ParseExact(item.Horario.HoraInicial, "H:m", null).Hour;
                var horaBanco2 = DateTime.ParseExact(item.Horario.HoraFinal, "H:m", null).Hour;
                var intervaloBanco = horaBanco2 - horaBanco;

                for (var i = 0; i < intervaloBanco && result; i++)
                {
                    var horaAgenda = DateTime.ParseExact(horario.HoraInicial, "H:m", null).Hour;
                    var horaAgenda2 = DateTime.ParseExact(horario.HoraFinal, "H:m", null).Hour;
                    var intervaloAgenda = horaAgenda2 - horaAgenda;

                    for (var j = 0; j < intervaloAgenda && result; j++)
                    {
                        if (horaBanco.Equals(horaAgenda))
                        {
                            result = false;
                        }
                        horaAgenda++;
                    }
                    horaBanco++;
                }

                if (!result)
                    break;
            }
            return result;
        }

        public void Dispose()
        {
            _db.Dispose();
        }  
    }
}
