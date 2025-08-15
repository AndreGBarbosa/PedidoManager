using Microsoft.AspNetCore.Mvc;
using PedidoManager.Models;
using PedidoManager.Models.ViewModels;
using PedidoManager.Repositories.Interfaces;

namespace PedidoManager.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IItemPedidoRepository _itemPedidoRepository;       

        public PedidoController(IPedidoRepository pedidoRepo, IClienteRepository clienteRepo, IProdutoRepository produtoRepository, IItemPedidoRepository itemPedidoRepository)
        {
            _pedidoRepository = pedidoRepo;
            _clienteRepository = clienteRepo;
            _produtoRepository = produtoRepository;
            _itemPedidoRepository = itemPedidoRepository;

        }

        public async Task<IActionResult> Index()
        {
            var pedidos = await _pedidoRepository.GetAllAsync();
            return View(pedidos);
        }

        public async Task<IActionResult> Criar()
        {
            var viewModel = new PedidoViewModel
            {
                Pedido = new Pedido(),
                Clientes = await _clienteRepository.GetAllAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Criar(PedidoViewModel viewModel)
        {
            var pedido = viewModel.Pedido;

            pedido.Cliente = await _clienteRepository.GetByIdAsync(pedido.ClienteId);


            if (!ModelState.IsValid && pedido.ClienteId == null)
            {
                viewModel.Clientes = await _clienteRepository.GetAllAsync();
                return View(viewModel);
            }

            pedido.DataPedido = DateTime.Now;
            pedido.Status = "Novo";
            pedido.ValorTotal = 0;

            var pedidoId = await _pedidoRepository.CreateAsync(pedido);
            TempData["Mensagem"] = "Pedido criado com sucesso!";
            return RedirectToAction("Adicionar", "ItemPedido", new { pedidoId });
        }

        public async Task<IActionResult> Editar(int id)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id);
            if (pedido == null)
                return NotFound();

            pedido.Itens = (await _itemPedidoRepository.GetByPedidoIdAsync(id)).ToList();
            var clientes = await _clienteRepository.GetAllAsync();

            var viewModel = new PedidoViewModel
            {
                Pedido = pedido,
                Clientes = clientes
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(PedidoViewModel viewModel)
        {

            var pedido = viewModel.Pedido;
            pedido.Cliente = await _clienteRepository.GetByIdAsync(pedido.ClienteId);

            if (!ModelState.IsValid && pedido.ClienteId == null)
            {
                viewModel.Clientes = await _clienteRepository.GetAllAsync();
                pedido.Itens = (await _itemPedidoRepository.GetByPedidoIdAsync(pedido.Id)).ToList();
                return View(viewModel);
            }

            var estoqueValido = await _produtoRepository.ValidarEstoqueAsync(pedido.Itens);
            if (!estoqueValido)
            {
                ModelState.AddModelError("", "Um ou mais produtos não possuem estoque suficiente.");
                viewModel.Clientes = await _clienteRepository.GetAllAsync();
                pedido.Itens = (await _itemPedidoRepository.GetByPedidoIdAsync(pedido.Id)).ToList();
                return View(viewModel);
            }

            var atualizado = await _pedidoRepository.UpdateStatusAsync(pedido.Id, pedido.Status);

            decimal novoTotal = 0;
            foreach (var item in pedido.Itens)
            {
                await _itemPedidoRepository.UpdateAsync(item);
                novoTotal += item.Quantidade * item.PrecoUnitario;
            }

            await _pedidoRepository.UpdateValorTotalAsync(pedido.Id, novoTotal);

            TempData["Mensagem"] = atualizado ? "Pedido e itens atualizados com sucesso!" : "Erro ao atualizar pedido.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Excluir(int id)
        {
            await _pedidoRepository.DeleteAsync(id);
            TempData["Mensagem"] = "Pedido excluído!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detalhes(int id)
        {
            var pedido = await _pedidoRepository.GetByIdWithItensAsync(id);
            if (pedido == null)
                return NotFound();

            return View(pedido);
        }

        public async Task<IActionResult> ConfirmarExclusao(int id)
        {
            var pedido = await _pedidoRepository.GetByIdWithItensAsync(id);
            if (pedido == null)
                return NotFound();

            return View(pedido);
        }

        [HttpPost]
        public async Task<IActionResult> ExcluirConfirmado(int id)
        {
            var pedido = await _pedidoRepository.GetByIdWithItensAsync(id);
            if (pedido == null)
                return NotFound();

            foreach (var item in pedido.Itens)
            {
                await _produtoRepository.AumentarEstoqueAsync(item.ProdutoId, item.Quantidade);
            }

            await _pedidoRepository.RemoverItensAsync(id);
            await _pedidoRepository.DeleteAsync(id);

            TempData["Mensagem"] = $"Pedido #{id} excluído com sucesso.";
            return RedirectToAction("Index");
        }
    }
}