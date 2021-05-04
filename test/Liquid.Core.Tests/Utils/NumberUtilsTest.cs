using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using Liquid.Core.Utils;
using NUnit.Framework;

namespace Liquid.Core.Tests.Utils
{
    /// <summary>
    /// Number Extensions Test Class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class NumberUtilsTest
    {
        /// <summary>
        /// Verifies the is prime number.
        /// </summary>
        [Test]
        public void Verify_IsPrimeNumber()
        {
            var sut = 7;
            var result = sut.IsPrimeNumber();
            Assert.IsTrue(result);
            sut = 8;
            result = sut.IsPrimeNumber();
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Verifies the is odd.
        /// </summary>
        [Test]
        public void Verify_IsOdd()
        {
            var sut = 10;
            var result = sut.IsOdd();
            Assert.IsTrue(result);
            sut = 5;
            result = sut.IsOdd();
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Verifies the is even.
        /// </summary>
        [Test]
        public void Verify_IsEven()
        {
            var sut = 5;
            var result = sut.IsEven();
            Assert.IsTrue(result);
            sut = 10;
            result = sut.IsEven();
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Verifies to local currency string.
        /// </summary>
        [Test]
        public void Verify_ToLocalCurrencyString()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var sut = 123d;
            var result = sut.ToLocalCurrencyString();
            Assert.AreEqual("$123.00",result);
        }
    }
}