using Dapper;
using PedidoManager.Models;
using PedidoManager.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

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
            string sql = "SELECT * FROM Produto";
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
    }
}