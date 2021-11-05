using Liquid.Services.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Wrap;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Liquid.Services.ResilienceHandlers
{
    /// <summary>
    /// Resilience Handler class.
    /// </summary>
    public class ResilienceHandler : IResilienceHandler
    {
        private IAsyncPolicy _policy;
        private AsyncPolicyWrap _policyWrap;
        private bool _enabled;

        /// <summary>
        /// The service settings
        /// </summary>
        protected LightServiceSetting ServiceSettings { get; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        protected ILogger Logger { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResilienceHandler" /> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">
        /// settings
        /// or
        /// logger
        /// </exception>
        public ResilienceHandler(LightServiceSetting settings, ILogger logger)
        {
            ServiceSettings = settings ?? throw new ArgumentNullException(nameof(settings));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            InitializePolicies();
        }

        /// <summary>
        /// Initializes the policies.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private void InitializePolicies()
        {
            if (ServiceSettings.Resilience == null) return;

            if (ServiceSettings.Resilience.Retry == null && ServiceSettings.Resilience.CircuitBreaker == null) return;

            var policies = new List<IAsyncPolicy>();
            var cbSettings = ServiceSettings?.Resilience?.CircuitBreaker;
            if (cbSettings?.Enabled == true)
            {
                var circuitBreakerPolicy = Policy
                    .Handle<CommunicationException>()
                    .Or<TimeoutException>()
                    .Or<AggregateException>()
                    .Or<TaskCanceledException>()
                    .Or<HttpRequestException>()
                    .Or<SocketException>()
                    .Or<WebSocketException>()
                    .Or<WebException>()
                    .AdvancedCircuitBreakerAsync(cbSettings.FailureThreshold,
                        TimeSpan.FromSeconds(cbSettings.SamplingDuration),
                        cbSettings.MinimumThroughput,
                        TimeSpan.FromSeconds(cbSettings.DurationOfBreak),
                        (exception, state, duration, context) => { Logger.LogInformation($"Circuit breaker {context.OperationKey} is open for the next {duration.TotalMilliseconds}ms. Exception: {exception}"); },
                        context => { Logger.LogInformation($"Circuit breaker {context.OperationKey} is closed for normal operation"); },
                        () => { Logger.LogInformation("Circuit breaker is under trial state."); });
                policies.Add(circuitBreakerPolicy);
            }

            var retrySettings = ServiceSettings?.Resilience?.Retry;
            if (retrySettings?.Enabled == true)
            {
                var retryPolicy = Policy
                    .Handle<CommunicationException>()
                    .Or<TimeoutException>()
                    .Or<AggregateException>()
                    .Or<TaskCanceledException>()
                    .Or<HttpRequestException>()
                    .Or<SocketException>()
                    .Or<WebSocketException>()
                    .Or<WebException>()
                    .WaitAndRetryAsync(retrySettings.Attempts, attempt => TimeSpan.FromMilliseconds(retrySettings.WaitDuration),
                        (exception, duration, attempt, context) => { Logger.LogInformation($"Retrying {attempt} attempt operation {context.OperationKey} in {duration.TotalMilliseconds}ms. Exception: {exception}"); });
                policies.Add(retryPolicy);
            }

            AddPoliciesToResilienceHandler(policies.ToArray());
        }

        /// <summary>
        /// Configures the policies into Resilience Handler.
        /// </summary>
        /// <param name="policies">The policies.</param>
        protected virtual void AddPoliciesToResilienceHandler(params IAsyncPolicy[] policies)
        {
            if (!policies.Any()) return;

            _enabled = true;
            if (policies.Length > 1)
            {
                _policyWrap = Policy.WrapAsync(policies);
            }
            else
            {
                _policy = policies.First();
            }
        }

        /// <summary>
        /// Executes the action under resilience control.
        /// </summary>
        /// <param name="operation">The operation to be executed under resilience.</param>
        public async Task HandleAsync(Func<Task> operation)
        {
            if (_enabled && _policyWrap != null)
            {
                await _policyWrap.ExecuteAsync(operation);
            }
            else if (_enabled && _policy != null)
            {
                await _policy.ExecuteAsync(operation);
            }
            else
            {
                await operation.Invoke();
            }
        }
    }
}