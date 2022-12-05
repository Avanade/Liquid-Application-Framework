using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Domain.PipelineBehaviors
{
    /// <summary>
    /// Validation Request Behavior implementation for Mediator pipelines.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class LiquidValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        /// <summary>
        /// Initialize an instance of <see cref="LiquidValidationBehavior{TRequest, TResponse}"/>
        /// </summary>
        /// <param name="validators">Validator for a <typeparamref name="TRequest"/> type. </param>
        public LiquidValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        /// <summary>
        ///  Pipeline handler. Perform validation and await the next delegate.
        /// </summary>
        /// <param name="request">Incoming request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="next"> Awaitable delegate for the next action in the pipeline. Eventually this delegate 
        /// represents the handler.</param>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var results = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                var errors = results.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (errors.Count != 0)
                    throw new ValidationException(errors);
            }
            return await next();
        }
    }
}
