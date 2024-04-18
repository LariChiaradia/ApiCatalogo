using ApiCatalogo.Models;

namespace ApiCatalogo.Repositories.Interface
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutosPorCategoria(int id);
    }
}
