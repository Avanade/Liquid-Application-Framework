using Liquid.Core.Interfaces;
using Liquid.Core.Settings;
using Liquid.Messaging.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging.Decorators
{
    /// <summary>
    /// Inserts configured context keys in ILogger service scope.
    /// Includes its behavior in messaging pipelines before process execution.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LiquidScopedLoggingDecorator : ILiquidPipeline
    {
        private readonly ILogger<LiquidScopedLoggingDecorator> _logger;
        private readonly ILiquidConfiguration<ScopedLoggingSettings> _options;
        private readonly ILiquidPipeline _inner;

        /// <summary>
        /// Initialize a new instance of <see cref="LiquidScopedLoggingDecorator"/>
        /// </summary>
        /// <param name="inner">Decorated service.</param>
        /// <param name="options">Default culture configuration.</param>
        /// <param name="logger">Logger service instance.</param>
        public LiquidScopedLoggingDecorator(ILiquidPipeline inner, ILiquidConfiguration<ScopedLoggingSettings> options, ILogger<LiquidScopedLoggingDecorator> logger)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        ///<inheritdoc/>
        public async Task Execute<T>(ProcessMessageEventArgs<T> message, Func<ProcessMessageEventArgs<T>, CancellationToken, Task> process, CancellationToken cancellationToken)
        {
            var scope = new List<KeyValuePair<string, object>>();

            foreach (var key in _options.Settings.Keys)
            {
                message.Headers.TryGetValue(key.KeyName, out object value);

                if (value is null && key.Required)                    
                    throw new MessagingMissingScopedKeysException(key.KeyName);

                scope.Add(new KeyValuePair<string, object>(key.KeyName, value));
            }

            using (_logger.BeginScope(scope.ToArray()))
            {
                await _inner.Execute(message, process, cancellationToken);
            }
        }
    }
}
