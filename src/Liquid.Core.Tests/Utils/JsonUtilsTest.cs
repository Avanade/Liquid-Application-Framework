using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Liquid.Core.Utils;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Liquid.Core.Tests.Utils
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class JsonUtilsTest
    {
        /// <summary>
        /// Verifies to json bytes.
        /// </summary>
        [Test]
        public void Verify_ToJsonBytes()
        {
            var sut = new
            {
                firstName = "Markoff",
                lastName = "Chaney",
                dateOfBirth = new
                {
                    year = 1901,
                    month = 4,
                    day = 30
                }
            };

            var json = sut.ToJsonBytes();
            Assert.Greater(json.Length, 0);

            sut = null;
            json = sut.ToJsonBytes();
            Assert.IsNull(json);
        }

        /// <summary>
        /// Verifies to json.
        /// </summary>
        [Test]
        public void Verify_ToJson()
        {
            var expected = @"{""firstName"":""Markoff"",""lastName"":""Chaney""}";
            var sut = new
            {
                firstName = "Markoff",
                lastName = "Chaney"
            };
            var json = sut.ToJson();
            Assert.AreEqual(expected, json);

            json = sut.ToJson(new JsonSerializerSettings { CheckAdditionalContent = false });
            Assert.AreEqual(expected, json);

            Assert.Throws<ArgumentNullException>(() => sut.ToJson(null));

            sut = null;
            json = sut.ToJson();
            Assert.IsNull(json);

            json = sut.ToJson(new JsonSerializerSettings { CheckAdditionalContent = false });
            Assert.IsNull(json);


        }

        /// <summary>
        /// Verifies to canonical json.
        /// </summary>
        [Test]
        public void Verify_ToIdentedJson()
        {
            var expected = @"{
  ""firstName"": ""Markoff"",
  ""lastName"": ""Chaney""
}";
            var sut = new
            {
                firstName = "Markoff",
                lastName = "Chaney"
            };
            var json = sut.ToIndentedJson();
            Assert.AreEqual(expected, json);

            sut = null;
            json = sut.ToIndentedJson();
            Assert.IsNull(json);
        }

        /// <summary>
        /// Verifies the parse json.
        /// </summary>
        [Test]
        public void Verify_ParseJson()
        {
            var sut = @"{""firstName"": ""Markoff"", ""lastName"": ""Chaney""}";
            var obj = sut.ParseJson<Person>();
            Assert.AreEqual("Markoff", obj.FirstName);
            Assert.AreEqual("Chaney", obj.LastName);

            obj = sut.ParseJson<Person>(new JsonSerializerSettings { CheckAdditionalContent = false });
            Assert.AreEqual("Markoff", obj.FirstName);
            Assert.AreEqual("Chaney", obj.LastName);

            Assert.Throws<ArgumentNullException>(() => sut.ParseJson<Person>(null));

            obj = string.Empty.ParseJson<Person>();
            Assert.IsNull(obj);

            obj = string.Empty.ParseJson<Person>(new JsonSerializerSettings { CheckAdditionalContent = false });
            Assert.IsNull(obj);

        }

        /// <summary>
        /// Verifies the parse json bytes.
        /// </summary>
        [Test]
        public void Verify_ParseJsonBytes()
        {
            var sut = new Person
            {
                FirstName = "Markoff",
                LastName = "Chaney",
                Age = 10
            };
            var json = sut.ToJsonBytes();
            Assert.Greater(json.Length, 0);
            var obj = json.ParseJson<Person>();
            Assert.AreEqual("Markoff", obj.FirstName);
            Assert.AreEqual("Chaney", obj.LastName);

            obj = json.ParseJson<Person>(new JsonSerializerSettings { CheckAdditionalContent = false });
            Assert.AreEqual("Markoff", obj.FirstName);
            Assert.AreEqual("Chaney", obj.LastName);

            Assert.Throws<ArgumentNullException>(() => json.ParseJson<Person>(null));

            json = null;
            obj = json.ParseJson<Person>();
            Assert.IsNull(obj);

            obj = json.ParseJson<Person>(new JsonSerializerSettings { CheckAdditionalContent = false });
            Assert.IsNull(obj);
        }
    }


    /// <summary>
    /// Example class to use in tests.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        /// <value>
        /// The age.
        /// </value>
        public int Age { get; set; }
    }
}