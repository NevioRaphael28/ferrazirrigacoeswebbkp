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
    public class UnidadeMedidaTipoController : Controller
    {
        private sakitadbEntities db = new sakitadbEntities();

        // GET: UnidadeMedidaTipo
        public ActionResult Index()
        {
            return View(db.UnidadeMedidaTipos.ToList());
        }

        // GET: UnidadeMedidaTipo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnidadeMedidaTipos unidadeMedidaTipos = db.UnidadeMedidaTipos.Find(id);
            if (unidadeMedidaTipos == null)
            {
                return HttpNotFound();
            }
            return View(unidadeMedidaTipos);
        }

        // GET: UnidadeMedidaTipo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UnidadeMedidaTipo/Create
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome")] UnidadeMedidaTipos unidadeMedidaTipos)
        {
            if (ModelState.IsValid)
            {
                db.UnidadeMedidaTipos.Add(unidadeMedidaTipos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(unidadeMedidaTipos);
        }

        // GET: UnidadeMedidaTipo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnidadeMedidaTipos unidadeMedidaTipos = db.UnidadeMedidaTipos.Find(id);
            if (unidadeMedidaTipos == null)
            {
                return HttpNotFound();
            }
            return View(unidadeMedidaTipos);
        }

        // POST: UnidadeMedidaTipo/Edit/5
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome")] UnidadeMedidaTipos unidadeMedidaTipos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unidadeMedidaTipos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(unidadeMedidaTipos);
        }

        // GET: UnidadeMedidaTipo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnidadeMedidaTipos unidadeMedidaTipos = db.UnidadeMedidaTipos.Find(id);
            if (unidadeMedidaTipos == null)
            {
                return HttpNotFound();
            }
            return View(unidadeMedidaTipos);
        }

        // POST: UnidadeMedidaTipo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UnidadeMedidaTipos unidadeMedidaTipos = db.UnidadeMedidaTipos.Find(id);
            db.UnidadeMedidaTipos.Remove(unidadeMedidaTipos);
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
