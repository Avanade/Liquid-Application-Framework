using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Liquid.Core.Configuration;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Console;
using NUnit.Framework;

namespace Liquid.Cache.Redis.Tests.TestCases
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class LightRedisCacheTests
    {
        private IServiceProvider _serviceProvider;
        private ILightCache _sut;

        /// <summary>
        /// Establishes the context.
        /// </summary>
        [SetUp]
        public void EstablishContext()
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
            services.AddLightRedisCache();
            _serviceProvider = services.BuildServiceProvider();
            _sut = _serviceProvider.GetService<ILightCache>();
        }

        /// <summary>
        /// Verifies the add with timespan.
        /// </summary>
        [Test]
        public async Task Verify_AddAsync_With_Timespan()
        {
            var sut = _sut;
            await sut.AddAsync("keyt", "value", new TimeSpan(0, 0, 0, 2));

            var obj = await sut.RetrieveAsync<string>("keyt");
            Assert.IsNotNull(obj);
            Assert.AreEqual("value", obj);

            Thread.Sleep(3000);

            obj = await sut.RetrieveAsync<string>("keyt");
            Assert.IsNull(obj);
        }

        /// <summary>
        /// Verifies the remove.
        /// </summary>
        [Test]
        public async Task Verify_RemoveAsync()
        {
            var sut = _sut;
            await sut.AddAsync("keyr", "value", TimeSpan.FromSeconds(10));

            var obj = await sut.RetrieveAsync<string>("keyr");
            Assert.IsNotNull(obj);
            Assert.AreEqual("value", obj);

            await sut.RemoveAsync("keyr");

            obj = await sut.RetrieveAsync<string>("keyr");
            Assert.IsNull(obj);
        }

        /// <summary>
        /// Verifies the remove all.
        /// </summary>
        [Test]
        public async Task Verify_RemoveAllAsync()
        {
            var sut = _sut;
            await sut.AddAsync("keyra", "value", TimeSpan.FromSeconds(10));

            var obj = await sut.RetrieveAsync<string>("keyra");
            Assert.IsNotNull(obj);
            Assert.AreEqual("value", obj);

            await sut.RemoveAllAsync();

            obj = await sut.RetrieveAsync<string>("keyra");
            Assert.IsNull(obj);
        }

        /// <summary>
        /// Verifies the get all keys asynchronous.
        /// </summary>
        [Test]
        public async Task Verify_GetAllKeysAsync()
        {
            var sut = _sut;
            await sut.RemoveAllAsync();

            await sut.AddAsync("keyra1", "value1", TimeSpan.FromSeconds(10));
            await sut.AddAsync("keyra2", "value2", TimeSpan.FromSeconds(10));
            await sut.AddAsync("keyra3", "value3", TimeSpan.FromSeconds(10));
            await sut.AddAsync("keyra4", "value4", TimeSpan.FromSeconds(10));
            await sut.AddAsync("keyra5", "value5", TimeSpan.FromSeconds(10));
            await sut.AddAsync("keyra6", "value6", TimeSpan.FromSeconds(10));

            var keys = await sut.GetAllKeysAsync();
            Assert.AreEqual(6, keys.Count());

            var patternKeys = await sut.GetAllKeysAsync("*ra1*");
            Assert.AreEqual(1, patternKeys.Count());
        }

        /// <summary>
        /// Verifies the exists.
        /// </summary>
        [Test]
        public async Task Verify_ExistsAsync()
        {
            var sut = _sut;
            await sut.AddAsync("key", "value", TimeSpan.FromSeconds(10));

            var obj = await sut.ExistsAsync("key");
            Assert.IsTrue(obj);
        }

        /// <summary>
        /// Verifies the retrieve.
        /// </summary>
        [Test]
        public async Task Verify_RetrieveAsync()
        {
            var sut = _sut;
            await sut.AddAsync("keyre", "value", TimeSpan.FromSeconds(10));

            var obj = await sut.RetrieveAsync<string>("keyre");
            Assert.IsNotNull(obj);
            Assert.AreEqual("value", obj);
        }

        /// <summary>
        /// Verifies the retrieve or add with timespan.
        /// </summary>
        [Test]
        public async Task Verify_RetrieveOrAddAsync_With_Timespan()
        {
            var sut = _sut;
            var obj = await sut.RetrieveOrAddAsync("keyrert", () => "value", new TimeSpan(0, 0, 0, 2));
            Assert.IsNotNull(obj);
            Assert.AreEqual("value", obj);

            Thread.Sleep(3000);

            obj = await sut.RetrieveAsync<string>("keyrert");
            Assert.IsNull(obj);
        }

        /// <summary>
        /// Verifies the retrieve or add with timespan.
        /// </summary>
        [Test]
        public async Task Verify_RetrieveOrAddAsync_Object()
        {
            var sut = _sut;
            var obj = await sut.RetrieveOrAddAsync("keyrert", () => new TestEntity(), new TimeSpan(0, 0, 0, 2));
            Assert.IsNotNull(obj);
            obj = await sut.RetrieveOrAddAsync("keyrert", () => new TestEntity(), new TimeSpan(0, 0, 0, 2));
            Assert.IsNotNull(obj);
            Assert.AreEqual("Test", obj.TestProp);

            Thread.Sleep(3000);

            obj = await sut.RetrieveAsync<TestEntity>("keyrert");
            Assert.IsNull(obj);
        }

        /// <summary>
        /// Verifies the retrieve or add with timespan.
        /// </summary>
        [Test]
        public async Task Verify_RetrieveOrAddAsync_Task_Object()
        {
            var sut = _sut;
            var obj = await sut.RetrieveOrAddAsync("keyrert", () => Task.Run(() => new TestEntity()), new TimeSpan(0, 0, 0, 2));
            Assert.IsNotNull(obj);
            obj = await sut.RetrieveOrAddAsync("keyrert", () => Task.Run(() => new TestEntity()), new TimeSpan(0, 0, 0, 2));
            Assert.IsNotNull(obj);
            Assert.AreEqual("Test", obj.TestProp);

            Thread.Sleep(3000);

            obj = await sut.RetrieveAsync<TestEntity>("keyrert");
            Assert.IsNull(obj);
        }
    }

    [ExcludeFromCodeCoverage]
    [SetUpFixture]
    public class GlobalFixture
    {
        //private RedisInside.Redis _redisServer;
        //[OneTimeSetUp]
        //public void GlobalSetup()
        //{
        //    _redisServer = new RedisInside.Redis(config => { config.Port(6379); });
        //}

        //[OneTimeTearDown]
        //public void GlobalTearDown()
        //{
        //    _redisServer.Dispose();
        //    _redisServer = null;
        //}
    }

    [ExcludeFromCodeCoverage]
    public class TestEntity
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string TestProp => "Test";
    }
}