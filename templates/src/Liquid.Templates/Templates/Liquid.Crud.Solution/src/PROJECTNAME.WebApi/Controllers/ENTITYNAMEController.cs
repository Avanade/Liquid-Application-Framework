using Liquid.WebApi.Http.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PROJECTNAME.Domain.Entities;
using PROJECTNAME.Domain.Handlers.ENTITYNAME.Create;
using PROJECTNAME.Domain.Handlers.ENTITYNAME.Remove;
using PROJECTNAME.Domain.Handlers.ENTITYNAME.Read;
using PROJECTNAME.Domain.Handlers.ENTITYNAME.Update;
using System.Net;
using System.Threading.Tasks;
using PROJECTNAME.Domain.Handlers.ENTITYNAME.List;

namespace PROJECTNAME.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ENTITYNAMEController : LiquidControllerBase
    {
        public ENTITYNAMEController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id) => await ExecuteAsync(new ReadENTITYNAMEQuery(id), HttpStatusCode.OK);

        [HttpGet]
        public async Task<IActionResult> Get() => await ExecuteAsync(new ListENTITYNAMEQuery(), HttpStatusCode.OK);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ENTITYNAMEEntity entity) => await ExecuteAsync(new CreateENTITYNAMECommand(entity), HttpStatusCode.Created);

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ENTITYNAMEEntity entity) => await ExecuteAsync(new UpdateENTITYNAMECommand(entity), HttpStatusCode.OK);

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id) => await ExecuteAsync(new RemoveENTITYNAMECommand(id), HttpStatusCode.OK);
    }
}
