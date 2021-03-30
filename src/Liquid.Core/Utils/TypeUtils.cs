using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Liquid.Core.Utils
{
    /// <summary>
    /// Type Helper Extensions Class.
    /// </summary>
    public static class TypeUtils
    {
        /// <summary>
        /// Gets the types to register.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns></returns>
        public static Type[] GetTypesToRegister(Type serviceType, IEnumerable<Assembly> assemblies)
        {
            var typesToRegister = assemblies
                .Distinct()
                .Where(assembly => !assembly.IsDynamic)
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsConcreteType() && !type.GetTypeInfo().IsGenericTypeDefinition && ServiceIsAssignableFromImplementation(serviceType, type));

            return typesToRegister.ToArray();
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
        /// Services the is assignable from implementation.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="implementation">The implementation.</param>
        /// <returns></returns>
        private static bool ServiceIsAssignableFromImplementation(Type service, Type implementation)
        {
            if (service.IsAssignableFrom(implementation) || service.IsGenericTypeDefinitionOf(implementation)) return true;
            if (implementation.GetInterfaces().Any(type => type.IsGenericImplementationOf(service))) return true;

            var type1 = implementation.BaseType() ?? (implementation != typeof(object) ? typeof(object) : null);

            for (var type2 = type1; type2 != null as Type; type2 = type2.BaseType())
            {
                if (type2.IsGenericImplementationOf(service))
                    return true;
            }
            return false;
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
        private static Type BaseType(this Type type)
        {
            return type.GetTypeInfo().BaseType;
        }

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">obj</exception>
        /// <exception cref="ArgumentOutOfRangeException">Couldn't find field {fieldName} in type {objType.FullName}</exception>
        public static object GetFieldValue(this object obj, string fieldName)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            Type objType = obj.GetType();
            var fieldInfo = GetFieldInfo(objType, fieldName);
            if (fieldInfo == null)
                throw new ArgumentOutOfRangeException(fieldName,
                    $"Couldn't find field {fieldName} in type {objType.FullName}");
            return fieldInfo.GetValue(obj);
        }

        /// <summary>
        /// Sets the field value.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="val">The value.</param>
        /// <exception cref="ArgumentNullException">obj</exception>
        /// <exception cref="ArgumentOutOfRangeException">Couldn't find field {fieldName} in type {objType.FullName}</exception>
        public static void SetFieldValue(this object obj, string fieldName, object val)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            Type objType = obj.GetType();
            var fieldInfo = GetFieldInfo(objType, fieldName);
            if (fieldInfo == null)
                throw new ArgumentOutOfRangeException(fieldName,
                    $"Couldn't find field {fieldName} in type {objType.FullName}");
            fieldInfo.SetValue(obj, val);
        }

        /// <summary>
        /// Gets the field information.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        public static FieldInfo GetFieldInfo(Type type, string fieldName)
        {
            FieldInfo fieldInfo;
            do
            {
                fieldInfo = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                type = type.BaseType;
            } while (fieldInfo == null && type != null);

            return fieldInfo;
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">obj</exception>
        /// <exception cref="ArgumentOutOfRangeException">Couldn't find property {propertyName} in type {objType.FullName}</exception>
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            Type objType = obj.GetType();
            var propertyInfo = GetPropertyInfo(objType, propertyName);
            if (propertyInfo == null)
                throw new ArgumentOutOfRangeException(propertyName,
                    $"Couldn't find property {propertyName} in type {objType.FullName}");
            return propertyInfo.GetValue(obj, null);
        }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="val">The value.</param>
        /// <exception cref="ArgumentNullException">obj</exception>
        /// <exception cref="ArgumentOutOfRangeException">Couldn't find property {propertyName} in type {objType.FullName}</exception>
        public static void SetPropertyValue(this object obj, string propertyName, object val)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            Type objType = obj.GetType();
            var propertyInfo = GetPropertyInfo(objType, propertyName);
            if (propertyInfo == null)
                throw new ArgumentOutOfRangeException(propertyName,
                    $"Couldn't find property {propertyName} in type {objType.FullName}");
            propertyInfo.SetValue(obj, val, null);
        }

        /// <summary>
        /// Gets the property information.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo(Type type, string propertyName)
        {
            PropertyInfo propertyInfo;
            do
            {
                propertyInfo = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                type = type.BaseType;
            } while (propertyInfo == null && type != null);

            return propertyInfo;
        }
    }
}
