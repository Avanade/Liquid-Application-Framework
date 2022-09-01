using Liquid.Core.Utils;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;

namespace Liquid.Core.Extensions
{
    /// <summary>
    /// Object extensions class.
    /// </summary>
    public static class ObjectExtension
    {
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
            return obj.GetType().IsPrimitive;
        }

        /// <summary>
        /// Converts the object to json string.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="options">Provides options to be used with System.Text.Json.JsonSerializer.</param>
        public static string ToJsonString(this object source, JsonSerializerOptions options = null)
        {
            return source == null ? null : JsonSerializer.Serialize(source, options);
        }

        /// <summary>
        /// Converts the object to json bytes.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="options">Provides options to be used with System.Text.Json.JsonSerializer.</param>
        public static byte[] ToJsonBytes(this object source, JsonSerializerOptions options = null)
        {
            if (source == null)
                return new byte[0];
            var instring = JsonSerializer.Serialize(source, options);

            return Encoding.Default.GetBytes(instring);
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
            var fieldInfo = TypeUtils.GetFieldInfo(objType, fieldName);
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
            var fieldInfo =TypeUtils.GetFieldInfo(objType, fieldName);
            if (fieldInfo == null)
                throw new ArgumentOutOfRangeException(fieldName,
                    $"Couldn't find field {fieldName} in type {objType.FullName}");
            fieldInfo.SetValue(obj, val);
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
            var propertyInfo = TypeUtils.GetPropertyInfo(objType, propertyName);
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
            var propertyInfo = TypeUtils.GetPropertyInfo(objType, propertyName);
            if (propertyInfo == null)
                throw new ArgumentOutOfRangeException(propertyName,
                    $"Couldn't find property {propertyName} in type {objType.FullName}");
            propertyInfo.SetValue(obj, val, null);
        }
    }
}

