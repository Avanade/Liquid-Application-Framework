using Liquid.WebApi.Http.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PROJECTNAME.Domain.Entities;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using PROJECTNAME.Domain.Handlers;

namespace PROJECTNAME.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ENTITYNAMEController : LiquidControllerBase
    {
        public ENTITYNAMEController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<IActionResult> COMMANDNAME([FromBody] ENTITYNAME entity) => await ExecuteAsync(new COMMANDNAMEENTITYNAMERequest(entity), HttpStatusCode.OK);

    }
}