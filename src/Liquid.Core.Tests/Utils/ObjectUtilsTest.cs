using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Liquid.Core.Utils;
using NUnit.Framework;

namespace Liquid.Core.Tests.Utils
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ObjectUtilsTest
    {
        /// <summary>
        /// Verifies to dictionary.
        /// </summary>
        [Test]
        public void Verify_ToDictionary()
        {
            var sut = new
            {
                firstName = "Will",
                lastName = "Doe"
            };
            var dictionary = sut.ToDictionary();
            Assert.AreEqual("Will", (string)dictionary["firstName"]);
            Assert.AreEqual("Doe", (string)dictionary["lastName"]);

            var sut2 = new
            {
                firstName = "Will",
                lastName = "Doe",
                Number = 1
            };
            var dictionary2 = sut2.ToDictionary<string>();
            Assert.AreEqual("Will", dictionary2["firstName"]);
            Assert.AreEqual("Doe", dictionary2["lastName"]);

            Assert.Throws<ArgumentNullException>(() => ((object)null).ToDictionary());
            Assert.Throws<ArgumentNullException>(() => ((object)null).ToDictionary<string>());
        }

        /// <summary>
        /// Verifies the type of the is of.
        /// </summary>
        public void Verify_IsOfType()
        {
            var sut = new Person { FirstName = "Will", LastName = "Doe" };
            var result = sut.IsOfType<Person>();
            Assert.True(result);
        }

        /// <summary>
        /// Verifies the is datetime.
        /// </summary>
        [Test]
        public void Verify_IsDatetime()
        {
            object sut = DateTime.Today;
            var result = sut.IsDatetime();
            Assert.True(result);
        }

        /// <summary>
        /// Verifies to datetime.
        /// </summary>
        [Test]
        public void Verify_ToDatetime()
        {
            var sut = "2018-03-20";
            var result = sut.ToDatetime(out var outer);
            Assert.True(result);
            Assert.AreEqual(2018, outer?.Year);
            Assert.AreEqual(3, outer?.Month);
            Assert.AreEqual(20, outer?.Day);

            Assert.False(((object)null).ToDatetime(out _));
        }

        /// <summary>
        /// Verifies the is boolean.
        /// </summary>
        [Test]
        public void Verify_IsBoolean()
        {
            object sut = true;
            var result = sut.IsBoolean();
            Assert.True(result);
        }

        /// <summary>
        /// Verifies to boolean.
        /// </summary>
        [Test]
        public void Verify_ToBoolean()
        {
            var sut = "true";
            var result = sut.ToBoolean(out var outer);
            Assert.True(result);
            Assert.True(outer);

            Assert.False(((object)null).ToBoolean(out _));
        }

        /// <summary>
        /// Verifies the is unique identifier.
        /// </summary>
        [Test]
        public void Verify_IsGuid()
        {
            object sut = Guid.NewGuid();
            var result = sut.IsGuid();
            Assert.True(result);
        }

        /// <summary>
        /// Verifies the is numeric.
        /// </summary>
        [Test]
        public void Verify_IsNumeric()
        {
            object sut = 123;
            var result = sut.IsNumeric();
            Assert.True(result);

            sut = 123.2;
            result = sut.IsNumeric();
            Assert.True(result);

            Assert.False(((object)null).IsNumeric());
        }

        /// <summary>
        /// Verifies the is integer.
        /// </summary>
        [Test]
        public void Verify_IsInteger()
        {
            object sut = 123;
            var result = sut.IsInteger();
            Assert.True(result);

            sut = 123.2;
            result = sut.IsInteger();
            Assert.False(result);
        }

        /// <summary>
        /// Verifies to integer.
        /// </summary>
        [Test]
        public void Verify_ToInteger()
        {
            var sut = "123";
            var result = sut.ToInteger(out var outer);
            Assert.True(result);
            Assert.AreEqual(123, outer);

            Assert.False(((object)null).ToInteger(out _));
        }

        /// <summary>
        /// Verifies the is double.
        /// </summary>
        [Test]
        public void Verify_IsDouble()
        {
            object sut = 123;
            var result = sut.IsDouble();
            Assert.True(result);

            sut = 123.2;
            result = sut.IsDouble();
            Assert.True(result);
        }

        /// <summary>
        /// Verifies to double.
        /// </summary>
        [Test]
        public void Verify_ToDouble()
        {
            var sut = "123.12";
            var result = sut.ToDouble(out var outer);
            Assert.True(result);
            Assert.AreEqual(123.12, outer);

            Assert.False(((object)null).ToDouble(out _));
        }

        /// <summary>
        /// Verifies to bytes.
        /// </summary>
        [Test]
        public void Verify_ToBytes()
        {
            var sut = "123";
            var result = sut.ToBytes();
            Assert.True(result.Any());
            sut = null;
            result = sut.ToBytes();
            Assert.False(result.Any());
        }

        /// <summary>
        /// Verifies the get private property value.
        /// </summary>
        [Test]
        public void Verify_GetPrivatePropertyValue()
        {
            var sut = new TestObject();
            var result = sut.GetPrivatePropertyValue<string>("LastName");
            Assert.AreEqual("Doe", result);

            Assert.Throws<ArgumentNullException>(() => ((object) null).GetPrivatePropertyValue<string>("LastName"));
            Assert.Throws<ArgumentOutOfRangeException>(() => sut.GetPrivatePropertyValue<string>("Id"));
        }

        /// <summary>
        /// Verifies the get private field value.
        /// </summary>
        [Test]
        public void Verify_GetPrivateFieldValue()
        {
            var sut = new TestObject();
            var result = sut.GetPrivateFieldValue<string>("_firstName");
            Assert.AreEqual("Will", result);

            Assert.Throws<ArgumentNullException>(() => ((object)null).GetPrivateFieldValue<string>("_firstName"));
            Assert.Throws<ArgumentOutOfRangeException>(() => sut.GetPrivateFieldValue<string>("Id"));
        }

        /// <summary>
        /// Verifies the set private property value.
        /// </summary>
        [Test]
        public void Verify_SetPrivatePropertyValue()
        {
            var sut = new TestObject();
            var result = sut.Age;
            Assert.AreEqual(18, result);
            sut.SetPrivatePropertyValue("Age", 19);
            result = sut.Age;
            Assert.AreEqual(19, result);
            Assert.Throws<ArgumentNullException>(() => ((object)null).SetPrivatePropertyValue("Age", 20));
            Assert.Throws<ArgumentOutOfRangeException>(() => sut.SetPrivatePropertyValue("Id", "id"));
        }

        /// <summary>
        /// Verifies the set private field value.
        /// </summary>
        [Test]
        public void Verify_SetPrivateFieldValue()
        {
            var sut = new TestObject();
            var result = sut.GetPrivateFieldValue<string>("_firstName");
            Assert.AreEqual("Will", result);

            sut.SetPrivateFieldValue("_firstName", "John");
            result = sut.GetPrivateFieldValue<string>("_firstName");
            Assert.AreEqual("John", result);

            Assert.Throws<ArgumentNullException>(() => ((object)null).SetPrivateFieldValue("_firstName", "John"));
            Assert.Throws<ArgumentOutOfRangeException>(() => sut.SetPrivateFieldValue("Id", "id"));
        }

        /// <summary>
        /// Verifies the type of the is primitive.
        /// </summary>
        [Test]
        public void Verify_IsPrimitiveType()
        {
            Assert.IsTrue(typeof(Enum).IsPrimitiveType());
            Assert.IsTrue(typeof(String).IsPrimitiveType());
            Assert.IsTrue(typeof(Char).IsPrimitiveType());
            Assert.IsTrue(typeof(Guid).IsPrimitiveType());

            Assert.IsTrue(typeof(Boolean).IsPrimitiveType());
            Assert.IsTrue(typeof(Byte).IsPrimitiveType());
            Assert.IsTrue(typeof(Int16).IsPrimitiveType());
            Assert.IsTrue(typeof(Int32).IsPrimitiveType());
            Assert.IsTrue(typeof(Int64).IsPrimitiveType());
            Assert.IsTrue(typeof(Single).IsPrimitiveType());
            Assert.IsTrue(typeof(Double).IsPrimitiveType());
            Assert.IsTrue(typeof(Decimal).IsPrimitiveType());

            Assert.IsTrue(typeof(SByte).IsPrimitiveType());
            Assert.IsTrue(typeof(UInt16).IsPrimitiveType());
            Assert.IsTrue(typeof(UInt32).IsPrimitiveType());
            Assert.IsTrue(typeof(UInt64).IsPrimitiveType());

            Assert.IsTrue(typeof(DateTime).IsPrimitiveType());
            Assert.IsTrue(typeof(DateTimeOffset).IsPrimitiveType());
            Assert.IsTrue(typeof(TimeSpan).IsPrimitiveType());

            Assert.IsFalse(typeof(TestObject).IsPrimitiveType());
            
            Assert.IsTrue(typeof(char?).IsPrimitiveType());
            Assert.IsTrue(typeof(Guid?).IsPrimitiveType());

            Assert.IsTrue(typeof(bool?).IsPrimitiveType());
            Assert.IsTrue(typeof(byte?).IsPrimitiveType());
            Assert.IsTrue(typeof(short?).IsPrimitiveType());
            Assert.IsTrue(typeof(int?).IsPrimitiveType());
            Assert.IsTrue(typeof(long?).IsPrimitiveType());
            Assert.IsTrue(typeof(float?).IsPrimitiveType());
            Assert.IsTrue(typeof(double?).IsPrimitiveType());
            Assert.IsTrue(typeof(decimal?).IsPrimitiveType());

            Assert.IsTrue(typeof(sbyte?).IsPrimitiveType());
            Assert.IsTrue(typeof(ushort?).IsPrimitiveType());
            Assert.IsTrue(typeof(uint?).IsPrimitiveType());
            Assert.IsTrue(typeof(ulong?).IsPrimitiveType());

            Assert.IsTrue(typeof(DateTime?).IsPrimitiveType());
            Assert.IsTrue(typeof(DateTimeOffset?).IsPrimitiveType());
            Assert.IsTrue(typeof(TimeSpan?).IsPrimitiveType());

            Assert.IsTrue("test".IsPrimitiveType());
            Assert.IsTrue(1.IsPrimitiveType());

            Assert.IsFalse(new TestObject().IsPrimitiveType());
        }
    }


    /// <summary>
    /// Example class to use in tests.
    /// </summary>
    public class TestObject
    {
#pragma warning disable CS0414
        private string _firstName = "Will";
#pragma warning disable CS0414

        /// <summary>
        /// Gets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        private string LastName => "Doe";

        /// <summary>
        /// Gets the age.
        /// </summary>
        /// <value>
        /// The age.
        /// </value>
        public int Age { get; private set; }

        public TestObject()
        {
            Age = 18;
        }
    }
}