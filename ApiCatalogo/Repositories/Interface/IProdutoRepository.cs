using ApiCatalogo.Models;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Repositories.Interface
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        //IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParams);
        PagedList<Produto>  GetProdutos(ProdutosParameters produtosParam);
        PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco produtosFiltroParams);
        IEnumerable<Produto> GetProdutosPorCategoria(int id);
    }
}
