using PedidoManager.Models;
using PedidoManager.Repositories.Interfaces;

namespace PedidoManager.Services
{
    public class PedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IProdutoRepository _produtoRepository;

        public PedidoService(IPedidoRepository pedidoRepository, IProdutoRepository produtoRepository)
        {
            _pedidoRepository = pedidoRepository;
            _produtoRepository = produtoRepository;
        }

        public async Task<bool> CriarPedidoAsync(Pedido pedido, List<ItemPedido> itens)
        {
            decimal valorTotal = 0;

            foreach (var item in itens)
            {
                var produto = await _produtoRepository.GetByIdAsync(item.ProdutoId);

                if (produto == null || produto.QuantidadeEstoque < item.Quantidade)
                    throw new InvalidOperationException($"Produto '{produto?.Nome}' não possui estoque suficiente.");

                item.PrecoUnitario = produto.Preco;
                valorTotal += produto.Preco * item.Quantidade;
            }

            pedido.DataPedido = DateTime.Now;
            pedido.ValorTotal = valorTotal;
            pedido.Status = "Novo";

            var pedidoId = await _pedidoRepository.CreateAsync(pedido);
            await _pedidoRepository.AdicionarItensAsync(pedidoId, itens);

            // Opcional: abater estoque
            foreach (var item in itens)
            {
                await _produtoRepository.DiminuirEstoqueAsync(item.ProdutoId, item.Quantidade);
            }

            return pedidoId > 0;
        }

        public async Task<bool> AtualizarStatusAsync(int pedidoId, string novoStatus)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(pedidoId);
            if (pedido == null)
                throw new KeyNotFoundException("Pedido não encontrado.");

            var statusAnterior = pedido.Status;
            pedido.Status = novoStatus;

            var atualizado = await _pedidoRepository.UpdateStatusAsync(pedidoId, novoStatus);

            if (atualizado)
            {
                await _pedidoRepository.LogStatusChangeAsync(pedidoId, statusAnterior, novoStatus);
            }

            return atualizado;
        }
    }
}