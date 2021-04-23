using Liguid.Repository.Configuration;
using Liquid.Core.Configuration;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

namespace Liquid.Repository.EntityFramework.Tests
{
    class EntityFrameworkFactoryTests
    {
        private ILightConfiguration<List<LightConnectionSettings>> _configuration;
        private IEntityFrameworkClientFactory _sut;

        [SetUp]
        protected void SetContext()
        {            

            var connectionSettings = new LightConnectionSettings()
            {
                Id = "test",
                ConnectionString = "testconnectionstring",
                DatabaseName = "testDatabase"
            };
            _configuration = Substitute.For<ILightConfiguration<List<LightConnectionSettings>>>();

            _configuration.Settings
                .Returns(new List<LightConnectionSettings>() { connectionSettings }, new List<LightConnectionSettings>() { connectionSettings });

            _sut = new EntityFrameworkClientFactory(_configuration);

        }

        [Test]
        public void GetClient_WhenDatabaseIdExists_ClientCreated()
        {
            var result = _sut.GetClient("test");

            Assert.IsFalse(result?.Database is null);
        }

        [Test]
        public void GetClient_WhenDatabaseIdDoesNotExist_ThrowError()
        {
            Assert.Throws<LightDatabaseConfigurationDoesNotExistException>(() => _sut.GetClient("testThrow"));
        }

    }
}
