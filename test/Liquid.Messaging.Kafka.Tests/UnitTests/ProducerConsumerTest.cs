using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AutoFixture;
using Liquid.Core.Context;
using Liquid.Core.DependencyInjection;
using Liquid.Core.Telemetry;
using Liquid.Domain.Extensions;
using Liquid.Messaging.Extensions;
using Liquid.Messaging.Kafka.Extensions;
using Liquid.Messaging.Kafka.Tests.Consumers;
using Liquid.Messaging.Kafka.Tests.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Console;
using NUnit.Framework;

namespace Liquid.Messaging.Kafka.Tests.UnitTests
{
    /// <summary>
    /// Executes tests to all producer/consumer services.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ProducerConsumerTest
    {
        private IServiceProvider _serviceProvider;

        /// <summary>
        /// Initialize dependency injection before test
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            var services = new ServiceCollection();
            //Add log and configuration.
            services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, ConsoleLoggerProvider>());
            LoggerProviderOptions.RegisterProviderOptions<ConsoleLoggerOptions, ConsoleLoggerProvider>(services);
#pragma warning disable CS0618
            services.Configure(new Action<ConsoleLoggerOptions>(options => options.DisableColors = false));
#pragma warning restore CS0618
            services.AddSingleton(LoggerFactory.Create(builder => { builder.AddConsole(); }));
            IConfiguration configurationRoot = new ConfigurationBuilder().AddJsonFile($"{AppDomain.CurrentDomain.BaseDirectory}appsettings.json").Build();
            services.AddSingleton(configurationRoot);
            
            services.AddDefaultTelemetry();
            services.AddDefaultContext();
            services.AddDomainRequestHandlers(GetType().Assembly);
            services.AddAutoMapper(GetType().Assembly);
            services.AddKafkaProducer<KafkaTestMessage>("TestKafka", "TestMessageTopic", true);
            services.AddKafkaConsumer<KafkaTestConsumer, KafkaTestMessage>("TestKafka", "TestMessageTopic");

            _serviceProvider = services.BuildServiceProvider();
        }

        /// <summary>
        /// Dispose all objects.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            _serviceProvider = null;
        }

        /// <summary>
        /// Verifies if can send and consume message.
        /// </summary>
        /// <returns></returns>
        //[Test]
        public async Task Verify_If_Can_Send_And_Consume_Compressed_Message()
        {
            var message = new Fixture().Create<KafkaTestMessage>();
            _serviceProvider.StartMessaging();

            using (_serviceProvider.CreateScope())
            {
                var producer = _serviceProvider.GetRequiredService<ILightProducer<KafkaTestMessage>>();

                await producer.SendMessageAsync(message, new Dictionary<string, object> { { "headerTest", "value" } });

                await Task.Delay(5000);

                Assert.IsNotNull(KafkaTestMessage.Self);
                Assert.AreEqual(message.CreatedDate, KafkaTestMessage.Self.CreatedDate);
            }
        }

        /// <summary>
        /// Verifies if can throw exceptions.
        /// </summary>
        /// <returns></returns>
        [Test]
        public void Verify_If_Can_Throw_Exceptions()
        {
            using (_serviceProvider.CreateScope())
            {
                _serviceProvider.StartMessaging();
                var producer = _serviceProvider.GetRequiredService<ILightProducer<KafkaTestMessage>>();
                Assert.ThrowsAsync<ArgumentNullException>(async () => await producer.SendMessageAsync(null, new Dictionary<string, object> { { "headerTest", "value" } }));
            }
        }
    }
}