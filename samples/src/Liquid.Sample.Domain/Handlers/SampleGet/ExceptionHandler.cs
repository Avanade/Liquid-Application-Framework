using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MediatR.Pipeline;

namespace Liquid.Sample.Domain.Handlers.SampleGet
{
    public class ExceptionHandler : IRequestExceptionHandler<SampleRequest, SampleResponse, Exception>
    {
        private readonly IMediator _mediator;

        public ExceptionHandler(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public Task Handle(SampleRequest request, Exception exception, RequestExceptionHandlerState<SampleResponse> state, CancellationToken cancellationToken)
        {
            _mediator.Publish<>
        }
    }
}
