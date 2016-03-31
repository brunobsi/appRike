using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BNR_ComputerClass.Models;
using Dominio.Entidades;
using Dominio.Tools;
using Infra.Servicos;

namespace BNR_ComputerClass.Controllers
{
    public class HorarioController : Controller
    {
        private readonly ServicoDeHorario _servicoDeHorario = new ServicoDeHorario();
        private readonly ServicoDeComputador _servicoDeComputador = new ServicoDeComputador();
        private readonly ServicoDeAluno _servicoDeAluno = new ServicoDeAluno();
        private readonly ServicoDeAgenda _servicoDeAgenda = new ServicoDeAgenda();
        private readonly List<string> _listDias, _listHorariosIni, _listHorariosFimManha, _listHorariosFimTarde;

        public HorarioController()
        {
            _listDias = new List<string>
            {
               "Segunda", "Terça", "Quarta", "Quinta", "Sexta"
            };

            _listHorariosIni = new List<string>
            {
                "08:00", "09:00","10:00","14:00","15:00","16:00",
            };

            _listHorariosFimManha = new List<string>
            {
                "09:00", "10:00","11:00",
            };

            _listHorariosFimTarde = new List<string>
            {
                "15:00","16:00","17:00",
            };
        }

        public ActionResult Index()
        {
            var horarioId = RetornaHorarioIndex();
            var list = Mapper.Map<List<AgendaModel>>(_servicoDeAgenda.Get(x => x.HorarioId == horarioId, "Horario", "Aluno", "Computador"));
            var listAgendaModel = MontaListaComputadores(list);
            return View(listAgendaModel);
        }

        public ActionResult Create()
        {
            InitSelectsCreate();
            return View();
        }

