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
        public JsonResult AbrirCaixa(Caixa caixa)
        {
            caixa.Usuario = Convert.ToInt32(Session["UsuarioId"]);
            caixa.DataAbertura = DateTime.Now;
            db.Caixa.Add(caixa);
            db.SaveChanges();

            // Retorna o ID do caixa recém-aberto
            return Json(new { success = true, message = "Caixa Aberto com sucesso!", caixaId = caixa.Id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FecharCaixa(Caixa caixa)
        {
            Caixa caixaFechamento = db.Caixa.Find(Convert.ToInt32((Session["CaixaId"])));

            caixaFechamento.Usuario = Convert.ToInt32(Session["UsuarioId"]);
            caixaFechamento.TotalFinal = Convert.ToDecimal(db.CalculaCaixa(Convert.ToInt32(Session["CaixaId"])).FirstOrDefault()) + caixaFechamento.TotalInicial;
            caixaFechamento.DataFechamento = DateTime.Now;

            db.Entry(caixaFechamento).State = EntityState.Modified;
            db.SaveChanges();
            return Json("Caixa Fechado", JsonRequestBehavior.AllowGet);
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
