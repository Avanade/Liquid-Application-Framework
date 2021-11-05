using Grpc.Core;
using Liquid.Services.Configuration;
using Liquid.Services.ResilienceHandlers;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;

namespace Liquid.Services.Grpc.ResilienceHandlers
{
    /// <summary>
    /// Specific resilience handler for GRPC calls.
    /// </summary>
    /// <seealso cref="Liquid.Services.ResilienceHandlers.ResilienceHandler" />
    public class GrpcResilienceHandler : ResilienceHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrpcResilienceHandler"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="logger">The logger.</param>
        public GrpcResilienceHandler(LightServiceSetting settings, ILogger logger) : base(settings, logger)
        {
        }

        /// <summary>
        /// Configures the policies into Resilience Handler.
        /// </summary>
        /// <param name="policies">The policies.</param>
        protected override void AddPoliciesToResilienceHandler(params IAsyncPolicy[] policies)
        {
            //Ignore base policies parameter and create a new list only with RpcException.
            var policiesList = new List<IAsyncPolicy>();

            var cbSettings = ServiceSettings?.Resilience?.CircuitBreaker;
            var retrySettings = ServiceSettings?.Resilience?.Retry;

            if (cbSettings == null && retrySettings == null) return;

            if (cbSettings?.Enabled == true)
            {
                var circuitBreakerPolicy = Policy
                    .Handle<RpcException>(ex => ex?.StatusCode == StatusCode.DataLoss || ex?.StatusCode == StatusCode.Internal ||
                                                ex?.StatusCode == StatusCode.Unavailable || ex?.StatusCode == StatusCode.Unimplemented)
                    .AdvancedCircuitBreakerAsync(cbSettings.FailureThreshold,
                        TimeSpan.FromSeconds(cbSettings.SamplingDuration),
                        cbSettings.MinimumThroughput,
                        TimeSpan.FromSeconds(cbSettings.DurationOfBreak),
                        (exception, state, duration, context) => { Logger.LogInformation($"Circuit breaker {context.OperationKey} is open for the next {duration.TotalMilliseconds}ms. Exception: {exception}"); },
                        context => { Logger.LogInformation($"Circuit breaker {context.OperationKey} is closed for normal operation"); },
                        () => { Logger.LogInformation("Circuit breaker is under trial state."); });
                policiesList.Add(circuitBreakerPolicy);
            }

            if (retrySettings?.Enabled == true)
            {
                var retryPolicy = Policy
                    .Handle<RpcException>(ex => ex?.StatusCode == StatusCode.DataLoss || ex?.StatusCode == StatusCode.Internal ||
                                                ex?.StatusCode == StatusCode.Unavailable || ex?.StatusCode == StatusCode.Unimplemented)
                    .WaitAndRetryAsync(retrySettings.Attempts, attempt => TimeSpan.FromMilliseconds(retrySettings.WaitDuration),
                        (exception, duration, attempt, context) => { Logger.LogInformation($"Retrying {attempt} attempt operation {context.OperationKey} in {duration.TotalMilliseconds}ms. Exception: {exception}"); });
                policiesList.Add(retryPolicy);
            }

            base.AddPoliciesToResilienceHandler(policiesList.ToArray());
        }
    }
}