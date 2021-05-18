using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Liquid.Services.Attributes;
using Liquid.Services.Configuration;
using Liquid.Services.Grpc.Tests.Server.Services;
using Microsoft.Extensions.Logging;

namespace Liquid.Services.Grpc.Tests.Services
{
    /// <summary>
    /// Credit Rating Service Class.
    /// </summary>
    /// <seealso>
    ///     <cref>Liquid.Services.Grpc.LightGrpcService{Liquid.Services.Grpc.Tests.Server.Services.CreditRatingCheck.CreditRatingCheckClient}</cref>
    /// </seealso>
    /// <seealso>
    ///     <cref>Liquid.Services.Grpc.Tests.Services.ICreditRatingService</cref>
    /// </seealso>
    [ExcludeFromCodeCoverage]
    [ServiceId("CreditRatingService")]
    public class CreditRatingService : LightGrpcService<CreditRatingCheck.CreditRatingCheckClient>, ICreditRatingService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreditRatingService"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The HTTP client factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="servicesSettings">The services settings.</param>
        /// <param name="mapperService">The mapper service.</param>
        public CreditRatingService(IHttpClientFactory httpClientFactory, 
                                   ILoggerFactory loggerFactory, 
                                   ILightContextFactory contextFactory, 
                                   ILightTelemetryFactory telemetryFactory, 
                                   ILightServiceConfiguration<LightServiceSetting> servicesSettings, 
                                   IMapper mapperService) : base(httpClientFactory, loggerFactory, contextFactory, telemetryFactory, servicesSettings, mapperService)
        {
        }

        /// <summary>
        /// Checks the credit rating asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<CreditReply> CheckCreditRatingAsync(CreditRequest request)
        {
            return await ExecuteGrpcRequestAsync(async client => await client.CheckCreditRequestAsync(request));
        }
    }
}