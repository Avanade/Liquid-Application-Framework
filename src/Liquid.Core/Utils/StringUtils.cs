using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Liquid.Core.Utils
{
    /// <summary>
    /// String Extensions Class.
    /// </summary>
    public static class StringUtils
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
    }
}