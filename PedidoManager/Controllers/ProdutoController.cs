using Microsoft.AspNetCore.Mvc;
using PedidoManager.Models;

namespace PedidoManager.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<IActionResult> Index(string nome, decimal? precoMin, decimal? precoMax, bool? incluirInativos)
        {
            var produtos = await _produtoRepository.FiltrarAsync(nome, precoMin, precoMax, incluirInativos ?? false);
            return View(produtos);
        }

        public IActionResult Criar() => View();

        [HttpPost]
        public async Task<IActionResult> Criar(Produto produto)
        {
            if (!ModelState.IsValid) return View(produto);

            await _produtoRepository.CreateAsync(produto);
            TempData["Mensagem"] = "Produto criado com sucesso!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditarProduto(int id)
        {
            var produto = await _produtoRepository.GetByIdAsync(id);
            if (produto == null) return NotFound();

            return View(produto);
        }

        [HttpPost]
        public async Task<IActionResult> EditarProduto(Produto produto)
        {
            if (!ModelState.IsValid) return View(produto);

            await _produtoRepository.UpdateAsync(produto);
            TempData["Mensagem"] = "Produto atualizado!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Excluir(int id)
        {
            await _produtoRepository.DesativarAsync(id);
            TempData["Mensagem"] = "Produto desativado com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> BuscarPreco(int id)
        {
            var produto = await _produtoRepository.GetByIdAsync(id);
            if (produto == null) return NotFound();

            return Json(new { preco = produto.Preco });
        }

        [HttpPost]
        public async Task<IActionResult> Desativar(int id)
        {
            await _produtoRepository.DesativarAsync(id);
            TempData["Mensagem"] = "Produto desativado com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Ativar(int id)
        {
            await _produtoRepository.AtivarAsync(id);
            TempData["Mensagem"] = "Produto reativado com sucesso!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Todos()
        {
            var produtos = await _produtoRepository.GetAllIncludingInactiveAsync();
            return View(produtos);
        }
    }
}