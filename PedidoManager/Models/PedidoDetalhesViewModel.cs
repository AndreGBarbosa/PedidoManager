namespace PedidoManager.Models
{
    public class PedidoDetalhesViewModel
    {
        public int Id { get; set; }
        public string NomeCliente { get; set; }
        public DateTime Data { get; set; }
        public decimal ValorTotal { get; set; }
        public string Status { get; set; }

        public List<ItemPedidoDTO> Itens { get; set; }
    }

    public class ItemPedidoDTO
    {
        public string NomeProduto { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
    }
}
