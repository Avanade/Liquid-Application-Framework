using System.Diagnostics.CodeAnalysis;
using Liquid.Core.Utils;
using NUnit.Framework;

namespace Liquid.Core.Tests.Utils
{
    /// <summary>
    /// XmlExtensionsTest Class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class XmlUtilsTest
    {
        /// <summary>
        /// Verifies to XML.
        /// </summary>
        [Test]
        public void Verify_ToXml()
        {
            var sut = new Person {FirstName = "First", LastName = "Last"};
            var result = sut.ToXml();
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Verifies the parse XML.
        /// </summary>
        [Test]
        public void Verify_ParseXml()
        {
            var sut = @"<?xml version=""1.0""?>
<Person xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <FirstName>First</FirstName>
  <LastName>Last</LastName>
</Person>";
            var result = sut.ParseXml<Person>();
            Assert.IsNotNull(result);
            Assert.AreEqual("First", result.FirstName);
            Assert.AreEqual("Last", result.LastName);
        }
    }
}