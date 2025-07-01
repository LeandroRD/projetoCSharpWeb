using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoCSharpWeb.Models
{
    public class Cotacao
    {
        public int Id { get; set; }

        [Required]
        public string? Descricao { get; set; }

        [Required]
        [ForeignKey("Fornecedor")]
        public int FornecedorId { get; set; }
        public Fornecedor? Fornecedor { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Valor { get; set; }

        [Required]
        public int Aprovacao { get; set; } // 0 = Pendente, 1 = Aprovado, 2 = Não Aprovado
    }
}
