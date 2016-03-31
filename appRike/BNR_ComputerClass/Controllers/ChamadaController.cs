using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BNR_ComputerClass.Models;
using Dominio.Tools;
using Infra.Servicos;
using Microsoft.Ajax.Utilities;
using WebGrease.Css.Extensions;
using Dominio.Entidades;

namespace BNR_ComputerClass.Controllers
{
    public class ChamadaController : Controller
    {
        private readonly ServicoDeAula _servicoDeAula = new ServicoDeAula();
        private readonly ServicoDeChamada _servicoDeChamada = new ServicoDeChamada();
        private readonly ServicoDeAgenda _servicoDeAgenda = new ServicoDeAgenda();

        public ActionResult Index()
        {
            var aulas = _servicoDeAula.GetAll();
            ViewBag.DataHoje = DateTime.Now.ToShortDateString();
            ViewBag.Dia = Converter.DiaIngParaPort(DateTime.Now.DayOfWeek);
            return View(Mapper.Map<List<AulaModel>>(aulas));
        }

        public ActionResult Create()
        {
            var hoje = Converter.DiaIngParaPort(DateTime.Now.DayOfWeek);
            var horarioId = InitSelects(hoje);
            var aulaModel = new AulaModel
            {
                DataAula = DateTime.Now,
                ChamadasModel = RetornaChamadasCreate(horarioId)
            };

            ViewBag.DataHoje = aulaModel.Data;
            return View(aulaModel);
        }

        [HttpPost]
        public JsonResult Create(ChamadaModel chamadaModel)
        {
            try
            {
                var chamada = Mapper.Map<Chamada>(chamadaModel);
                return Json(_servicoDeChamada.Adicionar(chamada));
            }
            catch
            {
                return Json(false);
            }
        }

        public ActionResult Edit(int id)
        {
            var aula = _servicoDeAula.GetById(id);
            var horarioId = InitSelects("", 0, id);
            var aulaModel = Mapper.Map<AulaModel>(aula);
            aulaModel.ChamadasModel = RetornaChamadasEdit(horarioId, id);
            return View(aulaModel);
        }

        [HttpPost]
        public JsonResult Edit(ChamadaModel chamadaModel)
        {
            try
            {
                var chamada = _servicoDeChamada.GetById(chamadaModel.Id);
                chamada.Presenca = chamadaModel.Presenca;
                return Json(_servicoDeChamada.Alterar(chamada));
            }
            catch
            {
                return Json(false);
            }
        }

        public ActionResult Delete(int id)
        {
            var aula = _servicoDeAula.GetById(id);
            var aulaModel = Mapper.Map<AulaModel>(aula);
            aulaModel.AlunosPresentes = _servicoDeChamada.Get(x => x.AulaId == aula.Id, "Agenda.Aluno").Select(x => x.Agenda.Aluno).Count();
            return View(aulaModel);
        }

