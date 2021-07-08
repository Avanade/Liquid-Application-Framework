using Liquid.Core.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Core.Settings
{
    /// <summary>
    /// Represents the Culture Configuration inside appsettings.json.
    /// </summary>
    [LiquidSectionName("liquid:culture")]
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