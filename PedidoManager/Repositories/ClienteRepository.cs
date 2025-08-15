using Dapper;
using PedidoManager.Models;
using PedidoManager.Repositories.Interfaces;
using System.Data;

namespace PedidoManager.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly IDbConnection _connection;

        public ClienteRepository(DbConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.CriarConexao();
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            string sql = "SELECT * FROM Cliente";
            return await _connection.QueryAsync<Cliente>(sql);
        }

        public async Task<Cliente> GetByIdAsync(int id)
        {
            string sql = "SELECT * FROM Cliente WHERE Id = @Id";
            return await _connection.QueryFirstOrDefaultAsync<Cliente>(sql, new { Id = id });
        }

        public async Task CreateAsync(Cliente cliente)
        {
            string sql = @"INSERT INTO Cliente (Nome, Email, Telefone, DataCadastro)
                           VALUES (@Nome, @Email, @Telefone, @DataCadastro)";
            await _connection.ExecuteAsync(sql, cliente);
        }

        public async Task UpdateAsync(Cliente cliente)
        {
            string sql = @"UPDATE Cliente SET Nome = @Nome, Email = @Email, Telefone = @Telefone
                           WHERE Id = @Id";
            await _connection.ExecuteAsync(sql, cliente);
        }

        public async Task DeleteAsync(int id)
        {
            string sql = "DELETE FROM Cliente WHERE Id = @Id";
            await _connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}