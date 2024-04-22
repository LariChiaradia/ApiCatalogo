using ApiCatalogo.Models;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Repositories.Interface
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        //IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParams);
        PagedList<Produto>  GetProdutos(ProdutosParameters produtosParam);
        IEnumerable<Produto> GetProdutosPorCategoria(int id);
    }
}
