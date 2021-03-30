using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Liquid.Core.Configuration
{
    /// <summary>
    /// Parses the json configuration file into configuration data for Configuration Provider.
    /// </summary>
    internal class JsonConfigurationFileParser
    {
        private readonly IDictionary<string, string> _data = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly Stack<string> _context = new Stack<string>();
        private string _currentPath;

        /// <summary>
        /// Prevents a default instance of the <see cref="JsonConfigurationFileParser"/> class from being created.
        /// </summary>
        private JsonConfigurationFileParser() { }

        /// <summary>
        /// Parses the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IDictionary<string, string> Parse(Stream input) => new JsonConfigurationFileParser().ParseStream(input);

        /// <summary>
        /// Parses the stream.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private IDictionary<string, string> ParseStream(Stream input)
        {
            _data.Clear();

            var jsonLoadSettings = new JsonLoadSettings
            {
                CommentHandling = CommentHandling.Ignore,
                DuplicatePropertyNameHandling = DuplicatePropertyNameHandling.Ignore,
                LineInfoHandling = LineInfoHandling.Ignore
            };

            var configurationJsonString = input.GetConfigurationWithEnvironmentVariables();

            var doc = JObject.Parse(configurationJsonString, jsonLoadSettings);
            VisitElement(doc);

            return _data;
        }

        /// <summary>
        /// Visits the element.
        /// </summary>
        /// <param name="element">The element.</param>
        private void VisitElement(JObject element)
        {
            foreach (var property in element)
            {
                EnterContext(property.Key);
                VisitValue(property.Value);
                ExitContext();
            }
        }

        /// <summary>
        /// Visits the value.
        /// </summary>
        /// <param name="value">The value.</param>
        private void VisitValue(JToken value)
        {
            _data.TryAdd(_currentPath, value.ToString());
            switch (value.Type)
            {
                case JTokenType.Object:
                    VisitElement((JObject)value);
                    break;

                case JTokenType.Array:
                    var index = 0;
                    foreach (var arrayElement in value)
                    {
                        EnterContext(index.ToString());
                        VisitValue(arrayElement);
                        ExitContext();
                        index++;
                    }
                    break;
            }
        }

        /// <summary>
        /// Enters the context.
        /// </summary>
        /// <param name="context">The context.</param>
        private void EnterContext(string context)
        {
            _context.Push(context);
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }

        /// <summary>
        /// Exits the context.
        /// </summary>
        private void ExitContext()
        {
            _context.Pop();
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal static class JsonConfigurationFileParserExtensions
    {
        /// <summary>
        /// Gets the configuration with environment variables.
        /// </summary>
        /// <param name="configurationFileStream">The configuration file stream.</param>
        /// <returns></returns>
        public static string GetConfigurationWithEnvironmentVariables(this Stream configurationFileStream)
        {
            using var reader = new StreamReader(configurationFileStream);
            var returnValue = reader.ReadToEnd();
            var environmentVariables = Environment.GetEnvironmentVariables();

            foreach (System.Collections.DictionaryEntry environmentVariable in environmentVariables)
            {
                var value = environmentVariable.Value?.ToString() ?? string.Empty;
                returnValue = returnValue.Replace($"${{{environmentVariable.Key}}}", Regex.Replace(value, @"\\", @"\\"));
            }

            //Remove unused variables
            returnValue = Regex.Replace(returnValue, @"\$\{(.*)\}", string.Empty);
            return returnValue;
        }
    }
}