        [HttpPost]
        public ActionResult Create(AgendaModel model)
        {
            try
            {
                var agenda = Mapper.Map<Agenda>(model);
                var list = _servicoDeHorario.Adicionar(agenda.Horario);
                var erro = 0;

                foreach (var agendaAdd in list.Select(horario => new Agenda
                {
                    ComputadorId = agenda.ComputadorId,
                    AlunoId = agenda.AlunoId,
                    HorarioId = horario.Id
                }))
                {
                    if (_servicoDeAgenda.VerificarSePodeAgendar(agendaAdd))
                    {
                        _servicoDeAgenda.Adicionar(agendaAdd);
                    }
                    else
                    {
                        erro++;
                    }
                }
                if (erro < list.Count) return RedirectToAction("Index");
                InitSelectsCreate();
                ViewBag.Erro = "Esse computador já está agendado para este horário";
                return View();
            }
            catch
            {
                InitSelectsCreate();
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            var agenda = _servicoDeAgenda.Get(x => x.Id == id, "Horario", "Aluno", "Computador").First();
            return View(Mapper.Map<AgendaModel>(agenda));
        }

        [HttpPost]
        public ActionResult Delete(AgendaModel model)
        {
            try
            {
                var agenda = _servicoDeAgenda.GetById(model.Id);
                var result = _servicoDeAgenda.Excluir(agenda);
                if (!result) return RedirectToAction("Index");
                result = _servicoDeAgenda.Get(x => x.HorarioId == agenda.HorarioId).Any();
                if (!result)
                {
                    _servicoDeHorario.Excluir(agenda.HorarioId);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        [Route("SugerirHoraFinal/{horaInicial}"), HttpGet]
        public JsonResult SugerirHoraFinal(string horaInicial)
        {
            var hora = DateTime.ParseExact(horaInicial, "H:m", null).Hour;
            var listHorariosFim = hora < 11 ? _listHorariosFimManha : _listHorariosFimTarde;
            var list = (from t in listHorariosFim let horafim = DateTime.ParseExact(t, "H:m", null).Hour where horafim > hora select t).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [Route("AtualizaHorariosIndex/{dia}"), HttpGet]
        public ActionResult AtualizaHorariosIndex(string dia)
        {
            var list = _servicoDeAgenda.GetAll("Horario", "Aluno", "Computador");
            var horarioId = RetornaHorarioIndex(dia);
            var listModel = Mapper.Map<List<AgendaModel>>(list.Where(x => x.HorarioId == horarioId));
            var listAgendaModel = MontaListaComputadores(listModel);
            return PartialView("_Alunos", listAgendaModel);
        }

        [Route("AtualizaAgendasIndex/{horarioId:int}"), HttpGet]
        public ActionResult AtualizaAgendasIndex(int horarioId)
        {
            var list = _servicoDeAgenda.GetAll("Horario", "Aluno", "Computador");
            var listModel = Mapper.Map<List<AgendaModel>>(list.Where(x => x.HorarioId == horarioId));
            RetornaHorarioIndex(listModel.First().Horario.Dia, horarioId);
            var listAgendaModel = MontaListaComputadores(listModel);
            return PartialView("_Alunos", listAgendaModel);
        }

        private void InitSelectsCreate()
        {
            var dia = Converter.DiaIngParaPort(DateTime.Now.DayOfWeek);
            var hora = DateTime.Now.Hour;
            hora = hora < 8 ? 8 : hora > 10 && hora < 14 ? 14 : hora > 16 ? 16 : hora;
            var horaInicial = hora < 10 ? "0" + hora + ":00" : hora + ":00";
            ViewBag.Dias = new SelectList(_listDias, dia);
            ViewBag.HorariosIni = new SelectList(_listHorariosIni, horaInicial);
            ViewBag.HorariosFim = new SelectList(_listHorariosFimManha, 0);
            ViewBag.Computadores = new SelectList(_servicoDeComputador.GetAll(), "Id", "Descricao", 0);
            ViewBag.Alunos = new SelectList(_servicoDeAluno.GetAll(), "Id", "Nome", 0);
        }

        private int RetornaHorarioIndex(string dia = "", int horarioId = 0)
        {
            dia = string.IsNullOrEmpty(dia) ? Converter.DiaIngParaPort(DateTime.Now.DayOfWeek) : dia;
            var hora = DateTime.Now.Hour;
            hora = hora < 8 ? 8 : hora > 10 && hora < 14 ? 14 : hora > 16 ? 16 : hora;
            var horaInicial = hora < 10 ? "0" + hora + ":00" : hora + ":00";

            var horarios = _servicoDeAgenda.GetAll("Horario")
                                           .OrderBy(x => x.Horario.Ordem)
                                           .ThenBy(x => x.Horario.HoraInicial)
                                           .ThenBy(x => x.Horario.HoraFinal)
                                           .Select(x => x.Horario)
                                           .Distinct()
                                           .ToList();

            var listModel = Mapper.Map<List<HorarioModel>>(horarios);
            var listDias = listModel.Select(x => x.Dia).Distinct();
            listModel = listModel.Where(x => x.Dia.Equals(dia)).ToList();
            var listHorarios = listModel.Where(x => x.Dia.Equals(dia)).OrderBy(x => x.HoraInicial).Distinct();
            if (listModel.Count > 0)
            {
                var horario = listModel.FirstOrDefault(x => x.HoraInicial.Equals(horaInicial));
                while (horario == null)
                {
                    if (hora < 16)
                    {
                        hora++;
                        horaInicial = hora < 10 ? "0" + hora + ":00" : hora + ":00";
                        horario = listModel.FirstOrDefault(x => x.HoraInicial.Equals(horaInicial));
                    }
                    else
                    {
                        horario = listModel.FirstOrDefault();
                    }
                }

                horarioId = horario.Id;
            }

            TempData["Dias"] = new SelectList(listDias, dia);
            TempData["Horarios"] = new SelectList(listHorarios, "Id", "HorarioSelect", horarioId);
            return horarioId;
        }

        private List<AgendaModel> MontaListaComputadores(IEnumerable<AgendaModel> listModel)
        {
            var computadores = _servicoDeComputador.GetAll();
            var listAgendaModel = computadores.Select(comp => new AgendaModel
            {
                ComputadorId = comp.Id,
                Computador = Mapper.Map<ComputadorModel>(comp)
            }).ToList();

            foreach (var item in listModel)
            {
                var agenda = listAgendaModel.FirstOrDefault(x => x.ComputadorId == item.ComputadorId);
                if (agenda == null) continue;
                agenda.Id = item.Id;
                agenda.HorarioId = item.HorarioId;
                agenda.AlunoId = item.AlunoId;
                agenda.Horario = item.Horario;
                agenda.Aluno = item.Aluno;
            }

            return listAgendaModel;
        }
    }
}