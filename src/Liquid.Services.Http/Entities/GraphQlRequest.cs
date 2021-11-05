using Newtonsoft.Json;

namespace Liquid.Services.Http.Entities
{
    /// <summary>
    /// GraphQL request class. Represents a Query that can be fetched to a GraphQL Server.
    /// For more information <see href="http://graphql.org/learn/serving-over-http/#post-request"/>
    /// </summary>
    public class GraphQlRequest
    {
        /// <summary>
        /// Gets or sets the GraphQL query.
        /// </summary>
        /// <value>
        /// The query.
        /// </value>
        [JsonProperty("query")]
        public string Query { get; set; }

        /// <summary>
        /// Gets or sets the GraphQL variables.
        /// </summary>
        /// <value>
        /// The variables.
        /// </value>
        [JsonProperty("variables")]
        public object Variables { get; set; }
    }
}