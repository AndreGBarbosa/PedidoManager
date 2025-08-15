using System.ComponentModel.DataAnnotations;

namespace PedidoManager.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        [Required]
        public int ClienteId { get; set; }

        public DateTime DataPedido { get; set; }

        [Required]
        public decimal ValorTotal { get; set; }

        [Required]
        public string Status { get; set; }

        public Cliente Cliente { get; set; }

        public List<ItemPedido> Itens { get; set; } = new();

    }
}