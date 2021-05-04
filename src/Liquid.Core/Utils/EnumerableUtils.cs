using Liquid.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Liquid.Core.Utils
{
    /// <summary>
    /// Extensions for IEnumerable class.
    /// </summary>
    public static class EnumerableUtils
    {
        /// <summary>
        /// Executes the action in each specified item in enumerable.
        /// </summary>
        /// <typeparam name="T">Type of element inside enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action.</param>
        public static void Each<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }

        /// <summary>
        /// Produces a comma separated values of string out of an IEnumerable. 
        /// This would be useful if you want to automatically generate a CSV out of integer, string, or any other primitive data type collection or array. 
        /// </summary>
        /// <typeparam name="T">Type of element inside enumerable.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        public static string ToSeparatedString<T>(this IEnumerable<T> instance, char separator)
        {
            var array = instance?.ToArray();
            if (array == null || !array.Any()) return null;
            
            var csv = new StringBuilder();
            array.Each(value => csv.AppendFormat("{0}{1}", value, separator));
            return csv.ToString(0, csv.Length - 1);
        }

        /// <summary>
        /// Produces a comma separated values of string out of an IEnumerable. 
        /// This would be useful if you want to automatically generate a CSV out of integer, string, or any other primitive data type collection or array. 
        /// </summary>
        /// <typeparam name="T">Type of element inside enumerable.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static string ToSeparatedString<T>(this IEnumerable<T> instance)
        {
            return instance.ToSeparatedString(',');
        }

        /// <summary>
        /// Determines whether this collection is null or empty.
        /// </summary>
        /// <typeparam name="T">Type of element in collection.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>
        ///   <c>true</c> if the specified collection is null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }

        /// <summary>
        /// Determines whether a enumerable is not null or empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>
        ///   <c>true</c> if [is not null or empty] [the specified collection]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection != null && collection.Any();
        }

        /// <summary>
        /// Orders the enumerable based on a property ASC or DESC. Example "OrderBy("Name desc")"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="sortExpression">The sort expression.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">No property '" + property + "' in + " + typeof(T).Name + "'</exception>
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> list, string sortExpression)
        {
            sortExpression += "";
            var parts = sortExpression.Split(' ');
            var descending = false;

            if (parts.Length > 0 && parts[0] != "")
            {
                var property = parts[0];

                if (parts.Length > 1)
                {
                    descending = parts[1].ToLower().Contains("desc");
                }

                var prop = typeof(T).GetProperty(property);

                if (prop == null)
                {
                    throw new LightException("No property '" + property + "' in + " + typeof(T).Name + "'");
                }

                return descending ? list.OrderByDescending(x => prop.GetValue(x, null)) : list.OrderBy(x => prop.GetValue(x, null));
            }

            return list;
        }
    }

}