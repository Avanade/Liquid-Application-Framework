using Liquid.Core.Entities;
using Liquid.Core.Exceptions;
using Liquid.Core.Interfaces;
using Liquid.Core.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Core.Decorators
{
    /// <summary>
    /// Inserts configured context keys in ILogger service scope.
    /// Includes its behavior in worker service before process execution.
    /// </summary>
    public class LiquidScopedLoggingDecorator<TEntity> : ILiquidWorker<TEntity>
    {
        private readonly ILogger<LiquidScopedLoggingDecorator<TEntity>> _logger;
        private readonly IOptions<ScopedLoggingSettings> _options;
        private readonly ILiquidWorker<TEntity> _inner;

        /// <summary>
        /// Initialize a new instance of <see cref="LiquidScopedLoggingDecorator{TEntity}"/>
        /// </summary>
        /// <param name="inner">Decorated service.</param>
        /// <param name="options">Default culture configuration.</param>
        /// <param name="logger">Logger service instance.</param>
        public LiquidScopedLoggingDecorator(ILiquidWorker<TEntity> inner
            , IOptions<ScopedLoggingSettings> options
            , ILogger<LiquidScopedLoggingDecorator<TEntity>> logger)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        ///<inheritdoc/>
        public async Task ProcessMessageAsync(ConsumerMessageEventArgs<TEntity> args, CancellationToken cancellationToken)
        {
            var scope = new List<KeyValuePair<string, object>>();

            object value = default;

            foreach (var key in _options.Value.Keys)
            {
                args.Headers?.TryGetValue(key.KeyName, out value);

                if (value is null && key.Required)
                    throw new MessagingMissingScopedKeysException(key.KeyName);

                scope.Add(new KeyValuePair<string, object>(key.KeyName, value));
            }

            using (_logger.BeginScope(scope.ToArray()))
            {
                await _inner.ProcessMessageAsync(args, cancellationToken);
            }
        }
    }
}
