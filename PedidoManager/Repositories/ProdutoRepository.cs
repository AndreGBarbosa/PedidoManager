using Dapper;
using PedidoManager.Models;
using System.Data;

namespace PedidoManager.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly IDbConnection _connection;

        public ProdutoRepository(DbConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.CriarConexao();
        }

        public async Task<IEnumerable<Produto>> GetAllAsync()
        {
            string sql = "SELECT * FROM Produto WHERE Ativo = 1";
            return await _connection.QueryAsync<Produto>(sql);
        }

        public async Task<Produto> GetByIdAsync(int id)
        {
            string sql = "SELECT * FROM Produto WHERE Id = @Id";
            return await _connection.QueryFirstOrDefaultAsync<Produto>(sql, new { Id = id });
        }

        public async Task CreateAsync(Produto produto)
        {
            string sql = @"INSERT INTO Produto (Nome, Descricao, Preco, QuantidadeEstoque)
                           VALUES (@Nome, @Descricao, @Preco, @QuantidadeEstoque)";
            await _connection.ExecuteAsync(sql, produto);
        }

        public async Task UpdateAsync(Produto produto)
        {
            string sql = @"UPDATE Produto SET Nome = @Nome, Descricao = @Descricao,
                           Preco = @Preco, QuantidadeEstoque = @QuantidadeEstoque WHERE Id = @Id";
            await _connection.ExecuteAsync(sql, produto);
        }

        public async Task DeleteAsync(int id)
        {
            string sql = "DELETE FROM Produto WHERE Id = @Id";
            await _connection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task DiminuirEstoqueAsync(int produtoId, int quantidade)
        {
            string sql = @"UPDATE Produto
                           SET QuantidadeEstoque = QuantidadeEstoque - @Quantidade
                           WHERE Id = @Id AND QuantidadeEstoque >= @Quantidade";
            await _connection.ExecuteAsync(sql, new { Id = produtoId, Quantidade = quantidade });
        }

        public async Task AumentarEstoqueAsync(int produtoId, int quantidade)
        {
            string sql = @"UPDATE Produto SET QuantidadeEstoque = QuantidadeEstoque + @Quantidade WHERE Id = @Id";
            await _connection.ExecuteAsync(sql, new { Id = produtoId, Quantidade = quantidade });
        }

        public async Task DesativarAsync(int id)
        {
            string sql = "UPDATE Produto SET Ativo = 0 WHERE Id = @Id";
            await _connection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task AtivarAsync(int id)
        {
            string sql = "UPDATE Produto SET Ativo = 1 WHERE Id = @Id";
            await _connection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<IEnumerable<Produto>> GetAllIncludingInactiveAsync()
        {
            string sql = "SELECT * FROM Produto";
            return await _connection.QueryAsync<Produto>(sql);
        }

        public async Task<bool> TemEstoqueSuficienteAsync(int produtoId, int quantidadeSolicitada)
        {
            var sql = "SELECT QuantidadeEmEstoque FROM Produtos WHERE Id = @Id";
            var estoqueAtual = await _connection.QuerySingleOrDefaultAsync<int>(sql, new { Id = produtoId });

            return estoqueAtual >= quantidadeSolicitada;
        }

        public async Task<bool> ValidarEstoqueAsync(List<ItemPedido> itens)
        {
            foreach (ItemPedido item in itens)
            {
                var produto = await GetByIdAsync(item.Id);
                if (produto == null || produto.QuantidadeEstoque < item.Quantidade)
                    return false;
            }
            return true;
        }

        public async Task<IEnumerable<Produto>> FiltrarAsync(string nome, decimal? precoMin, decimal? precoMax, bool incluirInativos)
        {
            var sql = @"SELECT * FROM Produto WHERE 
                (@Nome IS NULL OR Nome LIKE '%' + @Nome + '%') AND
                (@PrecoMin IS NULL OR Preco >= @PrecoMin) AND
                (@PrecoMax IS NULL OR Preco <= @PrecoMax) AND
                (@IncluirInativos = 1 OR Ativo = 1)";

            return await _connection.QueryAsync<Produto>(sql, new
            {
                Nome = nome,
                PrecoMin = precoMin,
                PrecoMax = precoMax,
                IncluirInativos = incluirInativos ? 1 : 0
            });
        }
    }

}