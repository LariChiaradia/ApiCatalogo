﻿namespace ApiCatalogo.Repositories.Interface
{
    public interface IUnitOfWork
    {
        IProdutoRepository ProdutoRepository { get; }
        ICategoriaRepository CategoriaRepository { get; }
        Task CommitAsync();
    }
}
