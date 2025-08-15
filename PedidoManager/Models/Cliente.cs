using System.ComponentModel.DataAnnotations;

namespace PedidoManager.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Telefone { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}