namespace Liquid.Core.Interfaces
{
    /// <summary>
    /// Represents a set of key/value Liquid configuration properties.
    /// </summary>
    /// <typeparam name="TConfiguration">Type of settings entity.</typeparam>
    public interface ILiquidConfiguration<out TConfiguration>
    {
        /// <summary>
        /// Set of key/value configuration section properties.
        /// </summary>
        TConfiguration Settings { get; }
    }
}