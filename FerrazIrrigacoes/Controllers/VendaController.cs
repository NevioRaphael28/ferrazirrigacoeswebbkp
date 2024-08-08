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
    public class VendaController : Controller
    {
        private sakitadbEntities db = new sakitadbEntities();

        // GET: Venda
        public ActionResult Index()
        {
            var venda = db.Venda.Include(v => v.Caixa1).Include(v => v.Cliente).Include(v => v.FormaDePagamento1);
            return View(venda.ToList());
        }

        public JsonResult GerarNovaVenda()
        {
            VendaRepositorio objGerar = new VendaRepositorio();
            int Id = objGerar.GerarNovaVenda();
            return Json(Id, JsonRequestBehavior.AllowGet);
        }

        public void InserirItem(ItensVenda objdados) { 
            VendaItensRepositorio objgravar = new VendaItensRepositorio();
            objgravar.InserirItem(objdados);
        }

        // GET: Venda/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venda venda = db.Venda.Find(id);
            if (venda == null)
            {
                return HttpNotFound();
            }
            return View(venda);
        }

        public ActionResult Lancar()
        {
            //Passar os valores para a tela
            ViewBag.Cliente = new SelectList(db.Cliente, "Id", "Nome");

            //Passar os dados
            ViewBag.Produto = new SelectList(db.Produto, "Id", "Nome");

            return View();
        }

        // GET: Venda/Create
        public ActionResult Create()
        {
            ViewBag.Caixa = new SelectList(db.Caixa, "Id", "Id");
            ViewBag.ClienteId = new SelectList(db.Cliente, "Id", "Nome");
            ViewBag.FormaDePagamento = new SelectList(db.FormaDePagamento, "Id", "Descricao");
            return View();
        }

        // POST: Venda/Create
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DataVenda,Valor,Caixa,FormaDePagamento,MaodeObra,Desconto,ClienteId")] Venda venda)
        {
            if (ModelState.IsValid)
            {
                db.Venda.Add(venda);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Caixa = new SelectList(db.Caixa, "Id", "Id", venda.Caixa);
            ViewBag.ClienteId = new SelectList(db.Cliente, "Id", "Nome", venda.ClienteId);
            ViewBag.FormaDePagamento = new SelectList(db.FormaDePagamento, "Id", "Descricao", venda.FormaDePagamento);
            return View(venda);
        }

        // GET: Venda/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venda venda = db.Venda.Find(id);
            if (venda == null)
            {
                return HttpNotFound();
            }
            ViewBag.Caixa = new SelectList(db.Caixa, "Id", "Id", venda.Caixa);
            ViewBag.ClienteId = new SelectList(db.Cliente, "Id", "Nome", venda.ClienteId);
            ViewBag.FormaDePagamento = new SelectList(db.FormaDePagamento, "Id", "Descricao", venda.FormaDePagamento);
            return View(venda);
        }

        // POST: Venda/Edit/5
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DataVenda,Valor,Caixa,FormaDePagamento,MaodeObra,Desconto,ClienteId")] Venda venda)
        {
            if (ModelState.IsValid)
            {
                db.Entry(venda).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Caixa = new SelectList(db.Caixa, "Id", "Id", venda.Caixa);
            ViewBag.ClienteId = new SelectList(db.Cliente, "Id", "Nome", venda.ClienteId);
            ViewBag.FormaDePagamento = new SelectList(db.FormaDePagamento, "Id", "Descricao", venda.FormaDePagamento);
            return View(venda);
        }

        // GET: Venda/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venda venda = db.Venda.Find(id);
            if (venda == null)
            {
                return HttpNotFound();
            }
            return View(venda);
        }

        // POST: Venda/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Venda venda = db.Venda.Find(id);
            db.Venda.Remove(venda);
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
