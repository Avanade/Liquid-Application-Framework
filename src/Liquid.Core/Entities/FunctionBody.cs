using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Liquid.Core.Entities
{
    /// <summary>
    /// The body of a function to be called.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FunctionBody
    {

        /// <summary> The name of the function to be called. </summary>
        public string Name { get; set; }
        /// <summary>
        /// A description of what the function does. The model will use this description when selecting the function and
        /// interpreting its parameters.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The parameters the function accepts, described as a JSON Schema object.
        /// <para>
        /// To assign an object to this property use <see cref="BinaryData.FromObjectAsJson{T}(T, JsonSerializerOptions)"/>.
        /// </para>
        /// <para>
        /// To assign an already formatted json string to this property use <see cref="BinaryData.FromString(string)"/>.
        /// </para>
        /// </summary>
        public BinaryData Parameters { get; set; }

        /// <summary> Initializes a new instance of <see cref="FunctionBody"/>. </summary>
        /// <param name="name"> The name of the function to be called. </param>
        /// <param name="description">
        /// A description of what the function does. The model will use this description when selecting the function and
        /// interpreting its parameters.
        /// </param>
        /// <param name="parameters"> The parameters the function accepts, described as a JSON Schema object. </param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public FunctionBody(string name, string description, BinaryData parameters)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }

            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentException($"'{nameof(description)}' cannot be null or empty.", nameof(description));
            }

            Name = name;
            Description = description;
            Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));

        }

        /// <summary>
        /// Initializes a new instance of <see cref="FunctionBody"/>.
        /// </summary>
        /// <param name="functionBody">function definition JSON string. </param>
        public FunctionBody(string functionBody)
        {
            var function = JsonSerializer.Deserialize<JsonElement>(functionBody);

            Name = function.GetProperty("name").ToString();
            Description = function.GetProperty("description").ToString();
            Parameters = BinaryData.FromObjectAsJson(function.GetProperty("parameters"));
        }
    }
}
