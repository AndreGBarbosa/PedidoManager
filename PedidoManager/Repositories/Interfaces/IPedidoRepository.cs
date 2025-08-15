using PedidoManager.Models;

namespace PedidoManager.Repositories.Interfaces
{
    public interface IPedidoRepository
    {
        Task<IEnumerable<Pedido>> GetAllAsync();
        Task<Pedido> GetByIdAsync(int id);
        Task<int> CreateAsync(Pedido pedido);
        Task AdicionarItensAsync(int pedidoId, List<ItemPedido> itens);
        Task<bool> UpdateStatusAsync(int pedidoId, string novoStatus);
        Task LogStatusChangeAsync(int pedidoId, string anterior, string novo);
        Task DeleteAsync(int id);
        Task<Pedido> GetByIdWithItensAsync(int id);
        Task AtualizarValorTotalAsync(int pedidoId, decimal valorAdicional);
        Task RemoverItensAsync(int pedidoId);
        Task AumentarEstoqueAsync(int produtoId, int quantidade);
        Task<bool> UpdateValorTotalAsync(int pedidoId, decimal novoValor);

    }
}