        [HttpPost]
        public ActionResult Delete(AulaModel model)
        {
            try
            {
                _servicoDeAula.Excluir(model.Id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        [Route("AtualizaHorariosCreate/{dia}"), HttpGet]
        public ActionResult AtualizaHorariosCreate(string dia)
        {
            var horarioId = InitSelects(dia);

            var aulaModel = new AulaModel
            {
                ChamadasModel = RetornaChamadasCreate(horarioId)
            };

            return PartialView("_Alunos", aulaModel);
        }

        [Route("AtualizaAgendasCreate/{horarioId:int}"), HttpGet]
        public ActionResult AtualizaAgendasCreate(int horarioId)
        {
            var aulaModel = new AulaModel
            {
                ChamadasModel = RetornaChamadasCreate(horarioId)
            };

            InitSelects(aulaModel.ChamadasModel.First().Agenda.Horario.Dia, horarioId);
            return PartialView("_Alunos", aulaModel);
        }

        [Route("AtualizaAgendasEdit/{horarioId:int}/{aulaId:int}"), HttpGet]
        public ActionResult AtualizaAgendasEdit(int horarioId, int aulaId)
        {
            InitSelects("", horarioId, aulaId);

            var aulaModel = new AulaModel
            {
                ChamadasModel = RetornaChamadasEdit(horarioId, aulaId)
            };

            return PartialView("_Alunos", aulaModel);
        }

        [Route("AtualizaPorIntervaloHorarios/{dia}/{inicio}/{fim}"), HttpGet]
        public ActionResult AtualizaPorIntervaloHorarios(string dia, string inicio, string fim)
        {
            var aulaModel = new AulaModel
            {
                ChamadasModel = RetornaPorIntervaloHorarios(dia, inicio, fim)
            };

            return PartialView("_Alunos", aulaModel);
        }

        private int InitSelects(string dia, int horarioId = 0, int aulaId = 0)
        {
            var aulaHoje = _servicoDeChamada.GetAulaPorData(DateTime.Now);
            var chamadasCadastradas = aulaHoje != null ?
                _servicoDeChamada.Get(x => x.AulaId == aulaHoje.Id, "Agenda").Select(x => x.Agenda).Distinct() :
                 new List<Agenda>();

            IEnumerable<Agenda> agendas;

            if (aulaId != 0)
            {
                agendas =
                    _servicoDeChamada.Get(x => x.AulaId == aulaId, "Agenda.Horario")
                        .Select(x => x.Agenda)
                        .OrderBy(x => x.Horario.Ordem)
                        .Distinct()
                        .ToList();

                var firstOrDefault = agendas.FirstOrDefault();
                if (firstOrDefault != null) dia = firstOrDefault.Horario.Dia;
            }
            else
            {
                agendas = _servicoDeAgenda.GetAll("Horario").OrderBy(x => x.Horario.Ordem);
                var remove = chamadasCadastradas.Select(item => agendas.First(x => x.Id == item.Id)).ToList();
                agendas = agendas.Except(remove);
            }

            var horariosModel = Mapper.Map<List<HorarioModel>>(agendas.Select(x => x.Horario).Distinct().ToList());
            horariosModel = horariosModel.Where(x => x.Dia.Equals(dia)).ToList();
            var listHorarios = horariosModel.Where(x => x.Dia.Equals(dia)).OrderBy(x => x.HoraInicial).Distinct();
            var horario = horariosModel.FirstOrDefault(x => x.HoraInicial.Contains(DateTime.Now.Hour.ToString(CultureInfo.InvariantCulture))) ??
                horariosModel.FirstOrDefault();
            horarioId = horario != null && horarioId == 0 ? horario.Id : horarioId;
            TempData["Horarios"] = new SelectList(listHorarios, "Id", "HorarioSelect", horarioId);
            return horarioId;
        }

        private List<ChamadaModel> RetornaChamadasCreate(int horarioId)
        {
            if (horarioId == 0) return new List<ChamadaModel>();
            var agendas = Mapper.Map<List<AgendaModel>>(_servicoDeAgenda.Get(x => x.HorarioId == horarioId, "Aluno", "Horario"));
            var listaChamadas = agendas.Select(agendaModel => new ChamadaModel
            {
                Agenda = agendaModel,
                AgendaId = agendaModel.Id,
                Presenca = true
            }).ToList();
            return listaChamadas;
        }

        private List<ChamadaModel> RetornaChamadasEdit(int horarioId, int aulaId)
        {
            return Mapper.Map<List<ChamadaModel>>(_servicoDeChamada.Get(x => x.AulaId == aulaId && x.Agenda.HorarioId == horarioId, "Agenda.Aluno"));
        }

        private List<ChamadaModel> RetornaPorIntervaloHorarios(string dia, string inicio, string fim)
        {
            var agendas = Mapper.Map<List<AgendaModel>>(_servicoDeAgenda.Get(x => x.Horario.Dia.Equals(dia), "Aluno", "Horario"));
            agendas = PegarIntervalo(agendas, inicio, fim);

            var listaChamadas = agendas.Select(agendaModel => new ChamadaModel
            {
                Agenda = agendaModel,
                AgendaId = agendaModel.Id,
                Presenca = true
            }).ToList();
            return listaChamadas;
        }

        private static List<AgendaModel> PegarIntervalo(IEnumerable<AgendaModel> agendas, string inicio, string fim)
        {
            var horaIni = DateTime.ParseExact(inicio, "HH:mm", System.Globalization.CultureInfo.CurrentCulture);
            var horaFim = DateTime.ParseExact(fim, "HH:mm", System.Globalization.CultureInfo.CurrentCulture);
            return (from item
                           in agendas
                    let itemHoraIni = DateTime.ParseExact(item.Horario.HoraInicial, "hh:mm", System.Globalization.CultureInfo.CurrentCulture)
                    let itemHoraFim = DateTime.ParseExact(item.Horario.HoraFinal, "hh:mm", System.Globalization.CultureInfo.CurrentCulture)
                    where itemHoraIni >= horaIni && itemHoraFim <= horaFim
                    select item).ToList();
        }
    }
}