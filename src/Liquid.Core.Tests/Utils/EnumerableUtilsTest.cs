using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Liquid.Core.Utils;
using NUnit.Framework;

namespace Liquid.Core.Tests.Utils
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class EnumerableUtilsTest
    {
        /// <summary>
        /// Verifies the each.
        /// </summary>
        [Test]
        public void Verify_Each()
        {
            var sut = new[] { "A", "B", "C" };
            var strBuilder = new StringBuilder();
            sut.Each(str => strBuilder.Append($"{str}1"));
            Assert.AreEqual("A1B1C1", strBuilder.ToString());
        }

        /// <summary>
        /// Verifies to separated string.
        /// </summary>
        [Test]
        public void Verify_ToSeparatedString()
        {
            var sut = new[] { "A", "B", "C" };
            var result = sut.ToSeparatedString('-');
            Assert.AreEqual("A-B-C", result);
            result = ((IEnumerable<string>)null).ToSeparatedString('-');
            Assert.IsNull(result);
        }

        /// <summary>
        /// Verifies to separated string comma.
        /// </summary>
        [Test]
        public void Verify_ToSeparatedStringComma()
        {
            var sut = new[] { "A", "B", "C" };
            var result = sut.ToSeparatedString();
            Assert.AreEqual("A,B,C", result);
            result = ((IEnumerable<string>)null).ToSeparatedString();
            Assert.IsNull(result);
        }

        /// <summary>
        /// Verifies the is null or empty.
        /// </summary>
        [Test]
        public void Verify_IsNullOrEmpty()
        {
            var sut = new List<string>();
            var result = sut.IsNullOrEmpty();
            Assert.AreEqual(true, result);
        }

        /// <summary>
        /// Verifies the is null or empty.
        /// </summary>
        [Test]
        public void Verify_IsNotNullOrEmpty()
        {
            var sut = new List<string> { "test" };
            var result = sut.IsNotNullOrEmpty();
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Verify_OrderBy()
        {
            var sut = new List<TestOrderBy>
            {
                new TestOrderBy { Value = 2 },
                new TestOrderBy { Value = 3 },
                new TestOrderBy { Value = 1 }
            };

            var result = sut.OrderBy("Value");
            Assert.AreEqual(1, result.First().Value);

            result = sut.OrderBy("Value desc");
            Assert.AreEqual(3, result.First().Value);
        }
        internal class TestOrderBy
        {
            public int Value { get; set; }
        }
    }
}