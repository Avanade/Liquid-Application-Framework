using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using AutoMapper;
using Grpc.Net.Client;
using Liquid.Core.Configuration;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Liquid.Services.Configuration;
using Liquid.Services.Grpc.Exceptions;
using Liquid.Services.Grpc.Extensions;
using Liquid.Services.Grpc.ResilienceHandlers;
using Liquid.Services.Grpc.Utils;
using Microsoft.Extensions.Logging;

namespace Liquid.Services.Grpc
{
    /// <summary>
    /// Light Grpc Service Class.
    /// </summary>
    /// <seealso cref="Liquid.Services.LightService" />
    /// <seealso cref="ILightGrpcService" />
    public class LightGrpcService<TClient> : LightService, ILightGrpcService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILoggerFactory _loggerFactory;
        private TClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightGrpcService{TClient}"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The HTTP client factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="servicesSettings">The services settings.</param>
        /// <param name="mapperService">The mapper service.</param>
        public LightGrpcService(IHttpClientFactory httpClientFactory,
                                ILoggerFactory loggerFactory,
                                ILightContextFactory contextFactory,
                                ILightTelemetryFactory telemetryFactory,
                                ILightConfiguration<List<LightServiceSetting>> servicesSettings,
                                IMapper mapperService) : base(loggerFactory, contextFactory, telemetryFactory, servicesSettings, mapperService)
        {
            _httpClientFactory = httpClientFactory;
            _loggerFactory = loggerFactory;
            InitializeGrpcClient();
        }

        /// <summary>
        /// Initializes the GRPC client.
        /// </summary>
        private void InitializeGrpcClient()
        {
            Resilience = new GrpcResilienceHandler(ServiceSettings, _loggerFactory.CreateLogger(ServiceId));
            var httpClient = _httpClientFactory.CreateClient(ServiceId);
            httpClient.Timeout = TimeSpan.FromSeconds(ServiceSettings.Timeout);
            var channel = GrpcChannel.ForAddress(ServiceSettings.Address, new GrpcChannelOptions
            {
                LoggerFactory = _loggerFactory, 
                HttpClient = httpClient
            });
            var constructor = GrpcConstructor.CreateConstructor(typeof(TClient), typeof(GrpcChannel));
            _client = (TClient)constructor(channel);
        }

        /// <summary>
        /// Executes the GRPC request.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="grpcRequestMethod">The GRPC request.</param>
        /// <returns></returns>
        public async Task<TResponse> ExecuteGrpcRequestAsync<TResponse>(Func<TClient, Task<TResponse>> grpcRequestMethod)
        {
            var telemetry = TelemetryFactory.GetTelemetry();
            try
            {
                telemetry.AddContext("GrpcServiceRequest");
                
                var telemetryMetricKey = $"GrpcCall_{grpcRequestMethod.Method.Name}";
                telemetry.StartTelemetryStopWatchMetric(telemetryMetricKey);

                TResponse response = default;
                await Resilience.HandleAsync(async () =>
                {
                    response = await grpcRequestMethod(_client);
                    telemetry.CollectGrpcCallInformation(telemetryMetricKey, grpcRequestMethod.Target, response);
                });

                return response;
            }
            catch (Exception ex)
            {
                var wrappedException = new GrpcServiceCallException(grpcRequestMethod.Method.Name, ex);
                Logger.LogError(wrappedException, wrappedException.Message);
                throw wrappedException;
            }
            finally
            {
                telemetry.RemoveContext("GrpcServiceRequest");
            }
        }
    }
}