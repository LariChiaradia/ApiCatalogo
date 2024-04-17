using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Repositories.Interface;

namespace ApiCatalogo.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Produto> GetProdutos()
        {
            throw new NotImplementedException();
        }

        public Produto GetProduto(int id)
        {
            throw new NotImplementedException();
        }

        public Produto Create(Produto produto)
        {
            throw new NotImplementedException();
        }

        public Produto Update(Produto produto)
        {
            throw new NotImplementedException();
        }

        public Produto Delete(int id)
        {
            throw new NotImplementedException();
        }

    }
}
