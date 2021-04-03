using FluentValidation;
using Grpc.Core;
using Liquid.Core.Context;
using Liquid.Core.Exceptions;
using Liquid.Core.Localization;
using Liquid.Core.Telemetry;
using Liquid.Core.Utils;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Liquid.WebApi.Grpc
{
    /// <summary>
    /// Light Grpc Service Class.
    /// </summary>
    /// <seealso cref="Liquid.WebApi.Grpc.ILightGrpcService" />
    public class LightGrpcService : ILightGrpcService
    {
        private IMediator _mediator;
        private ILightContext _context;
        private ILightTelemetry _telemetry;
        private ILocalization _localization;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightGrpcService"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="context">The context.</param>
        /// <param name="telemetry">The telemetry.</param>
        /// <param name="localization">The localization.</param>
        public LightGrpcService(IMediator mediator,
                                ILightContext context,
                                ILightTelemetry telemetry,
                                ILocalization localization)
        {
            _mediator = mediator;
            _context = context;
            _telemetry = telemetry;
            _localization = localization;
        }


        /// <summary>
        /// Executes the action, sends the command to domain and returns the response .
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="request">The request command or query.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task<TResponse> ExecuteAsync<TResponse>(IRequest<TResponse> request, ServerCallContext context)
        {
            return await HandleResponseAsync(async () => await _mediator.Send(request), context);
        }

        /// <summary>
        /// Handles the request and generates the grpc response.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="requestFunction">The request function.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="RpcException"> Occurs when a specific exception has occurred during the request processing.
        /// </exception>
        public async Task<TResponse> HandleResponseAsync<TResponse>(Func<Task<TResponse>> requestFunction, ServerCallContext context)
        {
            try
            {
                var response = await requestFunction();

                var messages = _context.GetNotifications();
                foreach (var message in messages)
                {
                    context.ResponseTrailers.Add(message.Key, message.Value.ToString());
                }

                return response;
            }
            catch (ValidationException validationException)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, HandleValidationException(validationException)));
            }
            catch (LightCustomException ex)
            {
                _telemetry.AddErrorTelemetry(ex);
                throw new RpcException(new Status((StatusCode)ex.ResponseCode.Value, new { messages = _localization.Get(ex.Message) }.ToJson()));
            }
        }

        /// <summary>
        /// Handles the validation exception result.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        private string HandleValidationException(ValidationException ex)
        {
            var messages = new Dictionary<string, string>();
            var index = 0;

            ex.Errors.Each(error =>
            {
                messages.Add($"{index}_{error.PropertyName}", _localization.Get(error.ErrorMessage, _context.ContextChannel));
                index++;
            });
            return messages.ToJson();
        }
    }
}
