using System.ComponentModel.DataAnnotations;

namespace PedidoManager.Models
{
    public class ItemPedido
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int ProdutoId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser maior que zero.")]
        public int Quantidade { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero.")]
        public decimal PrecoUnitario { get; set; }

        // Se quiser exibir o nome do produto na View, pode adicionar:
        public string? NomeProduto { get; set; }
    }
}
