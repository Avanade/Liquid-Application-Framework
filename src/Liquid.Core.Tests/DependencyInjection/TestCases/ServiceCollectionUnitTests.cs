using System.Diagnostics.CodeAnalysis;
using Liquid.Core.DependencyInjection;
using Liquid.Core.Tests.DependencyInjection.Entities;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Liquid.Core.Tests.DependencyInjection.TestCases
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ServiceCollectionUnitTests
    {
        [Test]
        public void TestServiceCollectionAddScopedAssemblies()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddScopedAssemblies(typeof(ITestInterface<>), GetType().Assembly);
            var serviceProvider = services.BuildServiceProvider();

            var sut = serviceProvider.GetRequiredService<ITestInterface<string>>();
            var response = sut.GetTest();

            Assert.AreEqual("Success", response);
        }

        [Test]
        public void TestServiceCollectionAddScopedTypes()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddScopedAssemblies(typeof(ITestInterface<>), new[] { typeof(TestInterfaceClass) });
            var serviceProvider = services.BuildServiceProvider();

            var sut = serviceProvider.GetRequiredService<ITestInterface<string>>();
            var response = sut.GetTest();

            Assert.AreEqual("Success", response);
        }

        [Test]
        public void TestServiceCollectionAddTransientAssemblies()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddTransientAssemblies(typeof(ITestInterface<>), GetType().Assembly);
            var serviceProvider = services.BuildServiceProvider();

            var sut = serviceProvider.GetRequiredService<ITestInterface<string>>();
            var response = sut.GetTest();

            Assert.AreEqual("Success", response);
        }

        [Test]
        public void TestServiceCollectionAddTransientTypes()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddTransientAssemblies(typeof(ITestInterface<>), new[] { typeof(TestInterfaceClass) });
            var serviceProvider = services.BuildServiceProvider();

            var sut = serviceProvider.GetRequiredService<ITestInterface<string>>();
            var response = sut.GetTest();

            Assert.AreEqual("Success", response);
        }

        [Test]
        public void TestServiceCollectionAddSingletonAssemblies()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingletonAssemblies(typeof(ITestInterface<>), GetType().Assembly);
            var serviceProvider = services.BuildServiceProvider();

            var sut = serviceProvider.GetRequiredService<ITestInterface<string>>();
            var response = sut.GetTest();

            Assert.AreEqual("Success", response);
        }

        [Test]
        public void TestServiceCollectionAddSingletonTypes()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingletonAssemblies(typeof(ITestInterface<>), new[] { typeof(TestInterfaceClass) });
            var serviceProvider = services.BuildServiceProvider();

            var sut = serviceProvider.GetRequiredService<ITestInterface<string>>();
            var response = sut.GetTest();

            Assert.AreEqual("Success", response);
        }
    }
}