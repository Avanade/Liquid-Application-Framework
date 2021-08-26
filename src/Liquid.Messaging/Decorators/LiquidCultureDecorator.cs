using Liquid.Core.Interfaces;
using Liquid.Core.Settings;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging.Decorators
{
    /// <summary>
    /// Configures the culture in the current thread.
    /// Includes its behavior in messaging pipelines before process execution.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LiquidCultureDecorator : ILiquidPipeline
    {
        private const string _culture = "culture";
        private readonly ILiquidConfiguration<CultureSettings> _options;
        private readonly ILiquidPipeline _inner;

        /// <summary>
        /// Initialize a new instance of <see cref="LiquidCultureDecorator"/>
        /// </summary>
        /// <param name="inner">Decorated service.</param>
        /// <param name="options">Default culture configuration.</param>
        public LiquidCultureDecorator(ILiquidPipeline inner, ILiquidConfiguration<CultureSettings> options)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        ///<inheritdoc/>
        public async Task Execute<T>(ProcessMessageEventArgs<T> message, Func<ProcessMessageEventArgs<T>, CancellationToken, Task> process, CancellationToken cancellationToken)
        {

            message.Headers.TryGetValue(_culture, out object cultureCode);

            if (cultureCode is null && !string.IsNullOrEmpty(_options.Settings.DefaultCulture))
            {
                cultureCode = _options.Settings.DefaultCulture;
            }

            if (cultureCode != null)
            {
                CultureInfo.CurrentCulture = new CultureInfo(cultureCode.ToString());
                CultureInfo.CurrentUICulture = new CultureInfo(cultureCode.ToString());
            }

            await _inner.Execute(message, process, cancellationToken);
        }
    }
}
