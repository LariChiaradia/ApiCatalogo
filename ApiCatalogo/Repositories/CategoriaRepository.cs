using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {

        public CategoriaRepository(AppDbContext context) : base(context)
        {
        }
    }
}
