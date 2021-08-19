using System.Collections.Generic;
using System.Linq;
namespace Liquid.Messaging.Extensions
{
    /// <summary>
    /// Util Extensions Class
    /// </summary>
    public static class CommonExtensions
    {
        private const string EventContextAggregationId = "Event_ContextAggregationId_xzop";
        private const string EventContextEventId = "Event_ContextId_xzop";

        /// <summary>
        /// The Gzip content type
        /// </summary>
        public const string GZipContentType = "application/gzip";

        /// <summary>
        /// The content type header
        /// </summary>
        public const string ContentTypeHeader = "ContentType";

        /// <summary>
        /// Adds the range from the elements dictionary to the source dictionary. If the element from elements dictionary alreads exists in source, it will be discarded.
        /// </summary>
        /// <param name="source">The source dictionary.</param>
        /// <param name="elements">The elements to be added to source.</param>
        public static void AddRange(this IDictionary<string, object> source, IDictionary<string, object> elements)
        {
            if (elements == null || !elements.Any()) return;

            foreach (var element in elements)
            {
                if (!source.ContainsKey(element.Key)) source.Add(element);
            }
        }

        /// <summary>
        /// Gets the aggregation identifier from context, if there isn't one, it will return a new Guid and save it.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        //public static string GetAggregationId(this ILightContext context)
        //{
        //    var aggregationId = context.GetContextDataValue<string>(EventContextAggregationId);
        //    if (string.IsNullOrEmpty(aggregationId))
        //    {
        //        aggregationId = Guid.NewGuid().ToString();
        //        context.AddOrReplaceContextValue(EventContextAggregationId, aggregationId);
        //    }
        //    return aggregationId;
        //}

        /// <summary>
        /// Sets the aggregation identifier to the context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="aggregationId">The aggregation identifier.</param>
        //public static void SetAggregationId(this ILightContext context, string aggregationId)
        //{
        //    context.AddOrReplaceContextValue(EventContextAggregationId, aggregationId);
        //}

        /// <summary>
        /// Sets the message identifier.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="eventId">The message identifier.</param>
        //public static void SetMessageId(this ILightContext context, string eventId)
        //{
        //    context.AddOrReplaceContextValue(EventContextEventId, eventId);
        //}
    }
}