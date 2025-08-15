using System.ComponentModel.DataAnnotations;

namespace PedidoManager.Models.ViewModels
{
    public class ItemPedidoViewModel
    {
        [Required]
        public int PedidoId { get; set; }

        [Required]
        public int ProdutoId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser maior que zero.")]
        public int Quantidade { get; set; }

        public decimal PrecoUnitario { get; set; } // sem [Required]

        public string? NomeProduto { get; set; }
    }
}