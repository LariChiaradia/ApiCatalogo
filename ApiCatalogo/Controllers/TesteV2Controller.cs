﻿using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers
{
    [Route("api/v{version:apiVersion}/teste")]
    [ApiController]
    [ApiVersion("2.0")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TesteV2Controller : ControllerBase
    {
        [HttpGet]
        public string GetVersion2()
        {
            return "TesteV2 - GET - Api versão 2.0";
        }
    }
}
