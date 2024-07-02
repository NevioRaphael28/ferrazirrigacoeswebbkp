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
    public class ProdutoController : Controller
    {
        private sakitadbEntities db = new sakitadbEntities();

        // GET: Produto
        public ActionResult Index()
        {
            var produto = db.Produto.Include(p => p.CategoriasTipo1).Include(p => p.Marca1).Include(p => p.UnidadeMedidaTipos1);
            return View(produto.ToList());
        }

        // GET: Produto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = db.Produto.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // GET: Produto/Create
        public ActionResult Create()
        {
            ViewBag.CategoriasTipo = new SelectList(db.CategoriasTipo, "id", "Nome");
            ViewBag.Marca = new SelectList(db.Marca, "Id", "Nome");
            ViewBag.UnidadeMedidaTipos = new SelectList(db.UnidadeMedidaTipos, "Id", "Nome");
            return View();
        }

        // POST: Produto/Create
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,Descricao,Estoque,Valor,Marca,CategoriasTipo,UnidadeMedidaTipos")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                db.Produto.Add(produto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoriasTipo = new SelectList(db.CategoriasTipo, "id", "Nome", produto.CategoriasTipo);
            ViewBag.Marca = new SelectList(db.Marca, "Id", "Nome", produto.Marca);
            ViewBag.UnidadeMedidaTipos = new SelectList(db.UnidadeMedidaTipos, "Id", "Nome", produto.UnidadeMedidaTipos);
            return View(produto);
        }

        // GET: Produto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = db.Produto.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoriasTipo = new SelectList(db.CategoriasTipo, "id", "Nome", produto.CategoriasTipo);
            ViewBag.Marca = new SelectList(db.Marca, "Id", "Nome", produto.Marca);
            ViewBag.UnidadeMedidaTipos = new SelectList(db.UnidadeMedidaTipos, "Id", "Nome", produto.UnidadeMedidaTipos);
            return View(produto);
        }

        // POST: Produto/Edit/5
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,Descricao,Estoque,Valor,Marca,CategoriasTipo,UnidadeMedidaTipos")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(produto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoriasTipo = new SelectList(db.CategoriasTipo, "id", "Nome", produto.CategoriasTipo);
            ViewBag.Marca = new SelectList(db.Marca, "Id", "Nome", produto.Marca);
            ViewBag.UnidadeMedidaTipos = new SelectList(db.UnidadeMedidaTipos, "Id", "Nome", produto.UnidadeMedidaTipos);
            return View(produto);
        }

        // GET: Produto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = db.Produto.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // POST: Produto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Produto produto = db.Produto.Find(id);
            db.Produto.Remove(produto);
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
