using Microsoft.Extensions.Configuration;

namespace Liquid.Core.Configuration
{
    /// <summary>
    /// Light Configuration Source Class.
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Configuration.IConfigurationSource" />
    public class LightConfigurationSource : IConfigurationSource
    {
        private readonly string _filepath;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightConfigurationSource"/> class.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        public LightConfigurationSource(string filepath = null)
        {
            _filepath = filepath;
        }

        /// <summary>
        /// Builds the <see cref="T:Microsoft.Extensions.Configuration.IConfigurationProvider" /> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="T:Microsoft.Extensions.Configuration.IConfigurationBuilder" />.</param>
        /// <returns>
        /// An <see cref="T:Microsoft.Extensions.Configuration.IConfigurationProvider" />
        /// </returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new LightConfigurationProvider(_filepath);
        }
    }
}