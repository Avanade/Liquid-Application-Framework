using Liquid.Core.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.Cache.Tests
{
    public class LiquidCacheTests
    {
        private readonly IDistributedCache _distributedCache = Substitute.For<IDistributedCache>();
        private readonly LiquidCache _sut;

        public LiquidCacheTests()
        {
            _sut = new LiquidCache(_distributedCache);
        }
        [Fact]
        public void Ctor_WhenIDistributedCacheIsNull_ThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new LiquidCache(null));
        }

        [Fact]
        public void GetByteArray_WhenKeyExists_ThenReturnByteArray()
        {
            //Arrange
            var bytes = new byte[] { 1, 2, 3, 4, 5 };
            _distributedCache.Get(Arg.Any<string>()).Returns(bytes);

            //Act
            var result = _sut.Get("test");

            //Assert
            Assert.Equal(bytes, result);
        }

        [Fact]
        public async Task GetAsyncByteArray_WhenKeyExists_ThenReturnByteArray()
        {
            //Arrange
            var bytes = new byte[] { 1, 2, 3, 4, 5 };
            _distributedCache.GetAsync(Arg.Any<string>()).Returns(bytes);

            //Act
            var result = await _sut.GetAsync("test");

            //Assert
            Assert.Equal(bytes, result);

        }

        [Fact]
        public void GetComplexType_WhenKeyExists_ThenReturnType()
        {
            //Arrange
            var values = new MockType();
            _distributedCache.Get(Arg.Any<string>()).Returns(values.ToJsonBytes());

            //Act
            var result = _sut.Get<MockType>("test");

            //Assert
            Assert.IsType<MockType>(result);
        }

        [Fact]
        public async Task GetAsyncComplexType_WhenKeyExists_ThenReturnType()
        {
            //Arrange
            var values = new MockType();
            _distributedCache.GetAsync(Arg.Any<string>()).Returns(values.ToJsonBytes());

            //Act
            var result = await _sut.GetAsync<MockType>("test");

            //Assert
            Assert.IsType<MockType>(result);

        }

        [Fact]
        public void GetPrimitiveType_WhenKeyExists_ThenReturnPrimitive()
        {
            //Arrange
            var values = false;
            _distributedCache.Get(Arg.Any<string>()).Returns(values.ToJsonBytes());

            //Act
            var result = _sut.Get<bool>("test");

            //Assert
            Assert.IsType<bool>(result);
            Assert.Equal(values, result);
        }

        [Fact]
        public async Task GetAsyncPrimitiveType_WhenKeyExists_ThenReturnPrimitive()
        {
            //Arrange
            var values = true;
            _distributedCache.GetAsync(Arg.Any<string>()).Returns(values.ToJsonBytes());

            //Act
            var result = await _sut.GetAsync<bool>("test");

            //Assert
            Assert.IsType<bool>(result);
            Assert.Equal(values, result);
        }

        [Fact]
        public void GetGuid_WhenKeyExists_ThenReturnGuid()
        {
            //Arrange
            var values = new Guid();
            _distributedCache.Get(Arg.Any<string>()).Returns(values.ToJsonBytes());

            //Act
            var result = _sut.Get<Guid>("test");

            //Assert
            Assert.IsType<Guid>(result);
            Assert.Equal(values, result);
        }

        [Fact]
        public async Task GetAsyncGuid_WhenKeyExists_ThenReturnGuid()
        {
            //Arrange
            var values = new Guid();
            _distributedCache.GetAsync(Arg.Any<string>()).Returns(values.ToJsonBytes());

            //Act
            var result = await _sut.GetAsync<Guid>("test");

            //Assert
            Assert.IsType<Guid>(result);
            Assert.Equal(values, result);
        }

        [Fact]
        public void SetByteArray_WhenSucessfullySet_ThenDistributeCacheSetReceivedCall()
        {
            //Arrange
            var bytes = new byte[] { 1, 2, 3, 4, 5 };

            //Act
            _sut.Set("test", bytes, default);

            //Assert

            _distributedCache.Received(1).Set("test", bytes, Arg.Any<DistributedCacheEntryOptions>());
        }

        [Fact]
        public async Task SetAsyncByteArray_WhenSucessfullySet_ThenDistributeCacheSetReceivedCall()
        {
            //Arrange
            var bytes = new byte[] { 1, 2, 3, 4, 5 };

            //Act
            await _sut.SetAsync("test", bytes, default);

            //Assert
            await _distributedCache.Received(1).SetAsync("test", bytes, Arg.Any<DistributedCacheEntryOptions>());

        }

        [Fact]
        public void SetComplexType_WhenSucessfullySet_ThenDistributeCacheSetReceivedCall()
        {
            //Arrange
            var values = new MockType();

            //Act
            _sut.Set("test", values, default);

            //Assert

            _distributedCache.Received(1).Set("test", Arg.Any<byte[]>(), Arg.Any<DistributedCacheEntryOptions>());
        }

        [Fact]
        public async Task SetAsyncComplexType_WhenSucessfullySet_ThenDistributeCacheSetReceivedCall()
        {
            //Arrange
            var values = new MockType();

            //Act
            await _sut.SetAsync("test", values, default);

            //Assert
            await _distributedCache.Received(1).SetAsync("test", Arg.Any<byte[]>(), Arg.Any<DistributedCacheEntryOptions>());

        }

        [Fact]
        public void SetPrimitiveType_WhenSucessfullySet_ThenDistributeCacheSetReceivedCall()
        {
            //Arrange
            var values = "Test value";

            //Act
            _sut.Set("test", values, default);

            //Assert

            _distributedCache.Received(1).Set("test", Arg.Any<byte[]>(), Arg.Any<DistributedCacheEntryOptions>());
        }

        [Fact]
        public async Task SetAsyncPrimitiveType_WhenSucessfullySet_ThenDistributeCacheSetReceivedCall()
        {
            //Arrange
            var values = "Test value";

            //Act
            await _sut.SetAsync("test", values, default);

            //Assert
            await _distributedCache.Received(1).SetAsync("test", Arg.Any<byte[]>(), Arg.Any<DistributedCacheEntryOptions>());

        }

        [Fact]
        public void SetGuid_WhenSucessfullySet_ThenDistributeCacheSetReceivedCall()
        {
            //Arrange
            var values = new Guid();

            //Act
            _sut.Set("test", values, default);

            //Assert

            _distributedCache.Received(1).Set("test", Arg.Any<byte[]>(), Arg.Any<DistributedCacheEntryOptions>());
        }

        [Fact]
        public async Task SetAsyncGuid_WhenSucessfullySet_ThenDistributeCacheSetReceivedCall()
        {
            //Arrange
            var values = new Guid();

            //Act
            await _sut.SetAsync("test", values, default);

            //Assert
            await _distributedCache.Received(1).SetAsync("test", Arg.Any<byte[]>(), Arg.Any<DistributedCacheEntryOptions>());

        }

        [Fact]
        public void Refresh_WhenSucessfull_ThenDistributeCacheRefreshReceivedCall()
        {
            //Arrange

            //Act
            _sut.Refresh("test");

            //Assert

            _distributedCache.Received(1).Refresh("test");
        }

        [Fact]
        public async Task RefreshAsync_WhenSucessfull_ThenDistributeCacheRefreshAsyncReceivedCall()
        {
            //Arrange

            //Act
            await _sut.RefreshAsync("test");

            //Assert

            await _distributedCache.Received(1).RefreshAsync("test");

        }

        [Fact]
        public void Remove_WhenSucessfull_ThenDistributeCacheRemoveReceivedCall()
        {
            //Arrange

            //Act
            _sut.Remove("test");

            //Assert

            _distributedCache.Received(1).Remove("test");
        }

        [Fact]
        public async Task RemoveAsync_WhenSucessfull_ThenDistributeCacheRemoveAsyncReceivedCall()
        {
            //Arrange

            //Act
            await _sut.RemoveAsync("test");

            //Assert

            await _distributedCache.Received(1).RemoveAsync("test");

        }
    }
}