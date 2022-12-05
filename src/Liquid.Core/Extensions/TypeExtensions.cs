using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Liquid.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class TypeExtensions
    {
        /// <summary>
        /// Implements the generic interface.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <returns></returns>
        public static bool ImplementGenericInterface(this Type type, Type interfaceType)
            => type.IsGenericType(interfaceType) || type.GetTypeInfo().ImplementedInterfaces.Any(@interface => @interface.IsGenericType(interfaceType));

        /// <summary>
        /// Determines whether [is generic type] [the specified generic type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="genericType">Type of the generic.</param>
        /// <returns>
        ///   <c>true</c> if [is generic type] [the specified generic type]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsGenericType(this Type type, Type genericType)
            => type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == genericType;

        /// <summary>
        /// Determines whether [is generic type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if [is generic type] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsGenericType(this Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }

        /// <summary>
        /// Determines whether [is concrete type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if [is concrete type] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsConcreteType(this Type type)
        {
            return !type.IsAbstract() && !type.IsArray && type != typeof(object) && !typeof(Delegate).IsAssignableFrom(type);
        }

        /// <summary>
        /// Determines whether this instance is abstract.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is abstract; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAbstract(this Type type)
        {
            return type.GetTypeInfo().IsAbstract;
        }
        /// <summary>
        /// Determines whether [is generic type definition of] [the specified type to check].
        /// </summary>
        /// <param name="genericTypeDefinition">The generic type definition.</param>
        /// <param name="typeToCheck">The type to check.</param>
        /// <returns>
        ///   <c>true</c> if [is generic type definition of] [the specified type to check]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsGenericTypeDefinitionOf(this Type genericTypeDefinition, Type typeToCheck)
        {
            return typeToCheck.IsGenericType() && typeToCheck.GetGenericTypeDefinition() == genericTypeDefinition;
        }        

        /// <summary>
        /// Determines whether [is generic implementation of] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>
        ///   <c>true</c> if [is generic implementation of] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsGenericImplementationOf(this Type type, Type serviceType)
        {
            if (type == serviceType || serviceType.IsVariantVersionOf(type))
                return true;
            return type.IsGenericType() && serviceType.IsGenericTypeDefinition() && type.GetGenericTypeDefinition() == serviceType;
        }

        /// <summary>
        /// Determines whether [is variant version of] [the specified other type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="otherType">Type of the other.</param>
        /// <returns>
        ///   <c>true</c> if [is variant version of] [the specified other type]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsVariantVersionOf(this Type type, Type otherType)
        {
            return type.IsGenericType() && otherType.IsGenericType() && type.GetGenericTypeDefinition() == otherType.GetGenericTypeDefinition() && type.IsAssignableFrom(otherType);
        }

        /// <summary>
        /// Determines whether [is generic type definition].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if [is generic type definition] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsGenericTypeDefinition(this Type type)
        {
            return type.GetTypeInfo().IsGenericTypeDefinition;
        }

        /// <summary>
        /// Bases the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static Type GetBaseType(this Type type)
        {
            return type.GetTypeInfo().BaseType;
        }
    }
}
