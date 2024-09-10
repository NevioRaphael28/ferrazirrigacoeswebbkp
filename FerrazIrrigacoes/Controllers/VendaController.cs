using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using FerrazIrrigacoes.Models;
using FerrazIrrigacoes.Repositorio;
using Microsoft.Ajax.Utilities;
using PagedList;

namespace FerrazIrrigacoes.Controllers
{
    public class VendaController : Controller
    {
        private sakitadbEntities db = new sakitadbEntities();

        // GET: Venda
        public ActionResult Index(int? pagina)
        {
            var contexto = new sakitadbEntities();
            var listaVenda = contexto.Venda.OrderByDescending(x => x.Id).ToList();
            int paginaTamanho = 10;
            int paginaVenda = (pagina ?? 1);


            var venda = db.Venda.Include(v => v.Caixa1).Include(v => v.Cliente).Include(v => v.FormaDePagamento1).OrderByDescending(v => v.Id);
            return View(venda.ToPagedList(paginaVenda, paginaTamanho));
        }

        [HttpGet]
        public JsonResult GerarNovaVenda()
        {
            //LINHA ABAIXO CAIXA
            var caixaAberto = db.Caixa.FirstOrDefault(c => c.DataFechamento == null);
            if (caixaAberto == null)
            {
                return Json(new { sucesso = false, mensagem = "Não há caixa aberto. Abra um caixa para realizar vendas." }, JsonRequestBehavior.AllowGet);
            }
            //LINHA ACIMA CAIXA

            VendaRepositorio objGerar = new VendaRepositorio();
            int Id = objGerar.GerarNovaVenda();
            return Json(Id, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InserirItem(ItensVenda objdados)
        {
            //LINHA ABAIXO CAIXA
            var caixaAberto = db.Caixa.FirstOrDefault(c => c.DataFechamento == null);
            if (caixaAberto == null)
            {
                return Json(new { sucesso = false, mensagem = "Não há caixa aberto. Abra um caixa para inserir itens." });
            }
            //LINHA ACIMA CAIXA

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
                Venda objfecha = db.Venda.Find(objdados.Id);

                if (objfecha == null)
                {
                    return Json(new { sucesso = false, mensagem = "Venda não encontrada." });
                }
                var Total = db.ItensVenda.Where(C => C.Venda == objdados.Id).Sum(C => C.ValorTotalProdutos);
                decimal ValorComDesconto = 0;
                if (objdados.Desconto == 0)
                {
                    ValorComDesconto = Convert.ToDecimal(Total);
                }
                else
                {
                    ValorComDesconto = Convert.ToDecimal(Total - (Total * (objdados.Desconto / 100)));
                }
                //decimal ValorComDesconto = Convert.ToDecimal((objdados.Desconto / 100) * Total);
                decimal MaodeObra = Convert.ToDecimal(ValorComDesconto * Convert.ToDecimal(0.4));
                decimal totalFinal = MaodeObra + ValorComDesconto;

                //    .Select(r => new { r.Sum()  });

                // Atualiza os campos necessários
                objfecha.FormaDePagamento = objdados.FormaDePagamento;
                objfecha.Valor = totalFinal;//objdados.Valor;
                objfecha.Caixa = objdados.Caixa;
                objfecha.DataVenda = DateTime.Now;
                objfecha.ClienteId = objdados.ClienteId;
                objfecha.Desconto = objdados.Desconto;
                objfecha.MaodeObra = MaodeObra; // objdados.MaodeObra;

                db.Entry(objfecha).State = EntityState.Modified;
                db.SaveChanges();

                // Obtém informações do cliente
                var cliente = db.Cliente.SingleOrDefault(c => c.Id == objdados.ClienteId);

                if (cliente == null)
                {
                    return Json(new { sucesso = false, mensagem = "Cliente não encontrado." });
                }

                // Cria um lançamento financeiro
                Lancamento obllancamento = new Lancamento
                {
                    Venda = objdados.Id,
                    Valor = objdados.Valor,
                    Data = DateTime.Now,
                    CaixaId = objdados.Caixa,
                    Movimento = "C",
                    Descricao = "Venda para o cliente: " + cliente.Nome
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
