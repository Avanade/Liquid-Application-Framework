using System;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Services.Attributes
{
    /// <summary>
    /// Custom attribute for request/response data in http connections.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class FormFieldAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        public string FieldName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormFieldAttribute"/> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        public FormFieldAttribute(string fieldName)
        {
            FieldName = fieldName;
        }
    }
}