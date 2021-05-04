using System.Diagnostics.CodeAnalysis;
using Liquid.Core.Utils;
using NUnit.Framework;

namespace Liquid.Core.Tests.Utils
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ZipUtilsTest
    {
        /// <summary>
        /// Verifies Gzip Compression and Decompression.
        /// </summary>
        [Test]
        public void Verify_GzipCompressionDecompression()
        {
            const string sut = "Hello World!";
            var outputBytes = sut.GzipCompress();

            Assert.Greater(outputBytes.Length, 0);

            var outputString = outputBytes.GzipDecompress();
            Assert.AreEqual(sut, outputString);
        }

        /// <summary>
        /// Verifies Deflate Compression and Decompression.
        /// </summary>
        [Test]
        public void Verify_DeflateCompressionDecompression()
        {
            const string sut = "Hello World!";
            var outputBytes = sut.DeflateCompress();

            Assert.Greater(outputBytes.Length, 0);

            var outputString = outputBytes.DeflateDecompress();
            Assert.AreEqual(sut, outputString);
        }
    }
}