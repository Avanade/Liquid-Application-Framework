using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Liquid.Messaging.Configuration;
using Liquid.Messaging.Extensions;
using NUnit.Framework;

namespace Liquid.Messaging.Gcp.Tests.UnitTests
{
    /// <summary>
    /// Common Tests
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CommonTest
    {
        /// <summary>
        /// Verifies the can read custom parameters.
        /// </summary>
        [Test]
        public void Verify_Can_Read_Custom_Parameters()
        {
            var parameters = new List<CustomParameter>
            {
                new CustomParameter { Key = "key1", Value = "1" },
                new CustomParameter { Key = "key2", Value = "6:12:14:45" }
            };

            var value1 = parameters.GetCustomParameter<int>("key1");
            Assert.AreEqual(1, value1);

            var value2 = parameters.GetCustomParameter<TimeSpan>("key2");
            Assert.AreEqual(new TimeSpan(6, 12, 14, 45).TotalSeconds, value2.TotalSeconds);
        }

        /// <summary>
        /// Verifies the can read default custom parameters.
        /// </summary>
        [Test]
        public void Verify_Can_Read_Default_Custom_Parameters()
        {
            var parameters = new List<CustomParameter>();

            var value1 = parameters.GetCustomParameter<int>("key1", 1);
            Assert.AreEqual(1, value1);


            parameters = new List<CustomParameter>
            {
                new CustomParameter { Key = "key3", Value = "3" }
            };
            var value2 = parameters.GetCustomParameter<TimeSpan>("key2", new TimeSpan(6, 12, 14, 45));
            Assert.AreEqual(new TimeSpan(6, 12, 14, 45).TotalSeconds, value2.TotalSeconds);
        }

        /// <summary>
        /// Verifies the custom parameters can throw exception.
        /// </summary>
        [Test]
        public void Verify_Custom_Parameters_Can_Throw_Exception()
        {
            var parameters = new List<CustomParameter>();

            Assert.Throws<ArgumentOutOfRangeException>(() => parameters.GetCustomParameter<int>("key1"));

            parameters = new List<CustomParameter>
            {
                new CustomParameter { Key = "key3", Value = "3" }
            };
            Assert.Throws<ArgumentOutOfRangeException>(() => parameters.GetCustomParameter<TimeSpan>("key2"));
            Assert.Throws<ArgumentOutOfRangeException>(() => parameters.GetCustomParameter<Type>("key3"));
        }
    }
}