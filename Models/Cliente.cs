using System.ComponentModel.DataAnnotations;

namespace ProjetoCSharpWeb.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
    }
}
