using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.Cache.Tests
{
    public class IServiceCollectionExtensionTests
    {
        private Microsoft.Extensions.DependencyInjection.IServiceCollection _sut;
        private IConfiguration _configProvider = Substitute.For<IConfiguration>();
        private IConfigurationSection _configurationSection = Substitute.For<IConfigurationSection>();
        private readonly IDistributedCache _distributedCache = Substitute.For<IDistributedCache>();

        public IServiceCollectionExtensionTests()
        {
            _configProvider.GetSection(Arg.Any<string>()).Returns(_configurationSection);
            _sut = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
        }
    }
}
