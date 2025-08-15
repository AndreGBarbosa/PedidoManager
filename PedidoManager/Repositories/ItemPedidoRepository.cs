using Dapper;
using PedidoManager.Models;
using System.Data;

public class ItemPedidoRepository : IItemPedidoRepository
{
    private readonly IDbConnection _connection;

    public ItemPedidoRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<ItemPedido>> GetByPedidoIdAsync(int pedidoId)
    {
        var sql = @"
        SELECT 
            i.Id, i.PedidoId, i.ProdutoId, i.Quantidade, i.PrecoUnitario,
            p.Nome AS NomeProduto
        FROM ItemPedido i
        INNER JOIN Produto p ON i.ProdutoId = p.Id
        WHERE i.PedidoId = @PedidoId";

        var itens = await _connection.QueryAsync<ItemPedido>(sql, new { PedidoId = pedidoId });
        return itens;
    }

    public async Task UpdateAsync(ItemPedido item)
    {
        var sql = @"
            UPDATE ItemPedido
            SET Quantidade = @Quantidade,
                PrecoUnitario = @PrecoUnitario
            WHERE Id = @Id";

        await _connection.ExecuteAsync(sql, item);
    }

    public async Task AddAsync(ItemPedido item)
    {
        var sql = @"
            INSERT INTO ItemPedido (PedidoId, ProdutoId, Quantidade, PrecoUnitario)
            VALUES (@PedidoId, @ProdutoId, @Quantidade, @PrecoUnitario)";

        await _connection.ExecuteAsync(sql, item);
    }

    public async Task DeleteAsync(int itemId)
    {
        var sql = "DELETE FROM ItemPedido WHERE Id = @Id";
        await _connection.ExecuteAsync(sql, new { Id = itemId });
    }
}