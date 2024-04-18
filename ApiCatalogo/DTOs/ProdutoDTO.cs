namespace ApiCatalogo.DTOs
{
    public class ProdutoDTO
    {
        public int ProdutoId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal preco { get; set; }
        public string ImageUrl { get; set; }
        public int CategoriaId { get; set; }
    }
}
