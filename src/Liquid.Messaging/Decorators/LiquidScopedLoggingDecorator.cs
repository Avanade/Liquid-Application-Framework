using Liquid.Core.Interfaces;
using Liquid.Core.Settings;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging.Decorators
{
    /// <summary>
    /// 
    /// </summary>
    public class LiquidScopedLoggingDecorator : ILiquidPipeline
    {
        private readonly ILogger<LiquidScopedLoggingDecorator> _logger;
        private readonly ILiquidConfiguration<ScopedLoggingSettings> _options;
        private readonly ILiquidPipeline _inner;

        public LiquidScopedLoggingDecorator(ILiquidPipeline inner, ILiquidConfiguration<ScopedLoggingSettings> options, ILogger<LiquidScopedLoggingDecorator> logger)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));            
        }

        public async Task Execute<T>(ProcessMessageEventArgs<T> message, Func<ProcessMessageEventArgs<T>, CancellationToken, Task> process, CancellationToken cancellationToken)
        {
            var scope = new List<KeyValuePair<string, object>>();

            foreach (var key in _options.Settings.Keys)
            {
                message.Headers.TryGetValue(key.KeyName, out object value);
                
                if (value is null && key.Required)
                    //TODO: Custom Exception.
                    throw new Exception(key.KeyName);

                scope.Add(new KeyValuePair<string, object>(key.KeyName, value));
            }

            using (_logger.BeginScope(scope.ToArray()))
            {
                await _inner.Execute(message, process, cancellationToken);
            }
        }
    }
}
