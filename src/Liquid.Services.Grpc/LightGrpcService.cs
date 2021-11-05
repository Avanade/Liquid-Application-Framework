using AutoMapper;
using Grpc.Net.Client;
using Liquid.Core.Interfaces;
using Liquid.Services.Configuration;
using Liquid.Services.Grpc.Exceptions;
using Liquid.Services.Grpc.ResilienceHandlers;
using Liquid.Services.Grpc.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

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
        private readonly ILogger<LightGrpcService<TClient>> _logger;
        private TClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightGrpcService{TClient}"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The HTTP client factory.</param>
        /// <param name="logger">The logger factory.</param>
        /// <param name="servicesSettings">The services settings.</param>
        /// <param name="mapperService">The mapper service.</param>
        public LightGrpcService(IHttpClientFactory httpClientFactory,
                                ILogger<LightGrpcService<TClient>> logger,
                                ILiquidConfiguration<LightServiceSetting> servicesSettings,
                                IMapper mapperService) : base(logger, servicesSettings, mapperService)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            InitializeGrpcClient();
        }

        /// <summary>
        /// Initializes the GRPC client.
        /// </summary>
        private void InitializeGrpcClient()
        {
            Resilience = new GrpcResilienceHandler(ServiceSettings, _logger);
            var httpClient = _httpClientFactory.CreateClient(ServiceId);
            httpClient.Timeout = TimeSpan.FromSeconds(ServiceSettings.Timeout);
            var channel = GrpcChannel.ForAddress(ServiceSettings.Address, new GrpcChannelOptions
            {
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

            try
            {
                TResponse response = default;
                await Resilience.HandleAsync(async () =>
                {
                    response = await grpcRequestMethod(_client);
                });

                return response;
            }
            catch (Exception ex)
            {
                var wrappedException = new GrpcServiceCallException(grpcRequestMethod.Method.Name, ex);
                Logger.LogError(wrappedException, wrappedException.Message);
                throw wrappedException;
            }
        }
    }
}