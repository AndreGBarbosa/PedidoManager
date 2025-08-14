using PedidoManager.Models;

public interface IProdutoRepository
{
    Task<IEnumerable<Produto>> GetAllAsync();
    Task<Produto> GetByIdAsync(int id);
    Task CreateAsync(Produto produto);
    Task UpdateAsync(Produto produto);
    Task DeleteAsync(int id);
    Task DiminuirEstoqueAsync(int produtoId, int quantidade);
}