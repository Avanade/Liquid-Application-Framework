using Liquid.WebApi.Http.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PROJECTNAME.Domain.Entities;
using PROJECTNAME.Domain.Handlers.ENTITYNAME.Create;
using PROJECTNAME.Domain.Handlers.ENTITYNAME.Remove;
using PROJECTNAME.Domain.Handlers.ENTITYNAME.Read;
using PROJECTNAME.Domain.Handlers.ENTITYNAME.Update;
using PROJECTNAME.Domain.Handlers.ENTITYNAME.List;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;

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
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<IActionResult> Get([FromRoute] ENTITYIDTYPE id)
        {
            var response = await ExecuteAsync(new ReadENTITYNAMEQuery(id));

            if (response.Data == null) return NotFound();

            return Ok(response.Data);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var response = await ExecuteAsync(new ListENTITYNAMEQuery());

            if (response.Data == null) return NotFound();

            return Ok(response.Data);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] ENTITYNAMEEntity entity)
        {
            await ExecuteAsync(new CreateENTITYNAMECommand(entity));

            return CreatedAtRoute("", new { id = entity.Id }, entity);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Put([FromBody] ENTITYNAMEEntity entity)
        {
            var response = await ExecuteAsync(new UpdateENTITYNAMECommand(entity));

            if (response.Data == null) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete([FromRoute] ENTITYIDTYPE id)
        {
            var response = await ExecuteAsync(new RemoveENTITYNAMECommand(id));

            if (response.Data == null) return NotFound();

            return NoContent();
        }
    }
}