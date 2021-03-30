using System.Threading;
using System.Threading.Tasks;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using MediatR.Pipeline;

namespace Liquid.Domain.Pipelines
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <seealso cref="MediatR.Pipeline.IRequestPostProcessor{TRequest, TResponse}" />
    public class TelemetryRequestPostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
    {
        private readonly ILightTelemetry _telemetry;
        private readonly ILightContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryRequestPreProcessor{TRequest}" /> class.
        /// </summary>
        /// <param name="telemetry">The telemetry.</param>
        /// <param name="context">The context.</param>
        public TelemetryRequestPostProcessor(ILightTelemetry telemetry,
                                             ILightContext context)
        {
            _telemetry = telemetry;
            _context = context;
        }

        /// <summary>
        /// Processes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                _telemetry.CollectTelemetryStopWatchMetric($"{request.GetType().Name}_RequestTime", _context.GetNotifications());
                _telemetry.RemoveContext(request.GetType().Name);

            }, cancellationToken);
        }
    }
}