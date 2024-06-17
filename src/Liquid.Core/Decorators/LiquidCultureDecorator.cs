using Liquid.Core.Entities;
using Liquid.Core.Interfaces;
using Liquid.Core.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Core.Decorators
{
    /// <summary>
    /// Configures the culture in the current thread.
    /// Includes its behavior in worker service before process execution.
    /// </summary>
    public class LiquidCultureDecorator<TEntity> : ILiquidWorker<TEntity>
    {
        private const string _culture = "culture";
        private readonly IOptions<CultureSettings> _options;
        private readonly ILiquidWorker<TEntity> _inner;

        /// <summary>
        /// Initialize a new instance of <see cref="LiquidCultureDecorator{TEntity}"/>
        /// </summary>
        /// <param name="inner">Decorated service.</param>
        /// <param name="options">Default culture configuration.</param>
        public LiquidCultureDecorator(ILiquidWorker<TEntity> inner, IOptions<CultureSettings> options)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        ///<inheritdoc/>
        public async Task ProcessMessageAsync(ConsumerMessageEventArgs<TEntity> args, CancellationToken cancellationToken)
        {
            object cultureCode = default;

            args.Headers?.TryGetValue(_culture, out cultureCode);

            if (cultureCode is null && !string.IsNullOrEmpty(_options.Value.DefaultCulture))
            {
                cultureCode = _options.Value.DefaultCulture;
            }

            if (cultureCode != null)
            {
                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(cultureCode.ToString());
                CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(cultureCode.ToString());
            }

            await _inner.ProcessMessageAsync(args, cancellationToken);
        }
    }
}
