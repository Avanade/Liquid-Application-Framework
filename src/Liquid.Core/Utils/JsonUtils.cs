using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Liquid.Core.Utils
{
    /// <summary>
    /// Json extensions class.
    /// </summary>
    public static class JsonUtils
    {
        private static readonly UTF8Encoding Utf8NoBom = new UTF8Encoding(false);

        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            TypeNameHandling = TypeNameHandling.None,
            Converters = new JsonConverter[] { new StringEnumConverter() }
        };

        /// <summary>
        /// Converts the object to json bytes.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static byte[] ToJsonBytes(this object source)
        {
            if (source == null)
                return new byte[0];
            var instring = JsonConvert.SerializeObject(source, Formatting.Indented, JsonSettings);
            return Utf8NoBom.GetBytes(instring);
        }

        /// <summary>
        /// Converts the object to json string.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The string json representation of object.</returns>
        public static string ToJson(this object source)
        {
            return source == null ? null : JsonConvert.SerializeObject(source, Formatting.None, JsonSettings);
        }

        /// <summary>
        /// Converts the object to json string using specific serializer options.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="jsonSettings">The json serializer settings.</param>
        /// <returns>The string json representation of object.</returns>
        /// <exception cref="ArgumentNullException">jsonSettings</exception>
        public static string ToJson(this object source, JsonSerializerSettings jsonSettings)
        {
            if (source == null) return null;
            if (jsonSettings == null) throw new ArgumentNullException(nameof(jsonSettings));
            return JsonConvert.SerializeObject(source, jsonSettings);
        }

        /// <summary>
        /// Converts the object to indented json string.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The string json representation of object.</returns>
        public static string ToIndentedJson(this object source)
        {
            return source == null
                ? null
                : JsonConvert.SerializeObject(source, Formatting.Indented, JsonSettings);
        }

        /// <summary>
        /// Parses a string json to a specific object.
        /// </summary>
        /// <typeparam name="T">type of object to be parsed.</typeparam>
        /// <param name="json">The json string.</param>
        /// <returns>the object parsed from json.</returns>
        public static T ParseJson<T>(this string json)
        {
            if (string.IsNullOrEmpty(json)) return default;
            var result = JsonConvert.DeserializeObject<T>(json, JsonSettings);
            return result;
        }

        /// <summary>
        /// Parses a string json to a specific object.
        /// </summary>
        /// <typeparam name="T">type of object to be parsed.</typeparam>
        /// <param name="json">The json string.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>
        /// the object parsed from json.
        /// </returns>
        /// <exception cref="ArgumentNullException">settings</exception>
        public static T ParseJson<T>(this string json, JsonSerializerSettings settings)
        {
            if (string.IsNullOrEmpty(json)) return default;
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            var result = JsonConvert.DeserializeObject<T>(json, settings);
            return result;
        }

        /// <summary>
        /// Parses a Utf8 byte json to a specific object.
        /// </summary>
        /// <typeparam name="T">type of object to be parsed.</typeparam>
        /// <param name="json">The json bytes.</param>
        /// <returns>the object parsed from json.</returns>
        public static T ParseJson<T>(this byte[] json)
        {
            if (json == null || json.Length == 0) return default;
            var result = JsonConvert.DeserializeObject<T>(Utf8NoBom.GetString(json), JsonSettings);
            return result;
        }

        /// <summary>
        /// Parses a Utf8 byte json to a specific object.
        /// </summary>
        /// <typeparam name="T">type of object to be parsed.</typeparam>
        /// <param name="json">The json bytes.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>
        /// the object parsed from json.
        /// </returns>
        /// <exception cref="ArgumentNullException">settings</exception>
        public static T ParseJson<T>(this byte[] json, JsonSerializerSettings settings)
        {
            if (json == null || json.Length == 0) return default;
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            var result = JsonConvert.DeserializeObject<T>(Utf8NoBom.GetString(json), settings);
            return result;
        }
    }
}
