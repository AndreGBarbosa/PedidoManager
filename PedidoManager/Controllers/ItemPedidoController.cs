using Microsoft.AspNetCore.Mvc;
using PedidoManager.Models.ViewModels;
using PedidoManager.Repositories.Interfaces;

namespace PedidoManager.Controllers
{
    public class ItemPedidoController : Controller
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IPedidoRepository _pedidoRepository;

        public ItemPedidoController(IProdutoRepository produtoRepo, IPedidoRepository pedidoRepo)
        {
            _produtoRepository = produtoRepo;
            _pedidoRepository = pedidoRepo;
        }

        public async Task<IActionResult> Adicionar(int pedidoId)
        {
            ViewBag.Produtos = await _produtoRepository.GetAllAsync();
            var model = new ItemPedidoViewModel { PedidoId = pedidoId };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar(ItemPedidoViewModel item)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Produtos = await _produtoRepository.GetAllAsync();
                return View(item);
            }

            var produto = await _produtoRepository.GetByIdAsync(item.ProdutoId);
            if (produto == null || produto.QuantidadeEstoque < item.Quantidade)
            {
                ModelState.AddModelError("", "Produto inválido ou estoque insuficiente.");
                ViewBag.Produtos = await _produtoRepository.GetAllAsync();
                return View(item);
            }

            var itemPedido = new Models.ItemPedido
            {
                PedidoId = item.PedidoId,
                ProdutoId = item.ProdutoId,
                Quantidade = item.Quantidade,
                PrecoUnitario = produto.Preco
            };

            await _pedidoRepository.AdicionarItensAsync(item.PedidoId, new List<Models.ItemPedido> { itemPedido });
            await _produtoRepository.DiminuirEstoqueAsync(item.ProdutoId, item.Quantidade);

            var valorItem = itemPedido.Quantidade * itemPedido.PrecoUnitario;
            await _pedidoRepository.AtualizarValorTotalAsync(itemPedido.PedidoId, valorItem);

            TempData["Mensagem"] = "Item adicionado com sucesso!";
            return RedirectToAction("Detalhes", "Pedido", new { id = item.PedidoId });
        }
    }
}