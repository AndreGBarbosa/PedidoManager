using System;

namespace PedidoManager.Models.ViewModels
{
    public class PedidoViewModel
    {
        public int ClienteId { get; set; }
        public DateTime DataPedido { get; set; }
        public string ItensJson { get; set; } // Recebe os itens via jQuery
    }
}