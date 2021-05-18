using Liquid.Services.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Reflection;
using AutoMapper;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Liquid.Core.Utils;
using Liquid.Services.Attributes;
using Liquid.Services.ResilienceHandlers;
using Microsoft.Extensions.Logging;

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
        /// Gets the context factory.
        /// </summary>
        /// <value>
        /// The context factory.
        /// </value>
        protected ILightContextFactory ContextFactory { get; }

        /// <summary>
        /// Gets the telemetry factory.
        /// </summary>
        /// <value>
        /// The telemetry factory.
        /// </value>
        protected ILightTelemetryFactory TelemetryFactory { get; }

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
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="servicesSettings">The services settings.</param>
        /// <param name="mapperService">The mapper service.</param>
        protected LightService(ILoggerFactory loggerFactory,
                               ILightContextFactory contextFactory,
                               ILightTelemetryFactory telemetryFactory,
                               ILightServiceConfiguration<LightServiceSetting> servicesSettings,
                               IMapper mapperService)
        {
            ContextFactory = contextFactory;
            MapperService = mapperService;
            TelemetryFactory = telemetryFactory;

            InitiateService(loggerFactory, servicesSettings);
        }

        /// <summary>
        /// Initiates the service client.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="servicesSettings">The services settings.</param>
        private void InitiateService(ILoggerFactory loggerFactory, ILightServiceConfiguration<LightServiceSetting> servicesSettings)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                                   SecurityProtocolType.Tls11 |
                                                   SecurityProtocolType.Tls12;
            var attribute = GetType().GetCustomAttribute<ServiceIdAttribute>(true);

            ServiceId = attribute?.ServiceId?.IsNotNullOrEmpty() == true ? attribute.ServiceId : GetType().Name; 
            Logger = loggerFactory.CreateLogger(ServiceId);
            ServiceSettings = servicesSettings.GetSettings(ServiceId);
            Resilience ??= new ResilienceHandler(ServiceSettings, Logger);
        }
    }
}