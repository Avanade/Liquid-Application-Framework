using Liquid.Sample.Domain.Entities;
using Liquid.Sample.Domain.Handlers;
using Liquid.WebApi.Http.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Liquid.Sample.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SampleController : LiquidControllerBase
    {
        public SampleController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("Sample")]
        public async Task<IActionResult> Get([FromQuery] int id) => await ExecuteAsync(new SampleRequest(id), HttpStatusCode.OK);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SampleMessageEntity entity) => await ExecuteAsync(new SampleEventRequest(entity), HttpStatusCode.OK);
    }
}
