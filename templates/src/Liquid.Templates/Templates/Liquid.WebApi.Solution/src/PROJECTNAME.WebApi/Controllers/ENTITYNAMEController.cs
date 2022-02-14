using Liquid.WebApi.Http.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PROJECTNAME.Domain.Entities;
using PROJECTNAME.Domain.Handlers.ENTITYNAME.COMMANDNAME;
using System.Net;
using System.Threading.Tasks;

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
        public async Task<IActionResult> COMMANDNAME([FromBody] ENTITYNAMEEntity entity) => await ExecuteAsync(new COMMANDNAMEENTITYNAMECommand(entity), HttpStatusCode.OK);

    }
}
