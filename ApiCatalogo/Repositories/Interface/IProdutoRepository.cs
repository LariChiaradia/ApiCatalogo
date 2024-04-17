using ApiCatalogo.Models;

namespace ApiCatalogo.Repositories.Interface
{
    public interface IProdutoRepository
    {
        IEnumerable<Produto> GetProdutosPorCategoria(int id);
    }
}
