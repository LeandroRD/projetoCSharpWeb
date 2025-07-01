namespace ProjetoCSharpWeb.Models
{
    using System.ComponentModel.DataAnnotations;
    public class Usuario
    {
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Senha { get; set; } = string.Empty;
        [Required]
        public string TipoUsuario { get; set; } = string.Empty;
    }
}
