using System;
using System.Diagnostics.CodeAnalysis;
using Liquid.Core.Utils;
using NUnit.Framework;

namespace Liquid.Core.Tests.Utils
{
    /// <summary>
    /// ByteExtensionsTest Class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ByteUtilsTest
    {
        /// <summary>
        /// Verifies the size of the get kb.
        /// </summary>
        [Test]
        public void Verify_GetKbSize()
        {
            var sut = new byte[0];
            var result = sut.GetKbSize();
            Assert.AreEqual(0d, result);

            sut = CreateByteArray(1024);
            result = sut.GetKbSize();
            Assert.AreEqual(1d, result);
        }

        /// <summary>
        /// Verifies the size of the get mb.
        /// </summary>
        [Test]
        public void Verify_GetMbSize()
        {
            var sut = new byte[0];
            var result = sut.GetMbSize();
            Assert.AreEqual(0d, result);

            sut = CreateByteArray(1048576);
            result = sut.GetMbSize();
            Assert.AreEqual(1d, result);
        }

        /// <summary>
        /// Verifies the size of the get gb.
        /// </summary>
        [Test]
        public void Verify_GetGbSize()
        {
            var sut = new byte[0];
            var result = sut.GetGbSize();
            Assert.AreEqual(0d, result);

            sut = CreateByteArray(1073741824);
            result = sut.GetGbSize();
            Assert.AreEqual(1d, result);
        }

        /// <summary>
        /// Creates the byte array.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        private byte[] CreateByteArray(long size)
        {
            var b = new byte[size];
            new Random().NextBytes(b);
            return b;
        }
    }
}