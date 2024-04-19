using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCatalogo.DTOs
{
    public record ProdutoDTO
    {
        public int ProdutoId { get; set; }
        [Required]
        [StringLength(80)]
        public string Nome { get; set; }
        [Required]
        [StringLength(300)]
        public string Descricao { get; set; }
        [Required]
        public decimal preco { get; set; }
        [Required]
        [StringLength(300)]
        public string ImageUrl { get; set; }
        public int CategoriaId { get; set; }
    }
}
