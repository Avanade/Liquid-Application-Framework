using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using MediatR;

namespace Liquid.Domain
{
    /// <summary>
    /// Base command handler class.
    /// </summary>
    public abstract class RequestHandlerBase
    {
        /// <summary>
        /// Gets the command mediator.
        /// </summary>
        /// <value>
        /// The mediator.
        /// </value>
        [ExcludeFromCodeCoverage]
        public IMediator MediatorService { get; }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        [ExcludeFromCodeCoverage]
        public ILightContext ContextService { get; }

        /// <summary>
        /// Gets the telemetry service.
        /// </summary>
        /// <value>
        /// The telemetry service.
        /// </value>
        [ExcludeFromCodeCoverage]
        public ILightTelemetry TelemetryService { get; }

        /// <summary>
        /// Gets the object mapper service.
        /// </summary>
        /// <value>
        /// The mapper service.
        /// </value>
        [ExcludeFromCodeCoverage]
        public IMapper MapperService { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestHandlerBase" /> class.
        /// </summary>
        /// <param name="mediatorService">The mediator service.</param>
        /// <param name="contextService">The context service.</param>
        /// <param name="telemetryService">The telemetry service.</param>
        /// <param name="mapperService">The mapper service.</param>
        protected RequestHandlerBase(IMediator mediatorService,
                                          ILightContext contextService,
                                          ILightTelemetry telemetryService,
                                          IMapper mapperService)
        {
            MediatorService = mediatorService;
            ContextService = contextService;
            TelemetryService = telemetryService;
            MapperService = mapperService;
        }
    }
}
