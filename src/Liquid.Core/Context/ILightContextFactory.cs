namespace Liquid.Core.Context
{
    /// <summary>
    /// The context factory, responsible for get the current context.
    /// </summary>
    public interface ILightContextFactory
    {
        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <returns></returns>
        ILightContext GetContext();
    }
}