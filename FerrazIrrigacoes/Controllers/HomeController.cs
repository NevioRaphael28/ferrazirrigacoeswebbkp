using FerrazIrrigacoes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ferrazIrrigacoesWeb.Controllers
{
    public class HomeController : Controller
    {
        private sakitadbEntities db = new sakitadbEntities();
        public ActionResult Index()
        {
            if (Session["UsuarioId"] == null)
            {
                return RedirectToAction("Login", "Usuario", null);
            }

            var caixa = db.Caixa.Where(registro => registro.DataFechamento.Equals(null)).FirstOrDefault();

            if (caixa == null)
            {
                Session["CaixaId"] = "Fechado";
            }
            else
            {
                Session["CaixaId"] = caixa.Id;
                Session["SaldoCaixa"] = db.CalculaCaixa(Convert.ToInt32(Session["CaixaId"])).FirstOrDefault();
            }

            return View();
        }

        [HttpGet]
        public JsonResult BuscaSaldoCaixa(int id)
        {
            if (Session["CaixaId"].ToString() != "Fechado")
            {
                var saldo = db.CalculaCaixa(Convert.ToInt32(Session["CaixaId"])).FirstOrDefault();
                int caixaid = Convert.ToInt32(Session["CaixaId"]);
                var caixa = db.Caixa.Where(c => c.Id == caixaid).FirstOrDefault();
                double saldototal = Convert.ToDouble(saldo.Value) + Convert.ToDouble(caixa.TotalInicial);

                return Json(new { sucesso = true, valor = saldototal }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { sucesso = true, valor = 0 }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}