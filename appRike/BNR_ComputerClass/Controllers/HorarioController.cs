using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BNR_ComputerClass.Models;
using Dominio.Entidades;
using Infra.Servicos;

namespace BNR_ComputerClass.Controllers
{
    public class HorarioController : Controller
    {
        private readonly ServicoDeHorario _servicoDeHorario = new ServicoDeHorario();
        private readonly ServicoDeComputador _servicoDeComputador = new ServicoDeComputador();
        private readonly ServicoDeAluno _servicoDeAluno = new ServicoDeAluno();
        private readonly ServicoDeAgenda _servicoDeAgenda = new ServicoDeAgenda();
        private readonly List<string> _listDias, _listHorariosIni, _listHorariosFim;
        private const int FiltroByDia = 1, FiltroByAluno = 2, FiltroByComputador = 3;

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

            _listHorariosFim = new List<string>
            {
                "09:00", "10:00","11:00","15:00","16:00","17:00",
            };
        }

        private void InitSelectsCreate()
        {
            ViewBag.Dias = new SelectList(_listDias, 0);
            ViewBag.HorariosIni = new SelectList(_listHorariosIni, 0);
            ViewBag.HorariosFim = new SelectList(_listHorariosFim, 0);
            ViewBag.Computadores = new SelectList(_servicoDeComputador.GetAll(), "Id", "Descricao", 0);
            ViewBag.Alunos = new SelectList(_servicoDeAluno.GetAll(), "Id", "Nome", 0);
        }

        private int RetornaAlunosIndex(int alunoId = 0)
        {
            var alunos = _servicoDeAgenda.GetAll("Aluno").Select(x => x.Aluno).Distinct().OrderBy(x => x.Nome).ToList();
            TempData["Filtro"] = new SelectList(alunos, "Id", "Nome", alunoId);
            var firstOrDefault = alunos.FirstOrDefault();
            return firstOrDefault != null ? firstOrDefault.Id : 0;
        }

        private int RetornaComputadorIndex(int computadorId = 0)
        {
            var computadores = _servicoDeAgenda.GetAll("Computador").Select(x => x.Computador).Distinct().OrderBy(x => x.Descricao).ToList();
            TempData["Filtro"] = new SelectList(computadores, "Id", "Descricao", computadorId);
            var firstOrDefault = computadores.FirstOrDefault();
            return firstOrDefault != null ? firstOrDefault.Id : 0;
        }

        private string RetornaDiaIndex(string dia = "")
        {
            var dias = _servicoDeAgenda.GetAll("Horario").OrderBy(x => x.Horario.Ordem).Select(x => x.Horario.Dia).Distinct().ToList();
            TempData["Filtro"] = new SelectList(dias, dia);
            return dias.First();
        }

        public ActionResult Index()
        {
            var dia = RetornaDiaIndex();
            var list = Mapper.Map<List<AgendaModel>>(_servicoDeAgenda.Get(x => x.Horario.Dia.Equals(dia), "Horario", "Aluno", "Computador"));
            return View(list);
        }

        [Route("AtualizaHorariosIndex/{filtro:int}/{value}"), HttpGet]
        public ActionResult AtualizaHorariosIndex(int filtro, string value)
        {
            var list = _servicoDeAgenda.GetAll("Horario", "Aluno", "Computador");
            switch (filtro)
            {
                case FiltroByAluno:
                    var alunoId = Convert.ToInt32(value);
                    RetornaAlunosIndex(alunoId);
                    return PartialView("_Alunos", Mapper.Map<List<AgendaModel>>(list.Where(x => x.AlunoId == alunoId)));
                case FiltroByComputador:
                    var computadorId = Convert.ToInt32(value);
                    RetornaComputadorIndex(computadorId);
                    return PartialView("_Alunos", Mapper.Map<List<AgendaModel>>(list.Where(x => x.ComputadorId == computadorId)));
                case FiltroByDia:
                    RetornaDiaIndex(value);
                    return PartialView("_Alunos", Mapper.Map<List<AgendaModel>>(list.Where(x => x.Horario.Dia.Equals(value))));
            }

            return null;
        }

        [Route("AtualizaSelectFiltro/{filtro:int}"), HttpGet]
        public ActionResult AtualizaSelectFiltro(int filtro)
        {
            var list = _servicoDeAgenda.GetAll("Horario", "Aluno", "Computador");
            switch (filtro)
            {
                case FiltroByAluno:
                    var alunoId = RetornaAlunosIndex();
                    return PartialView("_Alunos", Mapper.Map<List<AgendaModel>>(list.Where(x => x.AlunoId == alunoId)));
                case FiltroByComputador:
                    var computadorId = RetornaComputadorIndex();
                    return PartialView("_Alunos", Mapper.Map<List<AgendaModel>>(list.Where(x => x.ComputadorId == computadorId)));
                case FiltroByDia:
                    var dia = RetornaDiaIndex();
                    return PartialView("_Alunos", Mapper.Map<List<AgendaModel>>(list.Where(x => x.Horario.Dia.Equals(dia))));
            }

            return PartialView("_Alunos", list);
        }

        public ActionResult Create()
        {
            InitSelectsCreate();
            return View();
        }

        [Route("SugerirHoraFinal/{horaInicial}"), HttpGet]
        public JsonResult SugerirHoraFinal(string horaInicial)
        {
            var hora = DateTime.ParseExact(horaInicial, "H:m", null).Hour;
            var list = (from t in _listHorariosFim let horafim = DateTime.ParseExact(t, "H:m", null).Hour where horafim > hora select t).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(AgendaModel model)
        {
            try
            {
                var agenda = Mapper.Map<Agenda>(model);
                _servicoDeHorario.Adicionar(agenda.Horario);
                agenda.HorarioId = agenda.Horario.Id;
                if (_servicoDeAgenda.VerificarSePodeAgendar(agenda))
                {
                    _servicoDeAgenda.Adicionar(agenda);
                    return RedirectToAction("Index");
                }

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
    }
}