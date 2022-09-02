using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace Liquid.Core.Extensions
{
    /// <summary>
    /// String Extensions Class.
    /// </summary>
    /// 
    [ExcludeFromCodeCoverage]
    public static class StringExtension
    {
        private static readonly Regex WordCountRegex = new Regex(@"[^\s]+", RegexOptions.Compiled);

        /// <summary>
        /// Determines whether a string contains a specified value.
        /// </summary>
        /// <param name="source">The string source.</param>
        /// <param name="value">The value to compare.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <returns>
        ///   <c>true</c> if source contains the specified value; otherwise, <c>false</c>.
        /// </returns>
        public static bool Contains(this string source, string value, StringComparison comparisonType = StringComparison.InvariantCultureIgnoreCase)
        {
            return source.IndexOf(value, comparisonType) >= 0;
        }

        /// <summary>
        /// Removes the line endings.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <returns>The string without line endings.</returns>
        public static string RemoveLineEndings(this string value)
        {
            if (string.IsNullOrEmpty(value)) { return value; }

            var lineSeparator = ((char)0x2028).ToString();
            var paragraphSeparator = ((char)0x2029).ToString();

            return value.Replace("\r\n", string.Empty)
                        .Replace("\n", string.Empty)
                        .Replace("\r", string.Empty)
                        .Replace(lineSeparator, string.Empty)
                        .Replace(paragraphSeparator, string.Empty);
        }

        /// <summary>
        /// Converts the string representation of a Guid to its Guid 
        /// equivalent. A return value indicates whether the operation 
        /// succeeded. 
        /// </summary>
        /// <param name="guid">A string containing a Guid to convert.</param>
        /// <value>
        /// <see langword="true" /> if <paramref name="guid"/> was converted 
        /// successfully; otherwise, <see langword="false" />.
        /// </value>
        /// <remarks>
        /// When this method returns, contains the Guid value equivalent to 
        /// the Guid contained in <paramref name="guid"/>, if the conversion 
        /// succeeded, or <see cref="Guid.Empty"/> if the conversion failed. 
        /// The conversion fails if the <paramref name="guid"/> parameter is a 
        /// <see langword="null" /> reference (<see langword="Nothing" /> in 
        /// Visual Basic), or is not of the correct format.  
        /// </remarks>
        public static bool IsGuid(this string guid)
        {
            return guid != null && Guid.TryParse(guid, out _);
        }

        /// <summary>
        /// Determines whether this string is a valid http url.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>
        ///   <c>true</c> if [is valid URL] [the specified text]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidHttpUrl(this string str)
        {
            return Uri.TryCreate(str, UriKind.Absolute, out _);
        }

        /// <summary>
        /// Appends to the string builder if matchers the condition.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">The value.</param>
        /// <param name="condition">if set to <c>true</c> appends to string builder.</param>
        /// <returns></returns>
        public static StringBuilder AppendIf(this StringBuilder builder, string value, bool condition)
        {
            if (condition) builder.Append(value);
            return builder;
        }

        /// <summary>
        /// Count all words in a given string, it excludes whitespaces, tabs and line breaks
        /// </summary>
        /// <param name="str">The string to count words</param>
        /// <returns>int</returns>
        public static int CountWords(this string str)
        {
            var count = 0;
            try
            {
                var matches = WordCountRegex.Matches(str);
                count = matches.Count;
            }
            catch
            {
                //left intentionally blank.
            }
            return count;
        }

        /// <summary>
        /// Converts a string to guid.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static Guid ToGuid(this string str)
        {
            Guid.TryParse(str, out var returnValue);
            return returnValue;
        }

        /// <summary>
        /// Converts string to enum object
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">String value to convert</param>
        /// <returns>Returns enum object</returns>
        public static T ToEnum<T>(this string value) where T : struct
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        /// Computes the hash of the string using a specified hash algorithm
        /// </summary>
        /// <param name="input">The string to hash</param>
        /// <param name="key">The hash key.</param>
        /// <param name="hashType">The hash algorithm to use</param>
        /// <returns>
        /// The resulting hash or an empty string on error
        /// </returns>
        public static string CreateHash(this string input, string key, HashType hashType)
        {
            try
            {
                var hash = GetComputedHash(input, key, hashType);
                var ret = new StringBuilder();

                foreach (var hashByte in hash)
                    ret.Append(hashByte.ToString("x2"));

                return ret.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the hash.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="key">The key.</param>
        /// <param name="hash">The hash.</param>
        /// <returns></returns>
        private static byte[] GetComputedHash(string input, string key, HashType hash)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var inputKey = Encoding.UTF8.GetBytes(key);

            return hash switch
            {
                HashType.HMacSha256 => new HMACSHA256(inputKey).ComputeHash(inputBytes),
                HashType.HMacSha384 => new HMACSHA384(inputKey).ComputeHash(inputBytes),
                HashType.HMacSha512 => new HMACSHA512(inputKey).ComputeHash(inputBytes),
                HashType.Sha256 => SHA256.Create().ComputeHash(inputBytes),
                HashType.Sha384 => SHA384.Create().ComputeHash(inputBytes),
                HashType.Sha512 => SHA512.Create().ComputeHash(inputBytes),
                _ => inputBytes,
            };
        }

        /// <summary>
        /// Parses a string json to a specific object.
        /// </summary>
        /// <typeparam name="T">type of object to be parsed.</typeparam>
        /// <param name="json"></param>
        /// <param name="options">Provides options to be used with System.Text.Json.JsonSerializer.</param>
        /// <returns>the object parsed from json.</returns>
        public static T ParseJson<T>(this string json, JsonSerializerOptions options = null)
        {
            if (string.IsNullOrEmpty(json)) return default;
            var result = JsonSerializer.Deserialize<T>(json, options);
            return result;
        }
        /// <summary>
        /// Gets the stream from string using UTF8 encoding.
        /// </summary>
        /// <param name="source">The string source.</param>
        /// <returns>UTF8 encoded stream.</returns>
        public static Stream ToStreamUtf8(this string source)
        {
            if (source == null) return null;
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(source));
            return stream;
        }

        /// <summary>
        /// Gets the stream from string using ASCII encoding.
        /// </summary>
        /// <param name="source">The string source.</param>
        /// <returns>ASCII encoded stream.</returns>
        public static Stream ToStreamAscii(this string source)
        {
            if (source == null) return null;
            var stream = new MemoryStream(Encoding.ASCII.GetBytes(source));
            return stream;
        }

        /// <summary>
        /// Gets the stream from string using UTF8 encoding.
        /// </summary>
        /// <param name="source">The string source.</param>
        /// <returns>Unicode encoded stream.</returns>
        public static Stream ToStreamUnicode(this string source)
        {
            if (source == null) return null;
            var stream = new MemoryStream(Encoding.Unicode.GetBytes(source));
            return stream;
        }

        /// <summary>
        /// Serializes an object to xml.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static string ToXml(this object obj)
        {
            string retVal;
            using (var ms = new MemoryStream())
            {
                var xs = new XmlSerializer(obj.GetType());
                xs.Serialize(ms, obj);
                ms.Flush();
                ms.Position = 0;
                retVal = ms.AsStringUtf8();
            }
            return retVal;
        }

        /// <summary>
        /// Parse a xml to an object.
        /// </summary>
        /// <typeparam name="T">type of object.</typeparam>
        /// <param name="str">The xml string.</param>
        /// <returns></returns>
        public static T ParseXml<T>(this string str) where T : new()
        {
            var xs = new XmlSerializer(typeof(T));
            var stream = str.ToStreamUtf8();
            if (xs.CanDeserialize(new XmlTextReader(stream)))
            {
                stream.Position = 0;
                return (T)xs.Deserialize(stream);
            }
            return default;
        }

        /// <summary>
        /// 
        /// </summary>
        public enum HashType
        {
            /// <summary>
            /// The HMACSHA256 Hash type.
            /// </summary>
            HMacSha256,
            /// <summary>
            /// The HMACSHA384 Hash type.
            /// </summary>
            HMacSha384,
            /// <summary>
            /// The HMACSHA512 Hash type.
            /// </summary>
            HMacSha512,
            /// <summary>
            /// The SHA256 Hash type.
            /// </summary>
            Sha256,
            /// <summary>
            /// The SHA384 Hash type.
            /// </summary>
            Sha384,
            /// <summary>
            /// The SHA512 Hash type.
            /// </summary>
            Sha512
        }
    }
}