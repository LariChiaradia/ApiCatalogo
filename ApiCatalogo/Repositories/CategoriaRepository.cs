﻿using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace ApiCatalogo.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {

        public CategoriaRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParameters)
        {
            var categorias = await GetAllAsync();
            var categoriasOrdenadas = categorias.OrderBy(c => c.CategoriaId).AsQueryable();
            //var resultado = PagedList<Categoria>.ToPagedList(categoriasOrdenadas, categoriasParameters.PageNumber, categoriasParameters.PageSize);
            var resultado = await categoriasOrdenadas.ToPagedListAsync(categoriasParameters.PageNumber, categoriasParameters.PageSize);

            return resultado;
        }

        public async Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasparams)
        {
            var categorias = await GetAllAsync();
            if (!string.IsNullOrEmpty(categoriasparams.Nome))
            {
                categorias = categorias.Where(c => c.Nome.Contains(categoriasparams.Nome));
            }

            //var categoriasFiltradas = IPagedList<Categoria>.ToPagedList(categorias.AsQueryable(), categoriasparams.PageNumber, categoriasparams.PageSize);
            var categoriasFiltradas = await categorias.ToPagedListAsync(categoriasparams.PageNumber, categoriasparams.PageSize);

            return categoriasFiltradas;
        }
    }
}
