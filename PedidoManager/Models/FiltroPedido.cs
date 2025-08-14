namespace PedidoManager.Models
{
    public class FiltroPedido
    {
        public string? NomeCliente { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
        public decimal? ValorMinimo { get; set; }
        public decimal? ValorMaximo { get; set; }
    }
}
