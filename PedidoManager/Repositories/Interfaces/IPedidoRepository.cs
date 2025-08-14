using PedidoManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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
    }
}