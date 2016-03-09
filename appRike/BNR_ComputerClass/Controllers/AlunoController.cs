using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using BNR_ComputerClass.Models;
using Dominio.Entidades;
using Infra.Servicos;

namespace BNR_ComputerClass.Controllers
{
    public class AlunoController : Controller
    {
        private readonly ServicoDeAluno _servicoDeAluno = new ServicoDeAluno();

        public ActionResult Index()
        {
            var list = Mapper.Map<List<AlunoModel>>(_servicoDeAluno.GetAll());
            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(AlunoModel model)
        {
            try
            {
                var aluno = Mapper.Map<Aluno>(model);
                _servicoDeAluno.Adicionar(aluno);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View(Mapper.Map<AlunoModel>(_servicoDeAluno.GetById(id)));
        }

        [HttpPost]
        public ActionResult Edit(AlunoModel model)
        {
            try
            {
                var aluno = Mapper.Map<Aluno>(model);
                _servicoDeAluno.Alterar(aluno);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        public ActionResult Delete(int id)
        {
            var aluno = _servicoDeAluno.GetById(id);
            return View(Mapper.Map<AlunoModel>(aluno));
        }

        [HttpPost]
        public ActionResult Delete(AlunoModel model)
        {
            try
            {
                _servicoDeAluno.Excluir(model.Id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }
    }
}
