using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BNR_ComputerClass.Models;
using Dominio.Entidades;
using Infra.Servicos;

namespace BNR_ComputerClass.Controllers
{
    public class ComputadorController : Controller
    {
        private readonly ServicoDeComputador _servicoDeComputador = new ServicoDeComputador();

        public ActionResult Index()
        {
            var list = Mapper.Map<List<ComputadorModel>>(_servicoDeComputador.GetAll());
            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ComputadorModel model)
        {
            try
            {
                var computador = Mapper.Map<Computador>(model);
                _servicoDeComputador.Adicionar(computador);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View(Mapper.Map<ComputadorModel>(_servicoDeComputador.GetById(id)));
        }

        [HttpPost]
        public ActionResult Edit(ComputadorModel model)
        {
            try
            {
                var computador = Mapper.Map<Computador>(model);
                _servicoDeComputador.Alterar(computador);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        public ActionResult Delete(int id)
        {
            var computador = _servicoDeComputador.GetById(id);
            return View(Mapper.Map<ComputadorModel>(computador));
        }

        [HttpPost]
        public ActionResult Delete(ComputadorModel model)
        {
            try
            {
                _servicoDeComputador.Excluir(model.Id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }
    }
}
