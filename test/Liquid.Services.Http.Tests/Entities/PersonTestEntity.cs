using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Liquid.Services.Http.Tests.Entities
{
    /// <summary>
    /// Person Test Entity Class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PersonTestEntity
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }
    }

    /// <summary>
    /// Data Test Entity class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DataTestEntity
    {
        [JsonProperty("test")]
        public PersonTestEntity Test { get; set; }
    }
}