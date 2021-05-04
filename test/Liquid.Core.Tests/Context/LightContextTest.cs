using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Liquid.Core.Context;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Liquid.Core.Tests.Context
{
    /// <summary>
    /// ScopeContextTest Class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class LightContextTest
    {
        private IServiceProvider _serviceProvider;

        /// <summary>
        /// Sets up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<ILightContextFactory, LightContextFactory>();
            serviceCollection.AddTransient<ILightContext, LightContext>();
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// Tears down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            _serviceProvider = null;
        }
        
        /// <summary>
        /// Tests the scoped context.
        /// </summary>
        [Test]
        public void Verify_ScopedContext()
        {
            var sut = _serviceProvider.GetRequiredService<ILightContext>();
            sut.AddOrReplaceContextValue("key", "value");
            var obj = sut.GetContextDataValue<string>("key");
            Assert.IsNotNull(obj);
            Assert.IsInstanceOf<string>(obj);
            Assert.AreEqual("value", obj);
        }

        /// <summary>
        /// Tests the scoped context key not found.
        /// </summary>
        [Test]
        public void Verify_ScopedContextKeyNotFound()
        {
            var sut = _serviceProvider.GetRequiredService<ILightContext>();
            sut.AddOrReplaceContextValue("key", "value");
            var obj = sut.GetContextDataValue<string>("notfound");
            Assert.IsNull(obj);
        }

        /// <summary>
        /// Tests the scoped context add existing key.
        /// </summary>
        [Test]
        public void Verify_ScopedContextAddExistingKey()
        {
            var sut = _serviceProvider.GetRequiredService<ILightContext>();
            sut.AddOrReplaceContextValue("key", "value");
            var obj = sut.GetContextDataValue<string>("key");
            Assert.IsNotNull(obj);
            Assert.IsInstanceOf<string>(obj);
            Assert.AreEqual("value", obj);

            sut.AddOrReplaceContextValue("key", "value1");

            obj = sut.GetContextDataValue<string>("key");
            Assert.IsNotNull(obj);
            Assert.IsInstanceOf<string>(obj);
            Assert.AreEqual("value1", obj);
        }

        /// <summary>
        /// Tests the change context identifier.
        /// </summary>
        [Test]
        public void Verify_ChangeContextId()
        {
            var sut = _serviceProvider.GetRequiredService<ILightContext>();
            var originalId = sut.ContextId;
            sut.SetContextId(Guid.NewGuid());
            Assert.AreNotEqual(originalId, sut.ContextId);

            Assert.Throws<ArgumentOutOfRangeException>(() => sut.SetContextId(Guid.Empty));
        }

        /// <summary>
        /// Tests the change business context identifier.
        /// </summary>
        [Test]
        public void Verify_ChangeBusinessContextId()
        {
            var sut = _serviceProvider.GetRequiredService<ILightContext>();
            var originalId = sut.BusinessContextId;
            sut.SetBusinessContextId(Guid.NewGuid());
            Assert.AreNotEqual(originalId, sut.BusinessContextId);

            Assert.Throws<ArgumentOutOfRangeException>(() => sut.SetBusinessContextId(Guid.Empty));
        }

        /// <summary>
        /// Tests the change culture.
        /// </summary>
        [Test]
        public void Verify_ChangeCulture()
        {
            var sut = _serviceProvider.GetRequiredService<ILightContext>();
            var originalCulture = sut.ContextCulture;
            sut.SetCulture("fr-FR");
            Assert.AreNotEqual(originalCulture, sut.ContextCulture);
        }

        /// <summary>
        /// Tests the change channel.
        /// </summary>
        [Test]
        public void Verify_ChangeChannel()
        {
            var sut = _serviceProvider.GetRequiredService<ILightContext>();
            var originalChannel = sut.ContextChannel;
            sut.SetChannel("mobile");
            Assert.AreNotEqual(originalChannel, sut.ContextChannel);
        }

        /// <summary>
        /// Tests notifications.
        /// </summary>
        [Test]
        public void Verify_Notifications()
        {
            var sut = _serviceProvider.GetRequiredService<ILightContext>();
            sut.Notify("key", "message");

            var result = sut.GetNotifications();
            Assert.IsNotNull(result);
            Assert.AreEqual("message", result["key"]);

            var data = sut.GetNotification("key");
            Assert.AreEqual("message", data);
        }

        [Test]
        public void Verify_ContextFactory()
        {
            var sut = _serviceProvider.GetRequiredService<ILightContextFactory>();
            var context = sut.GetContext();
            Assert.IsNotNull(context);
        }

        /// <summary>
        /// Tests the parallel context.
        /// </summary>
        [Test]
        public void Verify_ParallelContext()
        {
            ILightContext sut = new LightContext();

            Parallel.For(0, 10000, new ParallelOptions { MaxDegreeOfParallelism = 4 }, i =>
            {
                sut.AddOrReplaceContextValue("key", i);
            });

            Assert.GreaterOrEqual(sut.GetContextDataValue<int>("key"), 0);
        }
    }
}
