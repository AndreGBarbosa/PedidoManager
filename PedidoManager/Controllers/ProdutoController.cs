using Microsoft.AspNetCore.Mvc;
using PedidoManager.Models;
using PedidoManager.Repositories.Interfaces;
using System.Threading.Tasks;

namespace PedidoManager.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<IActionResult> Index()
        {
            var produtos = await _produtoRepository.GetAllAsync();
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

        public async Task<IActionResult> Editar(int id)
        {
            var produto = await _produtoRepository.GetByIdAsync(id);
            if (produto == null) return NotFound();

            return View(produto);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Produto produto)
        {
            if (!ModelState.IsValid) return View(produto);

            await _produtoRepository.UpdateAsync(produto);
            TempData["Mensagem"] = "Produto atualizado!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Excluir(int id)
        {
            await _produtoRepository.DeleteAsync(id);
            TempData["Mensagem"] = "Produto excluído!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> BuscarPreco(int id)
        {
            var produto = await _produtoRepository.GetByIdAsync(id);
            if (produto == null) return NotFound();

            return Json(new { preco = produto.Preco });
        }
    }
}