using System.Diagnostics.CodeAnalysis;

namespace Liquid.Core.Settings
{
    /// <summary>
    /// Definition of scoped key type.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ScopedKey
    {
        /// <summary>
        /// Name of the scoped key.
        /// </summary>
        public string KeyName { get; set; }

        /// <summary>
        /// Indicates if this key is mandatory.
        /// </summary>
        public bool Required { get; set; } = false;
    }
}
