using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FerrazIrrigacoes.Models;

namespace FerrazIrrigacoes.Controllers
{
    public class LancamentoController : Controller
    {
        private sakitadbEntities db = new sakitadbEntities();

        // GET: Lancamento
        public ActionResult Index()
        {
            var lancamento = db.Lancamento.Include(l => l.Venda1);
            return View(lancamento.ToList());
        }

        // GET: Lancamento/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lancamento lancamento = db.Lancamento.Find(id);
            if (lancamento == null)
            {
                return HttpNotFound();
            }
            return View(lancamento);
        }

        // GET: Lancamento/Create
        public ActionResult Create()
        {
            ViewBag.Venda = new SelectList(db.Venda, "Id", "Id");
            return View();
        }

        // POST: Lancamento/Create
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Movimento,Valor,Descricao,Venda,Data,CaixaId")] Lancamento lancamento)
        {
            if (ModelState.IsValid)
            {
                db.Lancamento.Add(lancamento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Venda = new SelectList(db.Venda, "Id", "Id", lancamento.Venda);
            return View(lancamento);
        }

        // GET: Lancamento/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lancamento lancamento = db.Lancamento.Find(id);
            if (lancamento == null)
            {
                return HttpNotFound();
            }
            ViewBag.Venda = new SelectList(db.Venda, "Id", "Id", lancamento.Venda);
            return View(lancamento);
        }

        // POST: Lancamento/Edit/5
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Movimento,Valor,Descricao,Venda,Data,CaixaId")] Lancamento lancamento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lancamento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Venda = new SelectList(db.Venda, "Id", "Id", lancamento.Venda);
            return View(lancamento);
        }

        // GET: Lancamento/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lancamento lancamento = db.Lancamento.Find(id);
            if (lancamento == null)
            {
                return HttpNotFound();
            }
            return View(lancamento);
        }

        // POST: Lancamento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lancamento lancamento = db.Lancamento.Find(id);
            db.Lancamento.Remove(lancamento);
            db.SaveChanges();
            return RedirectToAction("Index");
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
