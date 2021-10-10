using Liquid.Core.Interfaces;
using Liquid.Core.Settings;
using Liquid.Messaging.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging.Decorators
{
    /// <summary>
    /// Configures the culture in the current thread.
    /// Includes its behavior in worker service before process execution.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LiquidCultureDecorator<TEntity> : ILiquidWorker<TEntity>
    {
        private const string _culture = "culture";
        private readonly ILiquidConfiguration<CultureSettings> _options;
        private readonly ILiquidWorker<TEntity> _inner;

        /// <summary>
        /// Initialize a new instance of <see cref="LiquidCultureDecorator{TEntity}"/>
        /// </summary>
        /// <param name="inner">Decorated service.</param>
        /// <param name="options">Default culture configuration.</param>
        public LiquidCultureDecorator(ILiquidWorker<TEntity> inner, ILiquidConfiguration<CultureSettings> options)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        ///<inheritdoc/>
        public async Task ProcessMessageAsync(ProcessMessageEventArgs<TEntity> args, CancellationToken cancellationToken)
        {
            args.Headers.TryGetValue(_culture, out object cultureCode);

            if (cultureCode is null && !string.IsNullOrEmpty(_options.Settings.DefaultCulture))
            {
                cultureCode = _options.Settings.DefaultCulture;
            }

            if (cultureCode != null)
            {
                CultureInfo.CurrentCulture = new CultureInfo(cultureCode.ToString());
                CultureInfo.CurrentUICulture = new CultureInfo(cultureCode.ToString());
            }

            await _inner.ProcessMessageAsync(args, cancellationToken);
        }
    }
}
