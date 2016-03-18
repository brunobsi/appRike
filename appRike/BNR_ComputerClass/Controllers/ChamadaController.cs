using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BNR_ComputerClass.Models;
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

        private int InitSelectHorario(int aulaId = 0)
        {
            var aulaHoje = _servicoDeChamada.GetAulaPorData(DateTime.Now);
            var chamadasCadastradas = aulaHoje != null ?
                _servicoDeChamada.Get(x => x.AulaId == aulaHoje.Id, "Agenda").Select(x => x.Agenda).Distinct() :
                 new List<Agenda>();

            IEnumerable<Agenda> agendas;

            if (aulaId != 0)
                agendas = _servicoDeChamada.Get(x => x.AulaId == aulaId, "Agenda.Horario").Select(x => x.Agenda).OrderBy(x => x.Horario.Ordem).Distinct();
            else
            {
                agendas = _servicoDeAgenda.GetAll("Horario").OrderBy(x => x.Horario.Ordem);
                var remove = chamadasCadastradas.Select(item => agendas.First(x => x.Id == item.Id)).ToList();
                agendas = agendas.Except(remove);
            }

            var horariosModel = Mapper.Map<List<HorarioModel>>(agendas.Select(x => x.Horario).Distinct().ToList());
            ViewBag.Horarios = new SelectList(horariosModel, "Id", "HorarioSelect", 0);

            var firstOrDefault = horariosModel.FirstOrDefault();
            return firstOrDefault != null ? firstOrDefault.Id : 0;
        }

        private List<ChamadaModel> RetornaChamadasCreate(int horarioId)
        {
            if (horarioId == 0) return new List<ChamadaModel>();
            var agendas = Mapper.Map<List<AgendaModel>>(_servicoDeAgenda.Get(x => x.HorarioId == horarioId, "Aluno"));
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

        public ActionResult Index()
        {
            var aulas = _servicoDeAula.GetAll();
            ViewBag.DataHoje = DateTime.Now.ToShortDateString();
            return View(Mapper.Map<List<AulaModel>>(aulas));
        }

        public ActionResult Create()
        {
            var horarioId = InitSelectHorario();
            var aulaModel = new AulaModel
            {
                DataAula = DateTime.Now,
                ChamadasModel = RetornaChamadasCreate(horarioId)
            };

            ViewBag.DataHoje = aulaModel.Data;
            return View(aulaModel);
        }

        [Route("AtualizaAgendasCreate/{horarioId:int}"), HttpGet]
        public ActionResult AtualizaAgendasCreate(int horarioId)
        {
            var aulaModel = new AulaModel
            {
                ChamadasModel = RetornaChamadasCreate(horarioId)
            };

            return PartialView("_Alunos", aulaModel);
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
            var horarioId = InitSelectHorario(id);
            var aula = _servicoDeAula.GetById(id);
            var aulaModel = Mapper.Map<AulaModel>(aula);
            aulaModel.ChamadasModel = RetornaChamadasEdit(horarioId, id);
            return View(aulaModel);
        }

        [Route("AtualizaAgendasEdit/{horarioId:int}/{aulaId:int}"), HttpGet]
        public ActionResult AtualizaAgendasEdit(int horarioId, int aulaId)
        {
            var aulaModel = new AulaModel
            {
                ChamadasModel = RetornaChamadasEdit(horarioId, aulaId)
            };

            return PartialView("_Alunos", aulaModel);
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



    }
}