using Liquid.Core.Extensions.DependencyInjection;
using Liquid.Services.Grpc.Tests.Server;
using Liquid.Services.Grpc.Tests.Server.Services;
using Liquid.Services.Grpc.Tests.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Console;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;

namespace Liquid.Services.Grpc.Tests.TestCases
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class GrpcServiceTest
    {
        private IServiceProvider _serviceProvider;

        /// <summary>
        /// Establishes the context.
        /// </summary>
        /// <returns></returns>
        [SetUp]
        protected void EstablishContext()
        {
            //GrpcServer.StartServer();
            //var services = new ServiceCollection();
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, ConsoleLoggerProvider>());
            //LoggerProviderOptions.RegisterProviderOptions<ConsoleLoggerOptions, ConsoleLoggerProvider>(services);
            //services.AddSingleton(LoggerFactory.Create(builder => { builder.AddConsole(); }));

            //IConfiguration configurationRoot = new ConfigurationBuilder().AddJsonFile($"{AppDomain.CurrentDomain.BaseDirectory}appsettings.json").Build();
            //services.AddSingleton(configurationRoot);

            //services.AddAutoMapper(GetType().Assembly);
            //services.AddHttpClient();

            //// this is done to allow test execution without untrusted/invalid certificate issues.
            //services.AddHttpClient("CreditRatingService").ConfigurePrimaryHttpMessageHandler(() =>
            //    new HttpClientHandler { ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator });

            //services.AddGrpcServices(GetType().Assembly);
            //_serviceProvider = services.BuildServiceProvider();
        }

        /// <summary>
        /// Tests the cleanup.
        /// </summary>
        [TearDown]
        protected void TestCleanup()
        {
            //GrpcServer.StopServer();
        }

        [Test]
        public async Task Verify_Grpc_Request()
        {
            //var sut = _serviceProvider.GetRequiredService<ICreditRatingService>();

            //var result = await sut.CheckCreditRatingAsync(new CreditRequest { Credit = 100, CustomerId = "id0201" });

            //Assert.IsNotNull(result);
            //Assert.IsTrue(result.IsAccepted);

            //result = await sut.CheckCreditRatingAsync(new CreditRequest { Credit = 100000, CustomerId = "id0201" });

            //Assert.IsNotNull(result);
            //Assert.IsFalse(result.IsAccepted);
        }
    }
}