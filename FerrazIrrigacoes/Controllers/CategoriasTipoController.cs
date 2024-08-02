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
    public class CategoriasTipoController : Controller
    {
        private sakitadbEntities db = new sakitadbEntities();

        // GET: CategoriasTipo
        public ActionResult Index()
        {
            if (Session["Usuarioid"] == null)
            {
                return RedirectToAction("Login", "Usuario", null);
            }
            return View(db.CategoriasTipo.ToList());
        }

        // GET: CategoriasTipo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriasTipo categoriasTipo = db.CategoriasTipo.Find(id);
            if (categoriasTipo == null)
            {
                return HttpNotFound();
            }
            return View(categoriasTipo);
        }

        // GET: CategoriasTipo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoriasTipo/Create
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Nome")] CategoriasTipo categoriasTipo)
        {
            if (ModelState.IsValid)
            {
                db.CategoriasTipo.Add(categoriasTipo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(categoriasTipo);
        }

        // GET: CategoriasTipo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriasTipo categoriasTipo = db.CategoriasTipo.Find(id);
            if (categoriasTipo == null)
            {
                return HttpNotFound();
            }
            return View(categoriasTipo);
        }

        // POST: CategoriasTipo/Edit/5
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Nome")] CategoriasTipo categoriasTipo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categoriasTipo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categoriasTipo);
        }

        // GET: CategoriasTipo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriasTipo categoriasTipo = db.CategoriasTipo.Find(id);
            if (categoriasTipo == null)
            {
                return HttpNotFound();
            }
            return View(categoriasTipo);
        }

        // POST: CategoriasTipo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CategoriasTipo categoriasTipo = db.CategoriasTipo.Find(id);
            db.CategoriasTipo.Remove(categoriasTipo);
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
