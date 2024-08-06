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
    public class CaixaController : Controller
    {
        private sakitadbEntities db = new sakitadbEntities();

        // GET: Caixa
        public ActionResult Index()
        {
            var caixa = db.Caixa.Include(c => c.Usuario1);
            return View(caixa.ToList());
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

        //pagina de lançamento
        public ActionResult Lancar()
        {
            //Passar os valores para a tela
            ViewBag.Cliente = new SelectList(db.Cliente, "Id", "Nome");

            //Passar os dados
            ViewBag.Produto = new SelectList(db.Produto, "Id", "Nome");

            return View();
        }


        // GET: Caixa/Create
        public ActionResult Create()
        {
            ViewBag.Usuario = new SelectList(db.Usuario, "Id", "Email");
            return View();
        }

        // POST: Caixa/Create
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TotalFinal,TotalInicial,DataAbertura,DataFechamento,Usuario")] Caixa caixa)
        {
            if (ModelState.IsValid)
            {
                db.Caixa.Add(caixa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Usuario = new SelectList(db.Usuario, "Id", "Email", caixa.Usuario);
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
            ViewBag.Usuario = new SelectList(db.Usuario, "Id", "Email", caixa.Usuario);
            return View(caixa);
        }

        // POST: Caixa/Edit/5
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TotalFinal,TotalInicial,DataAbertura,DataFechamento,Usuario")] Caixa caixa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(caixa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Usuario = new SelectList(db.Usuario, "Id", "Email", caixa.Usuario);
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
