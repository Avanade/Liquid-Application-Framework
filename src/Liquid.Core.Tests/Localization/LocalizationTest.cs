using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using Liquid.Core.Configuration;
using Liquid.Core.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Liquid.Core.Tests.Localization
{
    /// <summary>
    /// Resource file catalog tests class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class LocalizationTest
    {
        private const string StringValueKey = "stringValueKey";
        private ILocalization _subjectUnderTest;
        private IServiceProvider _serviceProvider;

        /// <summary>
        /// Establishes the context.
        /// </summary>
        /// <returns></returns>
        [SetUp]
        protected void EstablishContext()
        {
            IServiceCollection services = new ServiceCollection();
            IConfiguration configurationRoot = new ConfigurationBuilder().AddLightConfigurationFile().Build();
            services.AddSingleton(configurationRoot);
            services.AddSingleton<ILightConfiguration<CultureSettings>, CultureConfiguration>();
            services.AddLocalizationService();
            _serviceProvider = services.BuildServiceProvider();


            var cultureInfo = new CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            _subjectUnderTest = _serviceProvider.GetService<ILocalization>();
        }

        /// <summary>
        /// Asserts if can read string from cache.
        /// </summary>
        [Test]
        public void Verify_if_can_read_string_from_file()
        {
            var stringValue = _subjectUnderTest.Get(StringValueKey);
            Assert.AreEqual("Texto em português", stringValue);
            stringValue = _subjectUnderTest.Get(StringValueKey, new CultureInfo("en-US"));
            Assert.AreEqual("English text", stringValue);
            stringValue = _subjectUnderTest.Get(StringValueKey, new CultureInfo("es-ES"), "iphone");
            Assert.AreEqual("texto en español", stringValue);
            stringValue = _subjectUnderTest.Get("InexistentKey");
            Assert.AreEqual("InexistentKey", stringValue);

        }

        /// <summary>
        /// Verifies exceptions.
        /// </summary>
        [Test]
        public void Verify_Exceptions()
        {
            Assert.Catch<ArgumentNullException>(() => { _serviceProvider.GetService<ILocalization>().Get(StringValueKey, (CultureInfo)null); });
        }
    }
}