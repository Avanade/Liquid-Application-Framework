using Liquid.Core.Extensions;
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
        /// Services the is assignable from implementation.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="implementation">The implementation.</param>
        /// <returns></returns>
        private static bool ServiceIsAssignableFromImplementation(Type service, Type implementation)
        {
            if (service.IsAssignableFrom(implementation) || service.IsGenericTypeDefinitionOf(implementation)) return true;
            if (implementation.GetInterfaces().Any(type => type.IsGenericImplementationOf(service))) return true;

            var type1 = implementation.GetBaseType() ?? (implementation != typeof(object) ? typeof(object) : null);

            for (var type2 = type1; type2 != null as Type; type2 = type2.GetBaseType())
            {
                if (type2.IsGenericImplementationOf(service))
                    return true;
            }
            return false;
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
