﻿using ApiCatalogo.Context;
using ApiCatalogo.DTOs;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositories.Interface;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using X.PagedList;

namespace ApiCatalogo.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiVersion("1.0")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    [Route("api/{v:apiVersion}/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet("produtos/{id}")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPorCategoria(int id)
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosPorCategoriaAsync(id);
            if (produtos is null)
            {
                return NotFound("Produtos não encontrados");
            }

            var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDTO);
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get([FromQuery] ProdutosParameters produtosParameters)
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosAsync(produtosParameters);
            return ObterProdutos(produtos);
        }

        private ActionResult<IEnumerable<ProdutoDTO>> ObterProdutos(IPagedList<Produto> produtos)
        {
            var metadata = new
            {
                produtos.Count,
                produtos.PageSize,
                produtos.PageCount,
                produtos.TotalItemCount,
                produtos.HasNextPage,
                produtos.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            return Ok(produtosDTO);
        }

        [HttpGet("filter/preco/paginaton")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosFilterPreco([FromQuery] ProdutosFiltroPreco produtosFiltroParameters)
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosFiltroPrecoAsync(produtosFiltroParameters);
            return ObterProdutos(produtos);
        }

        /// <summary>
        /// Exibe uma relação dos produtos
        /// </summary>
        /// <returns>Retorna uma lista de objetos Produto</returns>
        //[Authorize(Policy ="UserOnly")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get()
        {
            try
            {
                var produtos = await _uof.ProdutoRepository.GetAllAsync();

                if (produtos is null)
                {
                    return NotFound("Produtos não encontrados");
                }

                var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

                return Ok(produtosDTO);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Obtem o produto pelo seu identificador produtoId
        /// </summary>
        /// <param name="id">Código do produto</param>
        /// <returns>Um objeto Produto</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [HttpGet("{id:int}", Name="ObterProduto")]
        public async Task<ActionResult<ProdutoDTO>> Get(int id)
        {
            if(id == null || id <= 0)
            {
                return BadRequest("ID de produto inválido");
            }

            var produto = await _uof.ProdutoRepository.GetAsync(p => p.ProdutoId == id);
            if(produto is null)
            {
                return NotFound("Produto não encontrado");
            }

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return Ok(produtoDTO);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesDefaultResponseType]
        public async Task<ActionResult<ProdutoDTO>> Post(ProdutoDTO produtoDTO)
        {
            if (produtoDTO is null)
            {
                return BadRequest();
            }

            var produto = _mapper.Map<Produto>(produtoDTO);

            var novoProduto = _uof.ProdutoRepository.Create(produto);
            await _uof.CommitAsync();

            var novoProdutoDTO = _mapper.Map<ProdutoDTO>(novoProduto);

            return new CreatedAtRouteResult("ObterProduto",
                new {id = novoProdutoDTO.ProdutoId}, novoProdutoDTO); 
        }

        [HttpPatch("{id}/UpdatePartial")]
        public async Task<ActionResult<ProdutoDTOUpdateResponse>> Patch(int id,
            JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDTO)
        {
            if(patchProdutoDTO is null || id <= 0)
            {
                return BadRequest();
            }

            var produto = await _uof.ProdutoRepository.GetAsync(p => p.ProdutoId == id);

            if(produto is null)
            {
                return NotFound();
            }

            var produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);

            patchProdutoDTO.ApplyTo(produtoUpdateRequest,ModelState);

            if (!ModelState.IsValid || !TryValidateModel(produtoUpdateRequest))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(produtoUpdateRequest, produto);

            _uof.ProdutoRepository.Update(produto);
            await _uof.CommitAsync();

            return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ProdutoDTO>> Put(int id, ProdutoDTO produtoDTO) 
        { 
            if(id != produtoDTO.ProdutoId)
            {
                return BadRequest();
            }

            var produto = _mapper.Map<Produto>(produtoDTO);

            var produtoAtualizado = _uof.ProdutoRepository.Update(produto);
            await _uof.CommitAsync();

            var produtoDTOAtualizado = _mapper.Map<ProdutoDTO>(produtoAtualizado);

            return Ok(produtoDTOAtualizado);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ProdutoDTO>> Delete(int id)
        {
            var produto = await _uof.ProdutoRepository.GetAsync(p => p.ProdutoId == id);

            if (produto is null)
            {
                return NotFound("Produto não encontrado");
            }
            
            var produtoDeletado = _uof.ProdutoRepository.Delete(produto);
            await _uof.CommitAsync();

            var produtoDTODeletado = _mapper.Map<ProdutoDTO>(produtoDeletado);

            return Ok(produtoDTODeletado);
        }
    }
}