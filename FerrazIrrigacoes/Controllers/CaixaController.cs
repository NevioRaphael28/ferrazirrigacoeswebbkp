using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FerrazIrrigacoes.Models;
using FerrazIrrigacoes.Repositorio;

namespace FerrazIrrigacoes.Controllers
{
    public class CaixaController : Controller
    {
        private sakitadbEntities db = new sakitadbEntities();

        // GET: Caixa
        public ActionResult Index()
        {
            var caixas = db.Caixa.ToList();
            return View(caixas);
        }

        // GET: Caixa/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Caixa caixa = db.Caixa.Find(id);
            if (caixa == null)
            {
                return HttpNotFound();
            }
            return View(caixa);
        }

        // GET: Caixa/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Caixa/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TotalFinal,TotalInicial,DataAbertura,DataFechamento")] Caixa caixa)
        {
            if (ModelState.IsValid)
            {
                db.Caixa.Add(caixa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(caixa);
        }

        // GET: Caixa/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Caixa caixa = db.Caixa.Find(id);
            if (caixa == null)
            {
                return HttpNotFound();
            }
            return View(caixa);
        }

        // POST: Caixa/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TotalFinal,TotalInicial,DataAbertura,DataFechamento")] Caixa caixa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(caixa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(caixa);
        }

        // GET: Caixa/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Caixa caixa = db.Caixa.Find(id);
            if (caixa == null)
            {
                return HttpNotFound();
            }
            return View(caixa);
        }

        // POST: Caixa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Caixa caixa = db.Caixa.Find(id);
            db.Caixa.Remove(caixa);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult AbrirCaixa(DateTime dataAbertura, double valorAbertura)
        {
            try
            {
                var caixaAberto = db.Caixa.FirstOrDefault(c => c.DataFechamento == null);
                if (caixaAberto != null)
                {
                    return Json(new { success = false, message = "Já existe um caixa aberto." });
                }

                Caixa caixa = new Caixa
                {
                    DataAbertura = dataAbertura,
                    TotalInicial = (decimal?)valorAbertura
                };

                db.Caixa.Add(caixa);
                db.SaveChanges();

                return Json(new { success = true, message = "Caixa aberto com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erro ao abrir o caixa: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult FecharCaixa(int id, DateTime dataFechamento, double valorFechamento)
        {
            try
            {
                var caixa = db.Caixa.FirstOrDefault(c => c.Id == id && c.DataFechamento == null);
                if (caixa == null)
                {
                    return Json(new { success = false, message = "Nenhum caixa aberto encontrado." });
                }

                caixa.DataFechamento = dataFechamento;
                caixa.TotalFinal = (decimal?)valorFechamento;
                db.SaveChanges();

                return Json(new { success = true, message = "Caixa fechado com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erro ao fechar o caixa: " + ex.Message });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
