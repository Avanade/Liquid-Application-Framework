using Grpc.Core;
using MediatR;
using System;
using System.Threading.Tasks;


namespace Liquid.WebApi.Grpc
{
    /// <summary>
    /// Light Grpc Service Interface.
    /// </summary>
    public interface ILightGrpcService
    {
        /// <summary>
        /// Executes the action, sends the command to domain and returns the response .
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="request">The request command or query.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        Task<TResponse> ExecuteAsync<TResponse>(IRequest<TResponse> request, ServerCallContext context);

        /// <summary>
        /// Handles the request and generates the grpc response.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="requestFunction">The request function.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        Task<TResponse> HandleResponseAsync<TResponse>(Func<Task<TResponse>> requestFunction, ServerCallContext context);
    }
}
