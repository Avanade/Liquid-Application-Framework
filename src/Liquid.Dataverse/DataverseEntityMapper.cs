using Liquid.Core.AbstractMappers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Dataverse
{
    /// <summary>
    /// Implementation of <see cref="LiquidMapper{TFrom, TTo}"/> that
    /// maps json string data to a new instance of <see cref="Entity"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DataverseEntityMapper : LiquidMapper<string, Entity>
    {
        private readonly ILiquidDataverse _dataverseAdapter;
        private Dictionary<string, EntityMetadata> _entitiesMetadata = new Dictionary<string, EntityMetadata>();

        /// <summary>
        /// Initialize a new instance of <see cref="DataverseEntityMapper"/>
        /// </summary>
        /// <param name="adapter"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DataverseEntityMapper(ILiquidDataverse adapter) : base(nameof(DataverseEntityMapper))
        {
            _dataverseAdapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
        }

        ///<inheritdoc/>
        protected override async Task<Entity> MapImpl(string jsonString, string? entityName = null)
        {
            if (entityName == null) { throw new ArgumentNullException(nameof(entityName)); }

            var entityAttributes = await GetEntityAttributes(entityName);

            var entity = JsonToEntity(entityAttributes, jsonString);

            return entity;
        }

        private async Task<List<AttributeMetadata>> GetEntityAttributes(string entityName, List<string>? noMappingFields = null)
        {
            var entityMetadata = _entitiesMetadata.FirstOrDefault(x => x.Key == entityName).Value;

            if (entityMetadata == null)
            {
                entityMetadata = await _dataverseAdapter.GetMetadata(entityName);
                _entitiesMetadata.TryAdd(entityName, entityMetadata);
            }

            var attributes = entityMetadata.Attributes?.ToList();

            if (attributes != null)
            {
                attributes = attributes.Where(p => p.IsLogical != null).ToList();
            }
            if (noMappingFields != null)
            {
                foreach (var noMappingField in noMappingFields)
                {
                    attributes = attributes?.Where(p => p.LogicalName != noMappingField)?.ToList();
                }
            }

            var listAttributes = attributes?.ToList();

            return listAttributes;
        }
        private Entity JsonToEntity(List<AttributeMetadata> attributes, string values)
        {
            var entidade = new Entity();
            var valuesObject = JsonConvert.DeserializeObject<JObject>(values);

            foreach (var atrribute in attributes)
            {
                var logicalName = atrribute.LogicalName.ToUpper();
                if (valuesObject[logicalName] != null && valuesObject[logicalName].ToString() != "")
                {

                    switch (atrribute.AttributeType.ToString())
                    {
                        case "String":
                        case "Memo":
                            entidade[atrribute.LogicalName] = (string)valuesObject[logicalName];
                            break;
                        case "Virtual":
                            var options = valuesObject[logicalName].ToList();
                            OptionSetValueCollection collectionOptionSetValues = new OptionSetValueCollection();
                            foreach (var option in options)
                            {
                                collectionOptionSetValues.Add(new OptionSetValue(int.Parse(option["Value"].ToString())));
                            }
                            entidade[atrribute.LogicalName] = collectionOptionSetValues;
                            break;
                        case "Integer":
                            entidade[atrribute.LogicalName] = (int)valuesObject[logicalName];
                            break;
                        case "Decimal":
                            entidade[atrribute.LogicalName] = (decimal)valuesObject[logicalName];
                            break;
                        case "Boolean":
                            entidade[atrribute.LogicalName] = (bool)valuesObject[logicalName];
                            break;
                        case "Picklist":
                            entidade[atrribute.LogicalName] = new OptionSetValue((int)valuesObject[logicalName]);
                            break;
                        case "DateTime":
                            entidade[atrribute.LogicalName] = Convert.ToDateTime((string)valuesObject[logicalName]);
                            break;
                        case "Money":
                            if ((int)valuesObject[logicalName] > 0)
                                entidade[atrribute.LogicalName] = new Money((int)valuesObject[logicalName]);
                            break;
                        case "Double":
                            entidade[atrribute.LogicalName] = (double)valuesObject[logicalName];
                            break;
                        case "Lookup":
                        case "Customer":
                        case "Owner":
                            entidade[atrribute.LogicalName] = new EntityReference(valuesObject[logicalName]["LogicalName"].ToString()
                               , new Guid(valuesObject[logicalName]["Id"].ToString()));
                            break;
                    }

                }
            }
            return entidade;
        }

    }
}
