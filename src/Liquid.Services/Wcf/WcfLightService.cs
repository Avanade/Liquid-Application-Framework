using AutoMapper;
using Liquid.Core.Interfaces;
using Liquid.Services.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;

namespace Liquid.Services.Wcf
{
    /// <summary>
    /// Base Wcf service client class.
    /// </summary>
    /// <seealso cref="Liquid.Services.LightService" />
    [ExcludeFromCodeCoverage]
    public abstract class WcfLightService : LightService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WcfLightService"/> class.
        /// </summary>
        /// <param name="logger">The logger factory.</param>
        /// <param name="servicesSettings">The services settings.</param>
        /// <param name="mapperService">The mapper service.</param>
        protected WcfLightService(ILogger<WcfLightService> logger,
                                  ILiquidConfiguration<LightServiceSetting> servicesSettings,
                                  IMapper mapperService) : base(logger, servicesSettings, mapperService)
        {
        }

        /// <summary>
        /// Gets the basic HTTP security mode.
        /// </summary>
        /// <returns></returns>
        protected BasicHttpSecurityMode GetHttpSecurityMode()
        {
            var returnValue = BasicHttpSecurityMode.None;
            if (ServiceSettings.Address.ToLower().StartsWith("https://"))
            {
                returnValue = BasicHttpSecurityMode.Transport;
            }
            return returnValue;
        }
    }
}