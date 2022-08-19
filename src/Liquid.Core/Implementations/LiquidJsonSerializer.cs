using Liquid.Core.Interfaces;
using Liquid.Core.Utils;

namespace Liquid.Core.Implementations
{
    /// <summary>
    /// Implementation of Liquid Serializer to Json.
    /// </summary>
    public class LiquidJsonSerializer : ILiquidSerializer
    {
        /// <summary>
        /// Serializes object to json string.
        /// </summary>
        /// <param name="content">object that shoud be serialized.</param>
        /// <returns></returns>
        public string Serialize<T>(T content)
        {
            return content.ToJson();
        }
    }
}
