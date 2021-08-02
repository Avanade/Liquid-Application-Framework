using AutoFixture;
using Liquid.Core.Extensions.DependencyInjection;
using Liquid.Domain.Extensions;
using Liquid.Messaging.Aws.Extensions;
using Liquid.Messaging.Aws.Tests.Consumers;
using Liquid.Messaging.Aws.Tests.Messages;
using Liquid.Messaging.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Console;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Liquid.Messaging.Aws.Tests.UnitTests
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
//            IServiceCollection services = new ServiceCollection();
//            //Add log and configuration.
//            services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, ConsoleLoggerProvider>());
//            LoggerProviderOptions.RegisterProviderOptions<ConsoleLoggerOptions, ConsoleLoggerProvider>(services);
//#pragma warning disable CS0618
//            services.Configure(new Action<ConsoleLoggerOptions>(options => options.DisableColors = false));
//#pragma warning restore CS0618
//            services.AddSingleton(LoggerFactory.Create(builder => { builder.AddConsole(); }));
//            IConfiguration configurationRoot = new ConfigurationBuilder().AddJsonFile($"{AppDomain.CurrentDomain.BaseDirectory}appsettings.json").Build();
//            services.AddSingleton(configurationRoot);

//            services.AddLiquidConfiguration();
//            services.AddLiquidHandlers(false, false, GetType().Assembly);
//            services.AddAutoMapper(GetType().Assembly);

//            services.AddSnsProducer<SnsTestMessage>("TestSns", "SnsTestMessageTopic");
//            services.AddSqsProducer<SqsTestMessage>("TestSqs", "TestMessageQueue", true);
//            services.AddSqsConsumer<AwsSqsConsumer, SqsTestMessage>("TestSqs", "TestMessageQueue");


//            _serviceProvider = services.BuildServiceProvider();
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
        [Test]
        public async Task Verify_If_Can_Send_And_Consume_Compressed_Message()
        {
            //var message = new Fixture().Create<SqsTestMessage>();
            //_serviceProvider.StartMessaging();

            //using (_serviceProvider.CreateScope())
            //{
            //    var producer = _serviceProvider.GetRequiredService<ILightProducer<SqsTestMessage>>();

            //    await producer.SendMessageAsync(message, new Dictionary<string, object> { { "headerTest", "value" } });

            //    await Task.Delay(15000);

            //    Assert.IsNotNull(SqsTestMessage.Self);
            //    Assert.AreEqual(message.Name, SqsTestMessage.Self.Name);
            //}
        }

        /// <summary>
        /// Verifies if can send SNS message.
        /// </summary>
        [Test]
        public async Task Verify_If_Can_Send_Sns_Message()
        {
            //var message = new Fixture().Create<SnsTestMessage>();
            
            //using (_serviceProvider.CreateScope())
            //{
            //    var producer = _serviceProvider.GetRequiredService<ILightProducer<SnsTestMessage>>();

            //    await producer.SendMessageAsync(message, new Dictionary<string, object> { { "headerTest", "value" } });
            //    Assert.IsNotNull(message);
            //}
        }

        /// <summary>
        /// Verifies if can throw exceptions.
        /// </summary>
        /// <returns></returns>
        [Test]
        public void Verify_If_Can_Throw_Exceptions()
        {
            //using (_serviceProvider.CreateScope())
            //{
            //    _serviceProvider.StartMessaging();
            //    var producer = _serviceProvider.GetRequiredService<ILightProducer<SqsTestMessage>>();
            //    Assert.ThrowsAsync<ArgumentNullException>(async () => await producer.SendMessageAsync(null, new Dictionary<string, object> { { "headerTest", "value" } }));
            //}
        }
    }
}