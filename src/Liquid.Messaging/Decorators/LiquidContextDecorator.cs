using Liquid.Core.Interfaces;
using Liquid.Core.Settings;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging.Decorators
{
    /// <summary>
    /// Inserts configured context keys in LiquidContext service.
    /// Includes its behavior in messaging pipelines before process execution.
    /// </summary>
    public class LiquidContextDecorator : ILiquidPipeline
    {
        private readonly ILiquidPipeline _inner;
        private readonly ILiquidContext _context;
        private readonly ILiquidConfiguration<ScopedContextSettings> _options;

        /// <summary>
        /// Initialize a new instance of <see cref="LiquidContextDecorator"/>
        /// </summary>
        /// <param name="inner">Decorated service.</param>
        /// <param name="context">Scoped Context service.</param>
        /// <param name="options">Scoped context keys set.</param>
        public LiquidContextDecorator(ILiquidPipeline inner, ILiquidContext context, ILiquidConfiguration<ScopedContextSettings> options)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }
        ///<inheritdoc/>
        public async Task Execute<T>(ProcessMessageEventArgs<T> message, Func<ProcessMessageEventArgs<T>, CancellationToken, Task> process, CancellationToken cancellationToken)
        {

            object value = default;

            foreach (var key in _options.Settings.Keys)
            {
                message.Headers.TryGetValue(key.KeyName, out value);

                if (value is null && key.Required)
                    //TODO: build a custom exception
                    throw new Exception(key.KeyName);

                _context.Upsert(key.KeyName, value);
            }

            if (_options.Settings.Culture)
            {
                _context.Upsert("culture", CultureInfo.CurrentCulture.Name);
            }

            await _inner.Execute(message, process, cancellationToken);
        }
    }
}
