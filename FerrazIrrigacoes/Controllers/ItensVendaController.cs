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
    public class ItensVendaController : Controller
    {
        private sakitadbEntities db = new sakitadbEntities();

        // GET: ItensVenda
        public ActionResult Index()
        {
            var itensVenda = db.ItensVenda.Include(i => i.Produto1).Include(i => i.Venda1);
            return View(itensVenda.ToList());
        }

        // GET: ItensVenda/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItensVenda itensVenda = db.ItensVenda.Find(id);
            if (itensVenda == null)
            {
                return HttpNotFound();
            }
            return View(itensVenda);
        }

        // GET: ItensVenda/Create
        public ActionResult Create()
        {
            ViewBag.Produto = new SelectList(db.Produto, "Id", "Nome");
            ViewBag.Venda = new SelectList(db.Venda, "Id", "Id");
            return View();
        }

        // POST: ItensVenda/Create
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ValorProduto,Quantidade,Produto,Venda,ValorTotalProdutos")] ItensVenda itensVenda)
        {
            if (ModelState.IsValid)
            {
                db.ItensVenda.Add(itensVenda);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Produto = new SelectList(db.Produto, "Id", "Nome", itensVenda.Produto);
            ViewBag.Venda = new SelectList(db.Venda, "Id", "Id", itensVenda.Venda);
            return View(itensVenda);
        }

        // GET: ItensVenda/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItensVenda itensVenda = db.ItensVenda.Find(id);
            if (itensVenda == null)
            {
                return HttpNotFound();
            }
            ViewBag.Produto = new SelectList(db.Produto, "Id", "Nome", itensVenda.Produto);
            ViewBag.Venda = new SelectList(db.Venda, "Id", "Id", itensVenda.Venda);
            return View(itensVenda);
        }

        // POST: ItensVenda/Edit/5
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ValorProduto,Quantidade,Produto,Venda,ValorTotalProdutos")] ItensVenda itensVenda)
        {
            if (ModelState.IsValid)
            {
                db.Entry(itensVenda).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Produto = new SelectList(db.Produto, "Id", "Nome", itensVenda.Produto);
            ViewBag.Venda = new SelectList(db.Venda, "Id", "Id", itensVenda.Venda);
            return View(itensVenda);
        }

        // GET: ItensVenda/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItensVenda itensVenda = db.ItensVenda.Find(id);
            if (itensVenda == null)
            {
                return HttpNotFound();
            }
            return View(itensVenda);
        }

        // POST: ItensVenda/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ItensVenda itensVenda = db.ItensVenda.Find(id);
            db.ItensVenda.Remove(itensVenda);
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
