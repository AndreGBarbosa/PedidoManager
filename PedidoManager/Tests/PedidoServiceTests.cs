using PedidoManager.Models;
using PedidoManager.Models.ViewModels;
using PedidoManager.Repositories.Interfaces;
using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PedidoManager.Tests
{
    public class PedidoServiceTests
    {
        [Fact]
        public async Task DeveRetornarErro_QuandoEstoqueInsuficiente()
        {
            // Arrange
            var produtoMock = new Produto { Id = 1, Nome = "Produto A", Preco = 10, QuantidadeEstoque = 2 };

            var produtoRepo = new Mock<IProdutoRepository>();
            produtoRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(produtoMock);

            var pedidoRepo = new Mock<IPedidoRepository>();

            var itens = new List<ItemPedidoViewModel>
            {
                new ItemPedidoViewModel { ProdutoId = 1, NomeProduto = "Produto A", Quantidade = 5, PrecoUnitario = 10 }
            };

            var service = new PedidoService(pedidoRepo.Object, produtoRepo.Object);

            // Act
            var result = await service.ValidarEstoqueAsync(itens);

            // Assert
            Assert.False(result.Sucesso);
            Assert.Equal("Insufficient stock for product: Produto A", result.Mensagem);
        }
    }

    public class PedidoService
    {
        private readonly IPedidoRepository _pedidoRepo;
        private readonly IProdutoRepository _produtoRepo;

        public PedidoService(IPedidoRepository pedidoRepo, IProdutoRepository produtoRepo)
        {
            _pedidoRepo = pedidoRepo;
            _produtoRepo = produtoRepo;
        }

        public async Task<ResultadoValidacao> ValidarEstoqueAsync(List<ItemPedidoViewModel> itens)
        {
            foreach (var item in itens)
            {
                var produto = await _produtoRepo.GetByIdAsync(item.ProdutoId);
                if (produto == null || produto.QuantidadeEstoque < item.Quantidade)
                {
                    return new ResultadoValidacao
                    {
                        Sucesso = false,
                        Mensagem = $"Insufficient stock for product: {item.NomeProduto}"
                    };
                }
            }

            return new ResultadoValidacao { Sucesso = true };
        }
    }

    public class ResultadoValidacao
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
    }
}