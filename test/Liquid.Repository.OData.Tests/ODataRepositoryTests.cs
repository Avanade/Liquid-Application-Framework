using Liquid.Core.Interfaces;
using Liquid.Repository.OData.Extensions;
using Liquid.Repository.OData.Tests.Mock;
using NSubstitute;
using Simple.OData.Client;
using System.Linq.Expressions;

namespace Liquid.Repository.OData.Tests
{
    public class OdataRepositoryTests
    {
        private ILiquidRepository<People, string> _sut;

        private IODataClient _client;

        public OdataRepositoryTests()
        {           

            var factory = Substitute.For<IODataClientFactory>();           

            _client = Substitute.For<IODataClient>();

            factory.CreateODataClientAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(_client);

            _sut = new ODataRepository<People, string>(factory, "People");

        }

        [Fact]
        public void ODataRepository_WhenCreatedWithNoClientFactory_ThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new ODataRepository<TestEntity, string>(null, "TestEntity"));
        }

        [Fact]
        public void ODataRepository_WhenCreatedWithNoEntityName_ThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new ODataRepository<TestEntity, string>(Substitute.For<IODataClientFactory>(), null));
        }

        [Fact]
        public async Task AddAsync_WhenActionIsSuccessful_CallClient()
        {
            var entity = new People();

            _sut.SetODataAuthenticationHeader("token");

            await _sut.AddAsync(entity);

            await _client.Received(1).For<People>().Set(entity).InsertEntryAsync();
        }

        [Fact]
        public async Task AddAsync_WhenTokenIsNotSet_ThrowException()
        {
            var entity = new People();

            _sut.SetODataAuthenticationHeader("");

            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.AddAsync(entity));
        }

        [Fact]
        public async Task FindAllAsync_WhenActionIsSuccessful_CallClient()
        {
            _sut.SetODataAuthenticationHeader("token");

            await _sut.FindAllAsync();

            await _client.Received(1).For<People>().FindEntriesAsync();
        }

        [Fact]
        public async Task FindAllAsync_WhenTokenIsNotSet_ThrowException()
        {
            _sut.SetODataAuthenticationHeader("");

            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.FindAllAsync());
        }

        [Fact]

        public async Task FindByIdAsync_WhenActionIsSuccessful_CallClient()
        {
            _sut.SetODataAuthenticationHeader("token");

            await _sut.FindByIdAsync("id");

            await _client.Received(1).For<People>().Key("id").FindEntryAsync();
        }

        [Fact]
        public async Task FindByIdAsync_WhenTokenIsNotSet_ThrowException()
        {
            _sut.SetODataAuthenticationHeader("");

            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.FindByIdAsync("id"));
        }

        [Fact]
        public async Task RemoveByIdAsync_WhenActionIsSuccessful_CallClient()
        {
            _sut.SetODataAuthenticationHeader("token");

            await _sut.RemoveByIdAsync("id");

            await _client.Received(1).For<People>().Key("id").DeleteEntryAsync();
        }

        [Fact]
        public async Task RemoveByIdAsync_WhenTokenIsNotSet_ThrowException()
        {
            _sut.SetODataAuthenticationHeader("");

            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.RemoveByIdAsync("id"));
        }

        [Fact]
        public async Task UpdateAsync_WhenActionIsSuccessful_CallClient()
        {
            var entity = new People();

            _sut.SetODataAuthenticationHeader("token");

            await _sut.UpdateAsync(entity);

            await _client.Received(1).For<People>().Set(entity).UpdateEntryAsync();
        }

        [Fact]
        public async Task UpdateAsync_WhenTokenIsNotSet_ThrowException()
        {
            var entity = new People();

            _sut.SetODataAuthenticationHeader("");

            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.UpdateAsync(entity));
        }

        [Fact]
        public async Task SetODataAuthenticationHeader_WhenTokenIsSet_ReturnClient()
        {
            _sut.SetODataAuthenticationHeader("token");

            Assert.NotNull(_sut);
        }

        [Fact]
        public async Task WhereAsync_WhenActionIsSuccessful_CallClient()
        {
            _sut.SetODataAuthenticationHeader("token");

            Expression<Func<People, bool>> expression = e => e.Id.Equals("id");

            await _sut.WhereAsync(expression);

            await _client.Received(1).For<People>().Filter(expression).FindEntriesAsync();
        }

        [Fact]
        public async Task WhereAsync_WhenTokenIsNotSet_ThrowException()
        {
            _sut.SetODataAuthenticationHeader("");

            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.WhereAsync(e => e.Id.Equals("id")));
        }


        [Fact]
        public async Task SetODataAuthenticationHeader_WhenTokenIsNotSet_ThrowException()
        {
            _sut.SetODataAuthenticationHeader("");

            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.FindAllAsync());
        }

        [Fact]
        public async Task SetODataAuthenticationHeader_WhenTokenIsNull_ThrowException()
        {
            _sut.SetODataAuthenticationHeader(null);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.FindAllAsync());
        }
    }
}
