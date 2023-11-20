using Microsoft.Xrm.Sdk;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace CustomerRegistration.Infra.Dataverse
{
    /// <summary>
    /// Extension methods of <see cref="Entity"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class EntityExtensions
    {

        /// <summary>
        /// Converte os <see cref="AttributeCollection"/>
        /// para JsonString.
        /// </summary>
        /// <param name="entity">Entidade origem.</param>
        /// <returns></returns>
        public static string ToJsonString(this Entity entity)
        {
            var resultSet = new Dictionary<string, object>();

            foreach (var attribute in entity.Attributes)
            {
                resultSet.Add(attribute.Key, attribute.Value);
            }

            return JsonSerializer.Serialize(resultSet);
        }

    }
}
