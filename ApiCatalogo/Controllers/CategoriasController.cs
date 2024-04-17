
using ApiCatalogo.Filters;
using ApiCatalogo.Models;
using ApiCatalogo.Repositories;
using ApiCatalogo.Repositories.Interface;
using ApiCatalogo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public CategoriasController(ICategoriaRepository repository, IConfiguration configuration,
            ILogger<CategoriasController> logger)
        {
            _repository = repository;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            _logger.LogInformation("========================== GET api/categorias/produtos =======================");
                var listaProdutos = _repository.GetCategoriasProdutos();
                return Ok(listaProdutos);
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            _logger.LogInformation("========================== GET api/categorias =======================");

            var categorias = _repository.GetCategorias();
            return Ok(categorias);
        }

        [HttpGet("{id:int}", Name ="ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {

                var categoria = _repository.GetCategoria(id);

                _logger.LogInformation($"==========================GET api/categorias/id = {id} =======================");
                
                if (categoria is null)
                {
                    _logger.LogWarning($"Categoria com id == {id} não foi localizada.");
                    return NotFound($"Categoria com o id = {id} não foi localizada.");
                }
                return Ok(categoria);
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if (categoria is null)
            {
                _logger.LogWarning("Dados inválidos.");
                return BadRequest("Dados inválidos");
            }

            var categoriaCriada = _repository.Create(categoria);

            return new CreatedAtRouteResult("ObterCategoria", new {id = categoriaCriada.CategoriaId}, categoriaCriada);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if(id != categoria.CategoriaId)
            {
                _logger.LogWarning("Dados inválidos.");
                return BadRequest("Dados inválidos.");
            }

            _repository.Update(categoria);

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _repository.GetCategoria(id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id = {id} não foi localizada.");
                return NotFound($"Categoria com id = {id} não foi localizada.");
            }

            var categoriaExcluida = _repository.Delete(id);

            return Ok(categoriaExcluida);
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
