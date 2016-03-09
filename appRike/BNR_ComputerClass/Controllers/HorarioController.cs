using System.Collections.Generic;
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
        private readonly List<string> _listDias, _listHorariosIni, _listHorariosFim;

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

        private void InitSelects()
        {
            ViewBag.Dias = new SelectList(_listDias, 0);
            ViewBag.HorariosIni = new SelectList(_listHorariosIni, 0);
            ViewBag.HorariosFim = new SelectList(_listHorariosFim, 0);
        }

        public ActionResult Index()
        {
            var list = Mapper.Map<List<HorarioModel>>(_servicoDeHorario.GetAll());
            return View(list);
        }

        public ActionResult Create()
        {
            InitSelects();
            return View();
        }

        [HttpPost]
        public ActionResult Create(HorarioModel model)
        {
            try
            {
                var horario = Mapper.Map<Horario>(model);
                _servicoDeHorario.Adicionar(horario);
                return RedirectToAction("Index");
            }
            catch
            {
                InitSelects();
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            InitSelects();
            return View(Mapper.Map<HorarioModel>(_servicoDeHorario.GetById(id)));
        }

        [HttpPost]
        public ActionResult Edit(HorarioModel model)
        {
            try
            {
                var horario = Mapper.Map<Horario>(model);
                _servicoDeHorario.Alterar(horario);
                return RedirectToAction("Index");
            }
            catch
            {
                InitSelects();
                return View(model);
            }
        }

        public ActionResult Delete(int id)
        {
            var horario = _servicoDeHorario.GetById(id);
            return View(Mapper.Map<HorarioModel>(horario));
        }

        [HttpPost]
        public ActionResult Delete(HorarioModel model)
        {
            try
            {
                _servicoDeHorario.Excluir(model.Id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }
    }
}