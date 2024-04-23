using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using X.PagedList;

namespace ApiCatalogo.Repositories.Interface
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        //IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParams);
        Task<IPagedList<Produto>>  GetProdutosAsync(ProdutosParameters produtosParam);
        Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroParams);
        Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id);
    }
}
