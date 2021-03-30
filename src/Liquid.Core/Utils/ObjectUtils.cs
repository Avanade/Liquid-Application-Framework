using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace Liquid.Core.Utils
{
    /// <summary>
    /// Object extensions class.
    /// </summary>
    public static class ObjectUtils
    {
        /// <summary>
        /// Converts object to dictionary.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static IDictionary<string, object> ToDictionary(this object source)
        {
            return source.ToDictionary<object>();
        }

        /// <summary>
        /// Converts object to dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static IDictionary<string, T> ToDictionary<T>(this object source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), "Unable to convert object to a dictionary. The source object is null.");

            var dictionary = new Dictionary<string, T>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
                AddPropertyToDictionary(property, source, dictionary);
            return dictionary;
        }

        /// <summary>
        /// Adds the property to dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property">The property.</param>
        /// <param name="source">The source.</param>
        /// <param name="dictionary">The dictionary.</param>
        private static void AddPropertyToDictionary<T>(PropertyDescriptor property, object source, Dictionary<string, T> dictionary)
        {
            var value = property.GetValue(source);
            if (value.IsOfType<T>())
                dictionary.Add(property.Name, (T)value);
        }

        /// <summary>
        /// Determines whether [is of type] [the specified value].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if [is of type] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOfType<T>(this object value)
        {
            return value is T;
        }

        /// <summary>
        /// Determines whether this instance is datetime.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        ///   <c>true</c> if the specified expression is datetime; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDatetime(this object expression)
        {
            return expression.ToDatetime(out DateTime? _);
        }

        /// <summary>
        /// Converts object to date time.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="outer">The outer.</param>
        /// <returns></returns>
        public static bool ToDatetime(this object expression, out DateTime? outer)
        {
            outer = null;
            if (expression == null)
                return false;

            if (expression.IsOfType<DateTime>())
                return true;

            if (DateTime.TryParseExact(
                Convert.ToString(expression, CultureInfo.InvariantCulture),
                DateTimeFormatInfo.CurrentInfo?.GetAllDateTimePatterns('d'),
                CultureInfo.CurrentCulture, DateTimeStyles.None, out var parsed))
                outer = parsed;

            return outer.HasValue;
        }

        /// <summary>
        /// Determines whether this instance is boolean.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        ///   <c>true</c> if the specified expression is boolean; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBoolean(this object expression)
        {
            return expression.ToBoolean(out bool? _);
        }

        /// <summary>
        /// Converts object to boolean.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="outer">The outer.</param>
        /// <returns></returns>
        public static bool ToBoolean(this object expression, out bool? outer)
        {
            outer = null;
            if (expression == null)
                return false;

            if (bool.TryParse(Convert.ToString(expression, CultureInfo.InvariantCulture), out var parsed))
                outer = parsed;
            return outer.HasValue;
        }

        /// <summary>
        /// Determines whether this instance is unique identifier.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        ///   <c>true</c> if the specified expression is unique identifier; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsGuid(this object expression)
        {
            return expression.ToGuid(out _);
        }

        /// <summary>
        /// Converts object to  unique identifier.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="outer">The outer.</param>
        /// <returns></returns>
        private static bool ToGuid(this object expression, out Guid? outer)
        {
            outer = null;
            if (expression == null)
                return false;

            if (Guid.TryParse(Convert.ToString(expression, CultureInfo.InvariantCulture), out var parsed))
                outer = parsed;
            return outer.HasValue;
        }

        /// <summary>
        /// Determines whether this instance is numeric.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        ///   <c>true</c> if the specified expression is numeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumeric(this object expression)
        {
            return expression != null && double.TryParse(expression.ToString(), out _);
        }

        /// <summary>
        /// Determines whether this instance is integer.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        ///   <c>true</c> if the specified expression is integer; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInteger(this object expression)
        {
            return expression.ToInteger(out long? _);
        }

        /// <summary>
        /// Converts object to integer.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="outer">The outer.</param>
        /// <returns></returns>
        public static bool ToInteger(this object expression, out long? outer)
        {
            outer = null;
            if (expression == null)
                return false;

            if (long.TryParse(Convert.ToString(expression, CultureInfo.InvariantCulture),
                NumberStyles.Integer | NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowTrailingSign,
                CultureInfo.InvariantCulture, out var parsed))
                outer = parsed;
            return outer.HasValue;
        }

        /// <summary>
        /// Determines whether this instance is double.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        ///   <c>true</c> if the specified expression is double; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDouble(this object expression)
        {
            return expression.ToDouble(out double? _);
        }

        /// <summary>
        /// Converts object to double.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="outer">The outer.</param>
        /// <returns></returns>
        public static bool ToDouble(this object expression, out double? outer)
        {
            outer = null;
            if (expression == null)
                return false;

            if (double.TryParse(Convert.ToString(expression, CultureInfo.InvariantCulture),
                NumberStyles.AllowThousands | NumberStyles.AllowTrailingSign | NumberStyles.Currency | NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed))
                outer = parsed;
            return outer.HasValue;
        }

        /// <summary>
        /// Converts an object to byte array.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static byte[] ToBytes(this object obj)
        {
            if (obj == null) return Array.Empty<byte>();

            using var memoryStream = new MemoryStream();
            var binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(memoryStream, obj);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Returns a _private_ Property Value from a given Object. Uses Reflection.
        /// Throws a ArgumentOutOfRangeException if the Property is not found.
        /// </summary>
        /// <typeparam name="T">Type of the Property</typeparam>
        /// <param name="obj">Object from where the Property Value is returned</param>
        /// <param name="propName">Property name as string.</param>
        /// <returns>PropertyValue</returns>
        public static T GetPrivatePropertyValue<T>(this object obj, string propName)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            var pi = obj.GetType().GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (pi == null) throw new ArgumentOutOfRangeException(nameof(propName), $"Property {propName} was not found in Type {obj.GetType().FullName}");
            return (T)pi.GetValue(obj, null);
        }

        /// <summary>
        /// Returns a private Property Value from a given Object. Uses Reflection.
        /// Throws a ArgumentOutOfRangeException if the Property is not found.
        /// </summary>
        /// <typeparam name="T">Type of the Property</typeparam>
        /// <param name="obj">Object from where the Property Value is returned</param>
        /// <param name="propName">Property name as string.</param>
        /// <returns>PropertyValue</returns>
        public static T GetPrivateFieldValue<T>(this object obj, string propName)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            var t = obj.GetType();
            FieldInfo fi = null;
            while (fi == null && t != null)
            {
                fi = t.GetField(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                t = t.BaseType;
            }
            if (fi == null) throw new ArgumentOutOfRangeException(nameof(propName), $"Field {propName} was not found in Type {obj.GetType().FullName}");
            return (T)fi.GetValue(obj);
        }

        /// <summary>
        /// Sets a _private_ Property Value from a given Object. Uses Reflection.
        /// Throws a ArgumentOutOfRangeException if the Property is not found.
        /// </summary>
        /// <typeparam name="T">Type of the Property</typeparam>
        /// <param name="obj">Object from where the Property Value is set</param>
        /// <param name="propName">Property name as string.</param>
        /// <param name="val">Value to set.</param>
        /// <returns>PropertyValue</returns>
        public static void SetPrivatePropertyValue<T>(this object obj, string propName, T val)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            var t = obj.GetType();
            var prop = t.GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (prop == null)
                throw new ArgumentOutOfRangeException(nameof(propName), $"Property {propName} was not found in Type {obj.GetType().FullName}");
            t.InvokeMember(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance, null, obj, new object[] { val });
        }

        /// <summary>
        /// Set a private Property Value on a given Object. Uses Reflection.
        /// </summary>
        /// <typeparam name="T">Type of the Property</typeparam>
        /// <param name="obj">Object from where the Property Value is returned</param>
        /// <param name="propName">Property name as string.</param>
        /// <param name="val">the value to set</param>
        /// <exception cref="ArgumentOutOfRangeException">if the Property is not found</exception>
        public static void SetPrivateFieldValue<T>(this object obj, string propName, T val)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            var t = obj.GetType();
            FieldInfo fi = null;
            while (fi == null && t != null)
            {
                fi = t.GetField(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                t = t.BaseType;
            }
            if (fi == null) throw new ArgumentOutOfRangeException(nameof(propName), $"Field {propName} was not found in Type {obj.GetType().FullName}");
            fi.SetValue(obj, val);
        }

        /// <summary>
        /// Determines whether [is primitive type].
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        ///   <c>true</c> if [is primitive type] [the specified object]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPrimitiveType(this object obj)
        {
            return obj.GetType().IsPrimitiveType();
        }

        /// <summary>
        /// Determines whether [is primitive type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if [is primitive type] [the specified object]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPrimitiveType(this Type type)
        {
            return type.IsPrimitive ||
                   new[] {
                       typeof(Enum),
                       typeof(String),
                       typeof(Decimal),
                       typeof(DateTime),
                       typeof(DateTimeOffset),
                       typeof(TimeSpan),
                       typeof(Guid),
                       typeof(char?),
                       typeof(Guid?),
                       typeof(bool?),
                       typeof(byte?),
                       typeof(short?),
                       typeof(int?),
                       typeof(long?),
                       typeof(float?),
                       typeof(double?),
                       typeof(decimal?),
                       typeof(sbyte?),
                       typeof(ushort?),
                       typeof(uint?),
                       typeof(ulong?),
                       typeof(DateTime?),
                       typeof(DateTimeOffset?),
                       typeof(TimeSpan?)
                   }.Contains(type) ||
                   Convert.GetTypeCode(type) != TypeCode.Object ||
                   type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && IsPrimitiveType((object)type.GetGenericArguments()[0]);
        }
    }
}

