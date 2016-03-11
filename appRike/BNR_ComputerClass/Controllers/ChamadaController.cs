using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BNR_ComputerClass.Models;
using Infra.Servicos;
using WebGrease.Css.Extensions;
using Dominio.Entidades;

namespace BNR_ComputerClass.Controllers
{
    public class ChamadaController : Controller
    {
        private readonly ServicoDeAula _servicoDeAula = new ServicoDeAula();
        private readonly ServicoDeChamada _servicoDeChamada = new ServicoDeChamada();
        private readonly ServicoDeAgenda _servicoDeAgenda = new ServicoDeAgenda();

        private int InitSelects()
        {
            var horarios = _servicoDeAgenda.GetAll("Horario").Select(x => x.Horario).Distinct().ToList();
            var horariosModel = Mapper.Map<List<HorarioModel>>(horarios);
            horariosModel.ForEach(x => x.MontaHorarioSelect());
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

        public ActionResult Index()
        {
            var aulas = _servicoDeAula.GetAll();
            ViewBag.DataHoje = DateTime.Now.ToShortDateString();
            return View(Mapper.Map<List<AulaModel>>(aulas));
        }

        public ActionResult Create()
        {
            var horarioId = InitSelects();
            var aulaModel = new AulaModel
            {
                DataAula = DateTime.Now,
                ChamadasModel = RetornaChamadasCreate(horarioId)
            };

            ViewBag.DataHoje = aulaModel.Data;
            return View(aulaModel);
        }

        [Route("AlteraAgendas/{horarioId:int}"), HttpGet]
        public ActionResult AlteraAgendas(int horarioId)
        {
            return PartialView("_Alunos", RetornaChamadasCreate(horarioId));
        }

        [HttpPost]
        public JsonResult Create(ChamadaModel chamadaModel)
        {
            try
            {
                var chamada = Mapper.Map<Chamada>(chamadaModel);
                return Json( _servicoDeChamada.Adicionar(chamada));
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