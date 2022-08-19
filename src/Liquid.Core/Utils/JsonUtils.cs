using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Liquid.Core.Utils
{
    /// <summary>
    /// Json extensions class.
    /// </summary>
    public static class JsonUtils
    {
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        /// <summary>
        /// Converts the object to json string using specific serializer options.
        /// </summary>
        /// <typeparam name="T">Type of source to serialize.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="jsonSerializerOptions">Options to control serialization.</param>
        /// <returns>Json string</returns>
        public static string ToJson<T>(this T source, JsonSerializerOptions jsonSerializerOptions = null)
        {
            jsonSerializerOptions = jsonSerializerOptions ?? _jsonSerializerOptions;
            return JsonSerializer.Serialize(source, jsonSerializerOptions);
        }

        /// <summary>
        /// Converts the object to json bytes.
        /// </summary>
        /// <typeparam name="T">Type of source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="encoderShouldEmitUTF8Identifier"> true to specify that the System.Text.UTF8Encoding.GetPreamble method returns
        /// a Unicode byte order mark; otherwise, false.</param>
        /// <param name="jsonSerializerOptions">Options to control serialization.</param>
        /// <returns>Json array of byte </returns>
        public static byte[] ToJsonBytes<T>(this T source, bool encoderShouldEmitUTF8Identifier = false, JsonSerializerOptions jsonSerializerOptions = null)
        {
            
            if (source == null)
                return new byte[0];
            jsonSerializerOptions = jsonSerializerOptions ?? _jsonSerializerOptions;
            var instring = source.ToJson(jsonSerializerOptions);
            
            return new UTF8Encoding(encoderShouldEmitUTF8Identifier).GetBytes(instring);
        }

        /// <summary>
        /// Parses a string json to a specific object.
        /// </summary>
        /// <typeparam name="T">Type of object to be parsed.</typeparam>
        /// <param name="json">The json string.</param>
        /// <param name="jsonSerializerOptions">Options to control deserialization.</param>
        /// <returns>Typed object</returns>
        public static T ParseTypedOject<T>(this string json, JsonSerializerOptions jsonSerializerOptions = null)
        {
            if (string.IsNullOrWhiteSpace(json)) return default;
            jsonSerializerOptions = jsonSerializerOptions ?? _jsonSerializerOptions;
            return JsonSerializer.Deserialize<T>(json, jsonSerializerOptions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Type of object to be parsed.</typeparam>
        /// <param name="json">The json bytes.</param>
        /// <param name="useUtf8">Define use UTF8 or not</param>
        /// <param name="jsonSerializerOptions">Options to control deserialization.</param>
        /// <returns></returns>
        public static T ParseTypedOject<T>(this byte[] json, bool useUtf8 = false, JsonSerializerOptions jsonSerializerOptions = null)
        {
            var awaiter = json.ParseTypedOjectAsync<T>(useUtf8, jsonSerializerOptions).ConfigureAwait(false).GetAwaiter();
            return awaiter.GetResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Type of object to be parsed.</typeparam>
        /// <param name="json">The json bytes.</param>
        /// <param name="useUtf8">Define use UTF8 or not</param>
        /// <param name="jsonSerializerOptions">Options to control deserialization.</param>
        /// <returns></returns>
        public static async Task<T> ParseTypedOjectAsync<T>(this byte[] json, bool useUtf8 = false, JsonSerializerOptions jsonSerializerOptions = null)
        {
            if (json == null || json.Length == 0) return default(T);
            jsonSerializerOptions = jsonSerializerOptions ?? _jsonSerializerOptions;

            return await JsonSerializer.DeserializeAsync<T>(new UTF8Encoding(useUtf8).GetString(json).ToStreamUtf8(), jsonSerializerOptions).ConfigureAwait(false);
        }
    }
}
