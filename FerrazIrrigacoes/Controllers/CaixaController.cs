using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FerrazIrrigacoes.Models;

namespace ferrazIrrigacoesWeb.Controllers
{
    public class CaixaController : Controller
    {
        private sakitadbEntities db = new sakitadbEntities();

        // GET: Caixa
        public ActionResult Index()
        {
            // Verificar se o caixa está aberto e obter o caixaId
            int? caixaId = (int?)Session["CaixaId"];
            ViewBag.CaixaId = caixaId;
            ViewBag.CaixaAberto = VerificarCaixaAberto(); // Mantenha isso se precisar

            var caixa = db.Caixa.Include(c => c.Usuario1);
            return View(caixa.ToList());
        }
        public bool VerificarCaixaAberto()
        {
            return db.Caixa.Any(c => c.DataFechamento == null);
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
            ViewBag.Usuario = new SelectList(db.Usuario, "Id", "Email");
            return View();
        }

        // POST: Caixa/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TotalFinal,TotalInicial,DataAbertura,DataFechamento,Usuario")] Caixa caixa)
        {
            if (ModelState.IsValid)
            {
                db.Caixa.Add(caixa);
                db.SaveChanges();

                Session["CaixaId"] = caixa.Id;

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
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
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

        public JsonResult AbrirCaixa(Caixa caixa)
        {
            caixa.Usuario = Convert.ToInt32(Session["UsuarioId"]);
            db.Caixa.Add(caixa);
            caixa.DataAbertura = DateTime.Now;
            db.SaveChanges();

            Session["CaixaId"] = caixa.Id;
            return Json(new { sucesso = true, mensagem = "Caixa Aberto", caixaId = caixa.Id }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FecharCaixa()
        {
            int? caixaId = Session["CaixaId"] as int?;
            if (caixaId.HasValue)
            {
                Caixa CaixaFechamento = db.Caixa.Find(caixaId.Value);

                if (CaixaFechamento != null)
                {
                    CaixaFechamento.Usuario = Convert.ToInt32(Session["UsuarioId"]);
                    CaixaFechamento.TotalFinal = Convert.ToDecimal(db.CalculaCaixa(caixaId.Value).FirstOrDefault()) + CaixaFechamento.TotalInicial;
                    CaixaFechamento.DataFechamento = DateTime.Now;

                    db.Entry(CaixaFechamento).State = EntityState.Modified;
                    db.SaveChanges();

                    // Remove o caixaId da sessão
                    Session["CaixaId"] = null;

                    return Json(new { sucesso = true, mensagem = "Caixa Fechado" }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { sucesso = false, mensagem = "Erro ao fechar caixa" }, JsonRequestBehavior.AllowGet);
        }
    }
}