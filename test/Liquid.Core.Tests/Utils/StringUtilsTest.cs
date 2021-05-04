using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Liquid.Core.Utils;
using NUnit.Framework;

namespace Liquid.Core.Tests.Utils
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class StringUtilsTest
    {
        /// <summary>
        /// Verifies the contains.
        /// </summary>
        [Test]
        public void Verify_Contains()
        {
            var sut = "MARK_ONE";
            var result = sut.Contains("one", StringComparison.InvariantCultureIgnoreCase);
            Assert.True(result);

            result = sut.Contains("one", StringComparison.InvariantCulture);
            Assert.False(result);
        }

        /// <summary>
        /// Verifies the remove line endings.
        /// </summary>
        [Test]
        public void Verify_RemoveLineEndings()
        {
            var sut = "Line1\r\nLine2";
            var result = sut.RemoveLineEndings();
            Assert.AreEqual("Line1Line2", result);

            result = string.Empty.RemoveLineEndings();
            Assert.AreEqual(string.Empty, result);
        }

        /// <summary>
        /// Verifies the is unique identifier.
        /// </summary>
        [Test]
        public void Verify_IsGuid()
        {
            var sut = Guid.NewGuid().ToString();
            var result = sut.IsGuid();
            Assert.IsTrue(result);
            sut = "teste";
            result = sut.IsGuid();
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Verifies the is valid HTTP URL.
        /// </summary>
        [Test]
        public void Verify_IsValidHttpUrl()
        {
            var sut = "http://www.google.com";
            var result = sut.IsValidHttpUrl();
            Assert.IsTrue(result);
            sut = "teste";
            result = sut.IsValidHttpUrl();
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Verifies the append if.
        /// </summary>
        [Test]
        public void Verify_AppendIf()
        {
            var sut = new StringBuilder();
            var result = sut.AppendIf("123", false);
            Assert.AreEqual(string.Empty, result.ToString());
            result = sut.AppendIf("123", true);
            Assert.AreEqual("123", result.ToString());
        }

        /// <summary>
        /// Verifies the word count.
        /// </summary>
        [Test]
        public void Verify_WordCount()
        {
            var sut ="teste 3 palavras";
            var result = sut.CountWords();
            Assert.AreEqual(3, result);
        }

        /// <summary>
        /// Verifies to unique identifier.
        /// </summary>
        [Test]
        public void Verify_ToGuid()
        {
            var sut = "{1FEF9D3D-0411-4138-9E30-B838F0EA744A}";
            var result = sut.ToGuid();
            Assert.AreEqual(Guid.Parse("{1FEF9D3D-0411-4138-9E30-B838F0EA744A}"), result);
        }
    }
}