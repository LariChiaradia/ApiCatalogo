using ApiCatalogo.Controllers;
using ApiCatalogo.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCatalogoxUnitTests.UnitTests
{
    public class PutProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutosController _controller;

        public PutProdutoUnitTests(ProdutosUnitTestController controller)
        {
            _controller = new ProdutosController(controller.repository, controller.mapper);
        }

        [Fact]
        public async Task PutProduto_Return_OkResult()
        {
            //Arrange
            var prodId = 14;

            var updateProdutoDTO = new ProdutoDTO
            {
                ProdutoId = prodId,
                Nome = "Novo Produto Atualizado",
                Descricao = "Descrição do Novo Produto Atualizado - Testes",
                ImageUrl = "imagem1.jpg",
                CategoriaId = 2
            };

            //Act
            var result = await _controller.Put(prodId, updateProdutoDTO) as ActionResult<ProdutoDTO>; 

            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task PutProduto_Return_BadRequest()
        {
            //Arrange
            var prodId = 17;

            var updateProdutoDTO = new ProdutoDTO
            {
                ProdutoId = 14,
                Nome = "Novo Produto Atualizado",
                Descricao = "Descrição do Novo Produto Atualizado - Testes",
                ImageUrl = "imagem1.jpg",
                CategoriaId = 2
            };

            //Act
            var data = await _controller.Put(prodId, updateProdutoDTO);

            //Assert
            data.Result.Should().BeOfType<BadRequestResult>().Which.StatusCode.Should().Be(400) ;
        }
    }
}
