using System;
using System.Text;
using System.Text.Json;

namespace Liquid.Core.Extensions
{
    /// <summary>
    /// Byte Extensions Class.
    /// </summary>
    public static class ByteExtension
    {
        /// <summary>
        /// Gets the size of the kb.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static double GetKbSize(this byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return 0;
            return bytes.Length / 1024d;
        }

        /// <summary>
        /// Gets the size of the mb.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static double GetMbSize(this byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return 0;
            return bytes.Length / Math.Pow(1024, 2);
        }

        /// <summary>
        /// Gets the size of the gb.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static double GetGbSize(this byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return 0;
            return bytes.Length / Math.Pow(1024, 3);
        }

        /// <summary>
        /// Parses byte array json to a specific object type.
        /// </summary>
        /// <typeparam name="T">type of object to be parsed.</typeparam>
        /// <param name="json"></param>
        /// <param name="options">Provides options to be used with System.Text.Json.JsonSerializer.</param>
        public static T ParseJson<T>(this byte[] json, JsonSerializerOptions options = null)
        {
            if (json == null || json.Length == 0) return default;

            var result = JsonSerializer.Deserialize<T>(Encoding.Default.GetString(json), options);
            return result;
        }
    }
}