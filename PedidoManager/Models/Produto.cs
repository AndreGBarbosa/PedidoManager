using System.ComponentModel.DataAnnotations;

namespace PedidoManager.Models
{
    public class Produto
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        public string? Descricao { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Preco { get; set; }

        [Range(0, int.MaxValue)]
        public int QuantidadeEstoque { get; set; }
    }
}
