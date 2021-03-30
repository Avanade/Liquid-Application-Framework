using System.Diagnostics.CodeAnalysis;
using Liquid.Core.Utils;
using NUnit.Framework;

namespace Liquid.Core.Tests.Utils
{
    [TestFixture()]
    [ExcludeFromCodeCoverage]
    public class HashUtilsTest
    {
        [Test]
        public void Verify_ComputeHash()
        {
            var str = "teste";
            var result = str.CreateHash("key", HashUtils.HashType.HMacMd5);
            Assert.IsNotNull(result);
            result = str.CreateHash("key", HashUtils.HashType.HMacSha1);
            Assert.IsNotNull(result);
            result = str.CreateHash("key", HashUtils.HashType.HMacSha256);
            Assert.IsNotNull(result);
            result = str.CreateHash("key", HashUtils.HashType.HMacSha384);
            Assert.IsNotNull(result);
            result = str.CreateHash("key", HashUtils.HashType.HMacSha512);
            Assert.IsNotNull(result);
            result = str.CreateHash("key", HashUtils.HashType.Md5);
            Assert.IsNotNull(result);
            result = str.CreateHash("key", HashUtils.HashType.Sha1);
            Assert.IsNotNull(result);
            result = str.CreateHash("key", HashUtils.HashType.Sha256);
            Assert.IsNotNull(result);
            result = str.CreateHash("key", HashUtils.HashType.Sha384);
            Assert.IsNotNull(result);
            result = str.CreateHash("key", HashUtils.HashType.Sha512);
            Assert.IsNotNull(result);
        }
    }
}