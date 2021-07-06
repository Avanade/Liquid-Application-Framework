using Liquid.WebApi.Http.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.WebApi.Http.UnitTests.Mocks
{
    [ExcludeFromCodeCoverage]
    public class TestController : LiquidControllerBase
    {
        public TestController(IMediator mediator) : base(mediator)
        {
        }

        
        public async Task<IActionResult> GetCase1() => await ExecuteAsync(new TestCaseRequest(), HttpStatusCode.OK);

        public async Task<IActionResult> GetCase2() => Ok(await ExecuteAsync(new TestCaseRequest()));

    }
}
