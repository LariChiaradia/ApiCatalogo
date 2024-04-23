using ApiCatalogo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Context
{
    //É responsável por realizar a comunicação entre as entidades e o banco de dados relacional (Mapeamento)
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Categoria>? Categorias { get; set; }
        public DbSet<Produto>? Produtos { get; set; }
    }
}
