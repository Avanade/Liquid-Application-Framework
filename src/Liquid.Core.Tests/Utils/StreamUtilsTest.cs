using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using Liquid.Core.Utils;
using NUnit.Framework;

namespace Liquid.Core.Tests.Utils
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class StreamUtilsTest
    {
        /// <summary>
        /// Verifies the get string from stream UTF8.
        /// </summary>
        [Test]
        public void Verify_GetStringFromStreamUtf8()
        {
            var str = "test";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(str));

            var result = stream.AsStringUtf8();
            Assert.AreEqual(str, result);

            Assert.IsNull(((Stream)null).AsStringUtf8());
        }

        /// <summary>
        /// Verifies the get string from stream ASCII.
        /// </summary>
        [Test]
        public void Verify_GetStringFromStreamAscii()
        {
            var str = "test";
            var stream = new MemoryStream(Encoding.ASCII.GetBytes(str));

            var result = stream.AsStringAscii();
            Assert.AreEqual(str, result);

            Assert.IsNull(((Stream)null).AsStringAscii());
        }

        /// <summary>
        /// Verifies the get string from stream unicode.
        /// </summary>
        [Test]
        public void Verify_GetStringFromStreamUnicode()
        {
            var str = "test";
            var stream = new MemoryStream(Encoding.Unicode.GetBytes(str));

            var result = stream.AsStringUnicode();
            Assert.AreEqual(str, result);

            Assert.IsNull(((Stream)null).AsStringUnicode());
        }

        /// <summary>
        /// Verifies the get stream from string UTF8.
        /// </summary>
        [Test]
        public void Verify_GetStreamFromStringUtf8()
        {
            var str = "test";
            var stream = str.ToStreamUtf8();
            Assert.AreEqual(4, stream.Length);

            var result = stream.AsStringUtf8();
            Assert.AreEqual(str, result);

            Assert.IsNull(((string)null).ToStreamUtf8());
        }

        /// <summary>
        /// Verifies the get stream from string ASCII.
        /// </summary>
        [Test]
        public void Verify_GetStreamFromStringAscii()
        {
            var str = "test";
            var stream = str.ToStreamAscii();
            Assert.AreEqual(4, stream.Length);

            var result = stream.AsStringAscii();
            Assert.AreEqual(str, result);

            Assert.IsNull(((string)null).ToStreamAscii());
        }

        /// <summary>
        /// Verifies the get stream from string unicode.
        /// </summary>
        [Test]
        public void Verify_GetStreamFromStringUnicode()
        {
            var str = "test";
            var stream = str.ToStreamUnicode();
            Assert.AreEqual(8, stream.Length);

            var result = stream.AsStringUnicode();
            Assert.AreEqual(str, result);

            Assert.IsNull(((string)null).ToStreamUnicode());
        }

        /// <summary>
        /// Verifies to byte array.
        /// </summary>
        [Test]
        public void Verify_ToByteArray()
        {
            var str = "test";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(str));

            var result = stream.ToByteArray();
            Assert.AreEqual(4, result.Length);
        }
    }
}