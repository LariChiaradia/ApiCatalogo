using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Categoria> GetCategoriasProdutos()
        {
            var listaProdutos = _context.Categorias.Include(p => p.Produtos).ToList();
            return listaProdutos;
        }
        public IEnumerable<Categoria> GetCategorias()
        {
            var categoria = _context.Categorias.ToList();
            return categoria;
        }
        public Categoria GetCategoria(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);
            return categoria;
        }

        public Categoria Create(Categoria categoria)
        {
            if (categoria is null)
            {
                throw new ArgumentNullException(nameof(categoria));
            }

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return categoria;
        }

        public Categoria Update(Categoria categoria)
        {
            if (categoria is null)
            {
                throw new ArgumentNullException(nameof(categoria));
            }
            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return categoria;
        }

        public Categoria Delete(int id)
        {
            var categoria = _context.Categorias.Find(id);

            if (categoria is null)
            {
               throw new ArgumentNullException(nameof(categoria));
            }

            _context.Remove(categoria);
            _context.SaveChanges();

            return categoria;
        }
    }
}
