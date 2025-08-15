using Dapper;
using PedidoManager.Models;
using PedidoManager.Repositories.Interfaces;
using System.Data;

namespace PedidoManager.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly IDbConnection _connection;

        public PedidoRepository(DbConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.CriarConexao();
        }

        public async Task<IEnumerable<Pedido>> GetAllAsync()
        {
            string sql = @"SELECT p.*, c.* FROM Pedido p
                           INNER JOIN Cliente c ON p.ClienteId = c.Id";

            var result = await _connection.QueryAsync<Pedido, Cliente, Pedido>(
                sql,
                (pedido, cliente) =>
                {
                    pedido.Cliente = cliente;
                    return pedido;
                },
                splitOn: "Id"
            );

            return result;
        }

        public async Task<Pedido> GetByIdAsync(int id)
        {
            string sql = @"SELECT p.*, c.* FROM Pedido p
                           INNER JOIN Cliente c ON p.ClienteId = c.Id
                           WHERE p.Id = @Id";

            var result = await _connection.QueryAsync<Pedido, Cliente, Pedido>(
                sql,
                (pedido, cliente) =>
                {
                    pedido.Cliente = cliente;
                    return pedido;
                },
                new { Id = id },
                splitOn: "Id"
            );

            return result.FirstOrDefault();
        }

        public async Task<int> CreateAsync(Pedido pedido)
        {
            string sql = @"INSERT INTO Pedido (ClienteId, DataPedido, ValorTotal, Status)
                           VALUES (@ClienteId, @DataPedido, @ValorTotal, @Status);
                           SELECT CAST(SCOPE_IDENTITY() as int);";

            return await _connection.QuerySingleAsync<int>(sql, pedido);
        }

        public async Task AdicionarItensAsync(int pedidoId, List<ItemPedido> itens)
        {
            string sql = @"INSERT INTO ItemPedido (PedidoId, ProdutoId, Quantidade, PrecoUnitario)
                           VALUES (@PedidoId, @ProdutoId, @Quantidade, @PrecoUnitario)";

            foreach (var item in itens)
            {
                await _connection.ExecuteAsync(sql, new
                {
                    PedidoId = pedidoId,
                    ProdutoId = item.ProdutoId,
                    Quantidade = item.Quantidade,
                    PrecoUnitario = item.PrecoUnitario
                });
            }
        }

        public async Task<bool> UpdateStatusAsync(int pedidoId, string novoStatus)
        {
            string sql = @"UPDATE Pedido SET Status = @Status WHERE Id = @Id";
            int rows = await _connection.ExecuteAsync(sql, new { Id = pedidoId, Status = novoStatus });
            return rows > 0;
        }

        public async Task LogStatusChangeAsync(int pedidoId, string anterior, string novo)
        {
            string sql = @"INSERT INTO AlteracaoStatusPedido (PedidoId, StatusAnterior, StatusNovo, DataAlteracao)
                           VALUES (@PedidoId, @StatusAnterior, @StatusNovo, @DataAlteracao)";

            await _connection.ExecuteAsync(sql, new
            {
                PedidoId = pedidoId,
                StatusAnterior = anterior,
                StatusNovo = novo,
                DataAlteracao = DateTime.Now
            });
        }

        public async Task DeleteAsync(int id)
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            using var transaction = _connection.BeginTransaction();

            try
            {
                string deleteItens = "DELETE FROM ItemPedido WHERE PedidoId = @Id";
                await _connection.ExecuteAsync(deleteItens, new { Id = id }, transaction);

                string deletePedido = "DELETE FROM Pedido WHERE Id = @Id";
                await _connection.ExecuteAsync(deletePedido, new { Id = id }, transaction);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                _connection.Close(); 
            }
        }

        public async Task<Pedido> GetByIdWithItensAsync(int id)
        {
            string sql = @"
        SELECT 
            p.Id, p.ClienteId, p.DataPedido, p.ValorTotal, p.Status,
            c.Id, c.Nome,
            i.Id, i.PedidoId, i.ProdutoId, i.Quantidade, i.PrecoUnitario,
            pr.Nome AS NomeProduto
        FROM Pedido p
        INNER JOIN Cliente c ON p.ClienteId = c.Id
        LEFT JOIN ItemPedido i ON i.PedidoId = p.Id
        LEFT JOIN Produto pr ON pr.Id = i.ProdutoId
        WHERE p.Id = @Id";

            var pedidoDict = new Dictionary<int, Pedido>();

            var result = await _connection.QueryAsync<Pedido, Cliente, ItemPedido, Pedido>(
                sql,
                (pedido, cliente, item) =>
                {
                    if (!pedidoDict.TryGetValue(pedido.Id, out var pedidoEntry))
                    {
                        pedidoEntry = pedido;
                        pedidoEntry.Cliente = cliente;
                        pedidoEntry.Itens = new List<ItemPedido>();
                        pedidoDict.Add(pedido.Id, pedidoEntry);
                    }

                    if (item != null)
                    {
                        pedidoEntry.Itens.Add(item);
                    }

                    return pedidoEntry;
                },
                new { Id = id },
                splitOn: "Id,Id,Id"
            );

            return result.FirstOrDefault();
        }

        public async Task AtualizarValorTotalAsync(int pedidoId, decimal valorAdicional)
        {
            string sql = @"UPDATE Pedido SET ValorTotal = ValorTotal + @ValorAdicional WHERE Id = @Id";
            await _connection.ExecuteAsync(sql, new { Id = pedidoId, ValorAdicional = valorAdicional });
        }

        public async Task RemoverItensAsync(int pedidoId)
        {
            string sql = "DELETE FROM ItemPedido WHERE PedidoId = @PedidoId";
            await _connection.ExecuteAsync(sql, new { PedidoId = pedidoId });
        }

        public async Task AumentarEstoqueAsync(int produtoId, int quantidade)
        {
            string sql = @"UPDATE Produto SET QuantidadeEstoque = QuantidadeEstoque + @Quantidade WHERE Id = @Id";
            await _connection.ExecuteAsync(sql, new { Id = produtoId, Quantidade = quantidade });
        }

        public async Task<bool> UpdateValorTotalAsync(int pedidoId, decimal novoValor)
        {
            var sql = @"
            UPDATE Pedido
            SET ValorTotal = @ValorTotal
            WHERE Id = @Id";

            var rows = await _connection.ExecuteAsync(sql, new
            {
                Id = pedidoId,
                ValorTotal = novoValor
            });

            return rows > 0;
        }




    }
}
