﻿using Liquid.WebApi.Http.Controllers;
using Liquid.WebApi.Http.Interfaces;
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
    public class TestNotificationController(IMediator mediator, ILiquidNotificationHelper liquidNotification) : LiquidControllerBase(mediator)
    {
        private readonly ILiquidNotificationHelper _liquidNotification = liquidNotification;

        public async Task<IActionResult> GetCase2() => Ok(await ExecuteAsync(new TestCaseRequest()));

    }
}
