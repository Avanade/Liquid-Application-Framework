using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using Liquid.Core.Utils;
using Liquid.Core.Context;
using Liquid.Core.Exceptions;
using Liquid.Core.Localization;
using Liquid.Core.Telemetry;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.WebApi.Http.Controllers
{
    /// <summary>
    /// Base Controller Class.
    /// </summary>
    /// <seealso cref="Controller" />
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Gets or sets the log service.
        /// </summary>
        /// <value>
        /// The log service.
        /// </value>
        [ExcludeFromCodeCoverage]
        protected ILogger LogService { get; }

        /// <summary>
        /// Gets or sets the mediator service.
        /// </summary>
        /// <value>
        /// The mediator service.
        /// </value>
        [ExcludeFromCodeCoverage]
        protected IMediator Mediator { get; }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        [ExcludeFromCodeCoverage]
        protected ILightContext Context { get; }

        /// <summary>
        /// Gets the telemetry.
        /// </summary>
        /// <value>
        /// The telemetry.
        /// </value>
        [ExcludeFromCodeCoverage]
        protected ILightTelemetry Telemetry { get; }

        /// <summary>
        /// Gets the localization service.
        /// </summary>
        /// <value>
        /// The localization service.
        /// </value>
        [ExcludeFromCodeCoverage]
        protected ILocalization Localization { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController" /> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="mediator">The mediator service.</param>
        /// <param name="context">The current context.</param>
        /// <param name="telemetry">The current telemetry.</param>
        /// <param name="localization">The localization service.</param>
        protected BaseController(ILoggerFactory loggerFactory,
                                 IMediator mediator,
                                 ILightContext context,
                                 ILightTelemetry telemetry,
                                 ILocalization localization)
        {
            LogService = loggerFactory.CreateLogger(GetType().Name);
            Mediator = mediator;
            Context = context;
            Telemetry = telemetry;
            Localization = localization;
        }
        
        /// <summary>
        /// Executes the action and returns the response using http response code 200 (Success).
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="request">The request command or query.</param>
        /// <returns></returns>
        protected virtual async Task<IActionResult> ExecuteAsync<TRequest>(IRequest<TRequest> request)
        {
            return await HandleResponseAsync(async () => await Mediator.Send(request), HttpStatusCode.OK);
        }

        /// <summary>
        /// Executes the action and returns the response using a custom http response code.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="request">The request command or query.</param>
        /// <param name="responseCode">The http response code.</param>
        /// <returns></returns>
        protected virtual async Task<IActionResult> ExecuteAsync<TRequest>(IRequest<TRequest> request, HttpStatusCode responseCode)
        {
            return await HandleResponseAsync(async () => await Mediator.Send(request), responseCode);
        }

        /// <summary>
        /// Handles the request and generates the http response.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="requestFunction">The request function.</param>
        /// <param name="responseCode">The response code.</param>
        /// <returns></returns>
        protected virtual async Task<IActionResult> HandleResponseAsync<TResponse>(Func<Task<TResponse>> requestFunction, HttpStatusCode responseCode)
        {
            try
            {
                var response = await requestFunction();
                var messages = Context.GetNotifications();
                return messages.Any() ? StatusCode((int) responseCode, new {response, messages}) : StatusCode((int) responseCode, new {response});
            }
            catch (ValidationException validationException)
            {
                return HandleValidationException(validationException);
            }
            catch (LightCustomException ex)
            {
                Telemetry.AddErrorTelemetry(ex);
                return StatusCode(ex.ResponseCode.Value, new { messages = Localization.Get(ex.Message) });
            }
        }

        /// <summary>
        /// Handles the validation exception result.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        private IActionResult HandleValidationException(ValidationException ex)
        {
            var messages = new Dictionary<string, string>();
            var index = 0;
            
            ex.Errors.Each(error =>
            {
                messages.Add($"{index}_{error.PropertyName}", Localization.Get(error.ErrorMessage, Context.ContextChannel));
                index++;
            });
            return StatusCode((int)HttpStatusCode.BadRequest, new { messages });
        }
    }
}