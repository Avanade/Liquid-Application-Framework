using Liquid.Sample.Domain.Entities;
using Liquid.Sample.Domain.Handlers;
using Liquid.WebApi.Http.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Liquid.Sample.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SampleController : LiquidControllerBase
    {
        public SampleController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("Sample")]
        public async Task<IActionResult> Get([FromQuery] string id) => await ExecuteAsync(new SampleRequest(id), HttpStatusCode.OK);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SampleMessageEntity entity)
        {
            await Mediator.Send(new SampleEventRequest(entity));

            return NoContent();
        }
    }
}
