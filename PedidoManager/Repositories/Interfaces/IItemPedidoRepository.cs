using PedidoManager.Models;

public interface IItemPedidoRepository
{
    Task<IEnumerable<ItemPedido>> GetByPedidoIdAsync(int pedidoId);
    Task UpdateAsync(ItemPedido item);
    Task AddAsync(ItemPedido item);
    Task DeleteAsync(int itemId);
}