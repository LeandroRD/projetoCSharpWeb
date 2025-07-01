namespace ProjetoCSharpWeb.Models
{
    using System.ComponentModel.DataAnnotations;
    public class Fornecedor
    {
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
    }
}
