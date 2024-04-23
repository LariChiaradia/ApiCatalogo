
using ApiCatalogo.DTOs;
using ApiCatalogo.DTOs.Mappings;
using ApiCatalogo.Filters;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositories;
using ApiCatalogo.Repositories.Interface;
using ApiCatalogo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ApiCatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly ILogger _logger;

        public CategoriasController(IUnitOfWork uof,ILogger<CategoriasController> logger)
        {
            _uof = uof;
            _logger = logger;
        }

        [HttpGet("pagination")]
        public ActionResult<IEnumerable<CategoriaDTO>> Get([FromQuery] CategoriasParameters categoriasParameters)
        {
            var categorias = _uof.CategoriaRepository.GetCategorias(categoriasParameters);

            return ObterCategorias(categorias);
        }

        private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(PagedList<Categoria> categorias)
        {
            var metadata = new
            {
                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPages,
                categorias.HasNext,
                categorias.HasPrevious
            };
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var categoriasDTO = categorias.ToCategoriaDTOList();

            return Ok(categoriasDTO);
        }

        [HttpGet("filter/nome/pagination")]
        public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasFiltradas([FromQuery] CategoriasFiltroNome categoriasFiltro)
        {
            var categoriasFiltradas = _uof.CategoriaRepository.GetCategoriasFiltroNome(categoriasFiltro);
            return ObterCategorias(categoriasFiltradas);
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            _logger.LogInformation("========================== GET api/categorias =======================");

            var categorias = _uof.CategoriaRepository.GetAll();

            var categoriasDTO = categorias.ToCategoriaDTOList();

            return Ok(categoriasDTO);
        }

        [HttpGet("{id:int}", Name ="ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {

            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

            _logger.LogInformation($"==========================GET api/categorias/id = {id} =======================");
                
                if (categoria is null)
                {
                    _logger.LogWarning($"Categoria com id == {id} não foi localizada.");
                    return NotFound($"Categoria com o id = {id} não foi localizada.");
                }

                var categoriaDTO = categoria.ToCategoriaDTO();

                return Ok(categoriaDTO);
        }

        [HttpPost]
        public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO is null)
            {
                _logger.LogWarning("Dados inválidos.");
                return BadRequest("Dados inválidos");
            }

            var categoria = categoriaDTO.ToCategoria();

            var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();

            var novaCategoriaDTO = categoria.ToCategoriaDTO();

            return new CreatedAtRouteResult("ObterCategoria", new {id = novaCategoriaDTO.CategoriaId}, novaCategoriaDTO);
        }

        [HttpPut("{id:int}")]
        public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDTO)
        {
            if(id != categoriaDTO.CategoriaId)
            {
                _logger.LogWarning("Dados inválidos.");
                return BadRequest("Dados inválidos.");
            }

            var categoria = categoriaDTO.ToCategoria();

            _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();

            var atualizadaCategoriaDTO = categoria.ToCategoriaDTO();

            return Ok(atualizadaCategoriaDTO);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id = {id} não foi localizada.");
                return NotFound($"Categoria com id = {id} não foi localizada.");
            }

            var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();

            var categoriaExcluidaDTO = categoria.ToCategoriaDTO();

            return Ok(categoriaExcluidaDTO);
        }

        //Teste
        //[HttpGet("LerArquivoConfiguracao")]
        //public string GetValores()
        //{
        //    var valor1 = _configuration["chave1"];
        //    var valor2 = _configuration["chave2"];
        //    var secao1 = _configuration["secao1:chave2"];

        //    return $"Chave1 = {valor1} \nChave2 = {valor2} \nSeção1 => Chave2 = {secao1}";
        //}


        //[HttpGet("UsandoFromServices/{nome}")]
        //public ActionResult<string> GetSaudacaoFromServices([FromServices] IMeuServico meuServico,
        //                                                     string nome)
        //{
        //    return meuServico.Saudacao(nome);
        //}

        //[HttpGet("SemUsarFromServices/{nome}")]
        //public ActionResult<string> GetSaudacaoSemFromServices(IMeuServico meuServico,
        //                                             string nome)
        //{
        //    return meuServico.Saudacao(nome);
        //}
    }
}
