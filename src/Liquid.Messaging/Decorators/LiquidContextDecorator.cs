using Liquid.Core.Interfaces;
using Liquid.Core.Settings;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.Interfaces;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging.Decorators
{
    /// <summary>
    /// Inserts configured context keys in LiquidContext service.
    /// Includes its behavior in worker service before process execution.
    /// </summary>
    public class LiquidContextDecorator<TEntity> : ILiquidWorker<TEntity>
    {
        private readonly ILiquidWorker<TEntity> _inner;
        private readonly ILiquidContext _context;
        private readonly ILiquidConfiguration<ScopedContextSettings> _options;

        /// <summary>
        /// Initialize a new instance of <see cref="LiquidContextDecorator{TEntity}"/>
        /// </summary>
        /// <param name="inner">Decorated service.</param>
        /// <param name="context">Scoped Context service.</param>
        /// <param name="options">Scoped context keys set.</param>
        public LiquidContextDecorator(ILiquidWorker<TEntity> inner, ILiquidContext context, ILiquidConfiguration<ScopedContextSettings> options)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        ///<inheritdoc/>
        public async Task ProcessMessageAsync(ConsumerMessageEventArgs<TEntity> args, CancellationToken cancellationToken)
        {
            object value = default; 

            foreach (var key in _options.Settings.Keys)
            {
                args.Headers?.TryGetValue(key.KeyName, out value);

                if (value is null && key.Required)
                    throw new MessagingMissingContextKeysException(key.KeyName);

                _context.Upsert(key.KeyName, value);
            }

            if (_options.Settings.Culture)
            {
                _context.Upsert("culture", CultureInfo.CurrentCulture.Name);
            }

            await _inner.ProcessMessageAsync(args, cancellationToken);
        }
    }
}
