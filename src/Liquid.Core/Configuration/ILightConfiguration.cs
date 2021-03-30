namespace Liquid.Core.Configuration
{
    /// <summary>
    /// The Liquid Configuration Interface.
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public interface ILightConfiguration<out TConfiguration>
    {
        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        TConfiguration Settings { get; } 
    }
}