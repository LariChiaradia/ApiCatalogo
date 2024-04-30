using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers
{
    [Route("api/teste")]
    [ApiController]
    [ApiVersion(3)]
    [ApiVersion(4)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TesteV3Controller : ControllerBase
    {
        [MapToApiVersion(3)]
        [HttpGet]
        public string GetVersion3()
        {
            return "TesteV2 - GET - Api versão 3.0";
        }
        [MapToApiVersion(4)]
        [HttpGet]
        public string GetVersion4()
        {
            return "TesteV2 - GET - Api versão 4.0";
        }
    }
}
