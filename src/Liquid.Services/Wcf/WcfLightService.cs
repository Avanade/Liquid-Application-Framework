using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;
using AutoMapper;
using Liquid.Core.Configuration;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Liquid.Services.Configuration;
using Microsoft.Extensions.Logging;

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
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="servicesSettings">The services settings.</param>
        /// <param name="mapperService">The mapper service.</param>
        protected WcfLightService(ILoggerFactory loggerFactory, 
                                  ILightContextFactory contextFactory, 
                                  ILightTelemetryFactory telemetryFactory, 
                                  ILightConfiguration<List<LightServiceSetting>> servicesSettings, 
                                  IMapper mapperService) : base(loggerFactory, contextFactory, telemetryFactory, servicesSettings, mapperService)
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