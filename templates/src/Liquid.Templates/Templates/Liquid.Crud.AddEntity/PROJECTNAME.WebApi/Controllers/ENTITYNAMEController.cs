using Liquid.WebApi.Http.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PROJECTNAME.Domain.Entities;
using PROJECTNAME.Domain.Handlers.ENTITYNAME.Post;
using PROJECTNAME.Domain.Handlers.ENTITYNAME.Delete;
using PROJECTNAME.Domain.Handlers.ENTITYNAME.GetById;
using PROJECTNAME.Domain.Handlers.ENTITYNAME.Put;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id) => await ExecuteAsync(new GetByIdENTITYNAMEQuery(id), HttpStatusCode.Created);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ENTITYNAMEEntity entity) => await ExecuteAsync(new PostENTITYNAMECommand(entity), HttpStatusCode.OK);

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ENTITYNAMEEntity entity) => await ExecuteAsync(new PutENTITYNAMECommand(entity), HttpStatusCode.OK);

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id) => await ExecuteAsync(new DeleteENTITYNAMECommand(id), HttpStatusCode.OK);
    }
}
