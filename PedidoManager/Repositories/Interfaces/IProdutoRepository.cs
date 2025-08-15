using PedidoManager.Models;

public interface IProdutoRepository
{
    Task<IEnumerable<Produto>> GetAllAsync();
    Task<Produto> GetByIdAsync(int id);
    Task CreateAsync(Produto produto);
    Task UpdateAsync(Produto produto);
    Task DeleteAsync(int id);
    Task DiminuirEstoqueAsync(int produtoId, int quantidade);
    Task AumentarEstoqueAsync(int produtoId, int quantidade);
    Task AtivarAsync(int id);
    Task DesativarAsync(int id);
    Task<IEnumerable<Produto>> GetAllIncludingInactiveAsync();
    Task<bool> TemEstoqueSuficienteAsync(int produtoId, int quantidadeSolicitada);
    Task<bool> ValidarEstoqueAsync(List<ItemPedido> itens);
    Task<IEnumerable<Produto>> FiltrarAsync(string nome, decimal? precoMin, decimal? precoMax, bool incluirInativos);

}