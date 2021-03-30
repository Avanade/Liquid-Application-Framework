using System;
using System.Diagnostics.CodeAnalysis;
using Liquid.Core.Exceptions;

namespace Liquid.Core.Configuration
{
    /// <summary>
    /// Occurs when it is not possible to read the appsettings file.
    /// </summary>
    /// <seealso cref="Liquid.Core.Exceptions.LightException" />
    [ExcludeFromCodeCoverage]
    public class LightFileConfigurationException : LightException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LightFileConfigurationException" /> class.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        public LightFileConfigurationException(string filepath) : base($"Settings file '{filepath}' does not exists. Please check the file location")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LightFileConfigurationException" /> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="filepath">The filepath.</param>
        public LightFileConfigurationException(string filepath, Exception innerException) : base($"Settings file '{filepath}' could not be read. please check inner exception.", innerException)
        {
        }
    }
}