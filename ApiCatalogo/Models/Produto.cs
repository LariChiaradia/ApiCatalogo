using ApiCatalogo.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiCatalogo.Models
{
    [Table("Produtos")]
    public class Produto
    {
        [Key]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage ="O nome é obrigatório")]
        [StringLength(80)]
        [PrimeiraLetraMaiuscula]
        public string? Nome { get; set; }

        [Required]
        [StringLength(300, MinimumLength =10, ErrorMessage = "A descrição deve ter entre 10 e 300 caracteres")]
        public string? Descricao { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "O preço deve estar entre {1} {2}")]
        public decimal Preco { get; set; }

        [Required]
        [StringLength(300)]
        public string? ImageUrl { get; set; }

        public float Estoque { get; set; }
        public DateTime DataCadastro { get; set; }

        public int CategoriaId { get; set; }
        [JsonIgnore]
        public Categoria? Categoria { get; set; }
    }
}
