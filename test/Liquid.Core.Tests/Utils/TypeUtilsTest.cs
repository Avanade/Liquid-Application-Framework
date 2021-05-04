using System.Diagnostics.CodeAnalysis;
using Liquid.Core.Configuration;
using Liquid.Core.Tests.Configuration.Entities;
using NUnit.Framework;
using Liquid.Core.Utils;

namespace Liquid.Core.Tests.Utils
{
    /// <summary>
    /// Type Utils Tests.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class TypeUtilsTest
    {
        /// <summary>
        /// Verifies the get types to register.
        /// </summary>
        [Test]
        public void Verify_GetTypesToRegister()
        {
            var types = TypeUtils.GetTypesToRegister(typeof(ILightConfiguration<>), new[] { GetType().Assembly });
            Assert.Greater(types.Length, 0);
        }

        /// <summary>
        /// Verifies the type of the is concrete.
        /// </summary>
        [Test]
        public void Verify_IsConcreteType()
        {
            var result = GetType().IsConcreteType();
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Verifies the is abstract.
        /// </summary>
        [Test]
        public void Verify_IsAbstract()
        {
            var result = GetType().IsAbstract();
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Verifies the is generic type definition of.
        /// </summary>
        [Test]
        public void Verify_IsGenericTypeDefinitionOf()
        {
            var result = typeof(CustomSettingConfiguration).IsGenericTypeDefinitionOf(typeof(ILightConfiguration<>));
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Verifies the type of the is generic.
        /// </summary>
        [Test]
        public void Verify_IsGenericType()
        {
            var result = typeof(ILightConfiguration<>).IsGenericType();
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Verifies the is generic implementation of.
        /// </summary>
        [Test]
        public void Verify_IsGenericImplementationOf()
        {
            var result = typeof(ILightConfiguration<>).IsGenericImplementationOf(typeof(CustomSettingConfiguration));
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Verifies the is variant version of.
        /// </summary>
        [Test]
        public void Verify_IsVariantVersionOf()
        {
            var result = typeof(CustomSettingConfiguration).IsVariantVersionOf(typeof(ILightConfiguration<>));
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Verifies the is generic type definition.
        /// </summary>
        [Test]
        public void Verify_IsGenericTypeDefinition()
        {
            var result = typeof(ILightConfiguration<>).IsGenericTypeDefinition();
            Assert.IsTrue(result);
        }
    }
}