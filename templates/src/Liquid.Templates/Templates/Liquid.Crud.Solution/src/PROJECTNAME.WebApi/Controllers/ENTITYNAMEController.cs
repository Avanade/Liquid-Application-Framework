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

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<IActionResult> GetById([FromRoute] ENTITYIDTYPE id)
        {
            var response = await ExecuteAsync(new ReadENTITYNAMERequest(id));

            if (response.Data == null) return NotFound();

            return Ok(response.Data);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var response = await ExecuteAsync(new ListENTITYNAMERequest());

            if (response.Data == null) return NotFound();

            return Ok(response.Data);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] ENTITYNAME entity)
        {
            await Mediator.Send(new CreateENTITYNAMERequest(entity));

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Put([FromBody] ENTITYNAME entity)
        {
            var response = await ExecuteAsync(new UpdateENTITYNAMERequest(entity));

            if (response.Data == null) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete([FromRoute] ENTITYIDTYPE id)
        {
            var response = await ExecuteAsync(new RemoveENTITYNAMERequest(id));

            if (response.Data == null) return NotFound();

            return NoContent();
        }
    }
}