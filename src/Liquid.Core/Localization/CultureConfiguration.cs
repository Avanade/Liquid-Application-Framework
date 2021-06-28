using Liquid.Core.Attributes;
using Liquid.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Core.Localization
{
    /// <summary>
    /// Represents the Culture Configuration inside appsettings.json.
    /// </summary>
    [LiquidSectionName("liquid:culture")]
    [Obsolete("This class will be removed or refactored in the next release.")]
    [ExcludeFromCodeCoverage]
    public class CultureSettings
    {
        /// <summary>
        /// Gets the default culture.
        /// </summary>
        /// <value>
        /// The default culture.
        /// </value>
        public string DefaultCulture { get; set; }
    }
}