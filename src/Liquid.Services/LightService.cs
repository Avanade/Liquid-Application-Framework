using AutoMapper;
using Liquid.Core.Interfaces;
using Liquid.Core.Utils;
using Liquid.Services.Attributes;
using Liquid.Services.Configuration;
using Liquid.Services.ResilienceHandlers;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Reflection;

namespace Liquid.Services
{
    /// <summary>
    /// Base service client class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public abstract class LightService : ILightService
    {
        /// <summary>
        /// Gets the service identifier.
        /// </summary>
        /// <value>
        /// The service identifier.
        /// </value>
        public string ServiceId { get; private set; }

        /// <summary>
        /// Gets the service settings.
        /// </summary>
        /// <value>
        /// The service settings.
        /// </value>
        public LightServiceSetting ServiceSettings { get; private set; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        protected ILogger Logger { get; private set; }

        /// <summary>
        /// Gets the mapper service.
        /// </summary>
        /// <value>
        /// The mapper service.
        /// </value>
        protected IMapper MapperService { get; }

        /// <summary>
        /// Gets the resilience.
        /// </summary>
        /// <value>
        /// The resilience.
        /// </value>
        protected IResilienceHandler Resilience { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LightService" /> class.
        /// </summary>
        /// <param name="logger">The logger factory.</param>
        /// <param name="servicesSettings">The services settings.</param>
        /// <param name="mapperService">The mapper service.</param>
        protected LightService(ILogger logger,
                               ILiquidConfiguration<LightServiceSetting> servicesSettings,
                               IMapper mapperService)
        {
            MapperService = mapperService;
            InitiateService(logger, servicesSettings);
        }

        /// <summary>
        /// Initiates the service client.
        /// </summary>
        /// <param name="logger">The logger factory.</param>
        /// <param name="servicesSettings">The services settings.</param>
        private void InitiateService(ILogger logger, ILiquidConfiguration<LightServiceSetting> servicesSettings)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var attribute = GetType().GetCustomAttribute<ServiceIdAttribute>(true);

            ServiceId = attribute?.ServiceId?.IsNotNullOrEmpty() == true ? attribute.ServiceId : GetType().Name;
            Logger = logger;
            ServiceSettings = servicesSettings.Settings;
            Resilience ??= new ResilienceHandler(ServiceSettings, Logger);
        }
    }
}