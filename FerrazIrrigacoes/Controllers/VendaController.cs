using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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

        [HttpGet]
        public JsonResult GerarNovaVenda()
        {
            VendaRepositorio objGerar = new VendaRepositorio();
            int Id = objGerar.GerarNovaVenda();
            return Json(Id, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InserirItem(ItensVenda objdados)
        {
            VendaItensRepositorio objgravar = new VendaItensRepositorio();
            objgravar.InserirItem(objdados);

            return Json(new { sucesso = true, mensagem = "Item inserido com sucesso" });
        }

        [HttpPost]
        public JsonResult FecharVenda(Venda objdados)
        {
            if (objdados == null)
            {
                return Json(new { sucesso = false, mensagem = "Dados da venda não foram fornecidos." });
            }
            try
            {
                // Atribui o ID do Caixa a partir da sessão
                objdados.Caixa = Convert.ToInt32(Session["CaixaId"]);
                objdados.DataVenda = DateTime.Now;

                // Atualiza o registro da venda
                VendaRepositorio objFechar = new VendaRepositorio();
                objFechar.FecharVenda(objdados);

                // Obtém informações do cliente
                var cliente = db.Cliente.SingleOrDefault(c => c.Id == objdados.ClienteId);

                // Cria um lançamento financeiro
                Lancamento obllancamento = new Lancamento
                {
                    Venda = objdados.Id,
                    Valor = objdados.Valor,
                    Data = DateTime.Now,
                    CaixaId = objdados.Caixa,
                    Movimento = "C"
                };

                // Adiciona o lançamento ao banco de dados
                db.Lancamento.Add(obllancamento);
                db.SaveChanges();

                return Json(new { sucesso = true, mensagem = "Venda concluída com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, mensagem = $"Erro ao concluir a venda: {ex.Message}" });
            }
        }

        [HttpGet]
        public JsonResult ObterFormasPagamento()
        {
            var formasPagamento = db.FormaDePagamento.ToList();
            var result = new
            {
                sucesso = true,
                formasPagamento = formasPagamento.Select(fp => new
                {
                    id = fp.Id,
                    nome = fp.Descricao
                }).ToList(),
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
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
            ViewBag.Cliente = new SelectList(db.Cliente, "Id", "Nome");
            ViewBag.Produto = new SelectList(db.Produto, "Id", "Nome");
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Caixa = new SelectList(db.Caixa, "Id", "Id");
            ViewBag.ClienteId = new SelectList(db.Cliente, "Id", "Nome");
            ViewBag.FormaDePagamento = new SelectList(db.FormaDePagamento, "Id", "Descricao");
            return View();
        }

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

        [HttpGet]
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

        [HttpGet]
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

        public JsonResult IncluirPagamento(Lancamento lancamento)
        {
            try
            {
                // Adiciona o lançamento ao banco de dados
                db.Lancamento.Add(lancamento);
                db.SaveChanges();

                return Json(new { sucesso = true, mensagem = "Pagamento incluído com sucesso!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, mensagem = "Erro ao incluir pagamento: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult BuscarValor(int id)
        {
            var produto = db.Produto.SingleOrDefault(p => p.Id == id);

            if (produto == null)
            {
                return Json(new { sucesso = false, mensagem = "Produto não encontrado" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { sucesso = true, valor = produto.Valor }, JsonRequestBehavior.AllowGet);
        }
    }
}
