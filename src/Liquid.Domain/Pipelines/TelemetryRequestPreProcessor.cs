using System.Threading;
using System.Threading.Tasks;
using Liquid.Core.Telemetry;
using MediatR.Pipeline;

namespace Liquid.Domain.Pipelines
{
    /// <summary>
    /// Telemetry Request Pre Processor Class. Starts a new telemetry context and starts measuring the execution time of request.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <seealso cref="MediatR.Pipeline.IRequestPreProcessor{TRequest}" />
    public class TelemetryRequestPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILightTelemetry _telemetry;

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryRequestPreProcessor{TRequest}"/> class.
        /// </summary>
        /// <param name="telemetry">The telemetry.</param>
        public TelemetryRequestPreProcessor(ILightTelemetry telemetry)
        {
            _telemetry = telemetry;
        }


        /// <summary>
        /// Processes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                _telemetry.EnqueueContext(request.GetType().Name);
                _telemetry.StartTelemetryStopWatchMetric("RequestTime");
            }, cancellationToken);
        }
    }
}