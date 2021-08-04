using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;

namespace Liquid.WebApi.Http.Controllers
{
    /// <summary>
    /// Base Controller Class.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    public abstract class LiquidControllerBase : ControllerBase
    {
        /// <summary>
        /// Gets or sets the mediator service.
        /// </summary>
        /// <value>
        /// The mediator service.
        /// </value>
        [ExcludeFromCodeCoverage]
        protected IMediator Mediator { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LiquidControllerBase" /> class.
        /// </summary>
        /// <param name="mediator">The mediator service.</param>
        protected LiquidControllerBase(IMediator mediator)
        {
            Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        

        /// <summary>
        /// Executes the action <see cref="Mediator.Send{TResponse}(IRequest{TResponse}, System.Threading.CancellationToken)"/>.
        /// </summary>
        /// <param name="request">The request command or query.</param>
        protected virtual async Task<TResponse> ExecuteAsync<TResponse>(IRequest<TResponse> request)
        {
            return await Mediator.Send(request);
        }

        /// <summary>
        /// Executes the action and returns the response using a custom http response code.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="request">The request command or query.</param>
        /// <param name="responseCode">The http response code.</param>
        protected virtual async Task<IActionResult> ExecuteAsync<TRequest>(IRequest<TRequest> request, HttpStatusCode responseCode)
        {
            var response =  await Mediator.Send(request);

            return StatusCode((int)responseCode, response);
        }  
    }
}