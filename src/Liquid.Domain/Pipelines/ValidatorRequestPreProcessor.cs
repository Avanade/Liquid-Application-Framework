using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR.Pipeline;

namespace Liquid.Domain.Pipelines
{
    /// <summary>
    /// Validator request behavior pipeline class. Executes this behavior before each call of TRequestHandler. Calls the correspondent AbstractValidator
    /// Class and validates the command before enters in the Command/Query handler.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <seealso cref="MediatR.IPipelineBehavior{TRequest, TResponse}" />
    public class ValidatorRequestPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatorRequestPreProcessor{TRequest}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public ValidatorRequestPreProcessor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Process method executes before calling the Handle method on your handler
        /// </summary>
        /// <param name="request">Incoming request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var validator = await Task.Run(() => _serviceProvider.GetService(typeof(IValidator<TRequest>)), cancellationToken);
            
            if (validator != null)
            {
                var failures = ((IValidator<TRequest>)validator).Validate(request).Errors;
                if (failures.Any())
                {
                    throw new ValidationException(failures);
                }
            }
        }
    }
}