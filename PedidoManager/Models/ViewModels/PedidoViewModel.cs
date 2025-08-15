namespace PedidoManager.Models.ViewModels
{
    public class PedidoViewModel
    {
        public Pedido Pedido { get; set; }
        public IEnumerable<Cliente> Clientes { get; set; }

    }
}