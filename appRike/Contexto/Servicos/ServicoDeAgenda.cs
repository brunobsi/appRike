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
    public class ServicoDeAgenda
    {
        private List<DateTime> HorariosFixos;
        private DbContexto Db;

        public ServicoDeAgenda()
        {
            Db = new DbContexto();
            HorariosFixos = new List<DateTime>();

            HorariosFixos.Add(DateTime.ParseExact("08:00", "H:m", null));
            HorariosFixos.Add(DateTime.ParseExact("09:00", "H:m", null));
            HorariosFixos.Add(DateTime.ParseExact("10:00", "H:m", null));
            HorariosFixos.Add(DateTime.ParseExact("14:00", "H:m", null));
            HorariosFixos.Add(DateTime.ParseExact("15:00", "H:m", null));
            HorariosFixos.Add(DateTime.ParseExact("16:00", "H:m", null));
        }

        public bool Adicionar(Agenda agenda)
        {
            Db.Agendas.Add(agenda);
            return Db.SaveChanges() > 0;
        }

        public bool Excluir(Agenda agenda)
        {
            Db.Agendas.Remove(agenda);
            return Db.SaveChanges() > 0;
        }

        public Agenda GetById(int id)
        {
            return Db.Set<Agenda>().Find(id);
        }

        public List<Agenda> Get(Func<Agenda, bool> filtro, params string[] predicate)
        {
            return GetAll(predicate).Where(filtro).ToList();
        }

        public List<Agenda> GetAll(params string[] predicate)
        {
            IQueryable<Agenda> query = Db.Set<Agenda>();

            query.Include("Horario");

            foreach (var item in predicate)
            {
                query = query.Include(item);
            }

            return query.OrderBy(x => x.Horario.Ordem).ToList();
        }

        public bool VerificarSePodeAgendar(Agenda agenda)
        {
            var result = !Get(x => x.HorarioId.Equals(agenda.HorarioId)
                          && x.ComputadorId.Equals(agenda.ComputadorId)).Any();

            if (result)
            {
                var horario = Db.Horarios.First(x => x.Id.Equals(agenda.HorarioId));
                var datas = Get(x => x.ComputadorId.Equals(agenda.ComputadorId) &&
                                     x.Horario.Dia.Equals(Converter.RemoverAcentos(horario.Dia)), "Horario");

                foreach (var item in datas)
                {
                    var HoraBanco = DateTime.ParseExact(item.Horario.HoraInicial, "H:m", null).Hour;
                    var HoraBanco2 = DateTime.ParseExact(item.Horario.HoraFinal, "H:m", null).Hour;
                    var IntervaloBanco = HoraBanco2 - HoraBanco;

                    for (int i = 0; i < IntervaloBanco && result; i++)
                    {
                        var HoraAgenda = DateTime.ParseExact(horario.HoraInicial, "H:m", null).Hour;
                        var HoraAgenda2 = DateTime.ParseExact(horario.HoraFinal, "H:m", null).Hour;
                        var IntervaloAgenda = HoraAgenda2 - HoraAgenda;

                        for (int j = 0; j < IntervaloAgenda && result; j++)
                        {
                            if (HoraBanco.Equals(HoraAgenda))
                            {
                                result = false;
                            }

                            HoraAgenda++;
                        }

                        HoraBanco++;
                    }

                    if (!result)
                        break;
                }
            }
            return result;
        }

        public void Dispose()
        {
            Db.Dispose();
        }  
    }
}
