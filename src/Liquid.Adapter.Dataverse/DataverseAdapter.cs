using Microsoft.Crm.Sdk.Messages;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.PowerPlatform.Dataverse.Client.Extensions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;

namespace Liquid.Adapter.Dataverse
{
	///<inheritdoc/>
	public class DataverseAdapter : ILiquidDataverseAdapter
	{
		private readonly IDataverseClientFactory _serviceFactory;
		private readonly IOrganizationServiceAsync _client;
		
		/// <summary>
		/// Initialize a new instance of <see cref="DataverseAdapter"/>
		/// </summary>
		/// <param name="serviceFactory"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public DataverseAdapter(IDataverseClientFactory serviceFactory)
		{
			_serviceFactory = serviceFactory ?? throw new ArgumentNullException(nameof(serviceFactory));
			_client = _serviceFactory.GetClient();
		}

		public async Task<Entity> GetById(Guid id, string entityName, ColumnSet? columns = null)
		{
			if (columns == null)
				columns = new ColumnSet(true);

			var result = await _client.RetrieveAsync(entityName, id, columns);

			return result;
		}

		///<inheritdoc/>
		public async Task<List<Entity>> ListByFilter(string entityName, FilterExpression? filter = null, ColumnSet? columns = null)
		{
			List<Entity> results = new List<Entity>();

			QueryExpression queryData = new QueryExpression(entityName);

			if (filter != null)
				queryData.Criteria = filter;

			if (columns != null)
				queryData.ColumnSet = columns;

			var result = await _client.RetrieveMultipleAsync(queryData);
			if (result?.Entities != null)
			{
				foreach (var item in result.Entities)
				{
					results.Add(item);
				}
			}

			return results;
		}

		///<inheritdoc/>
		public async Task<List<Entity>> ListByFilter(string entityName, QueryExpression query)
		{
			if (query is null)
			{
				throw new ArgumentNullException(nameof(query));
			}

			List<Entity> results = new List<Entity>();

			var result = await _client.RetrieveMultipleAsync(query);

			if (result?.Entities != null)
			{
				foreach (var item in result.Entities)
				{
					results.Add(item);
				}
			}

			return results;
		}

		///<inheritdoc/>
		public async Task<EntityMetadata> GetMetadata(string entityName)
		{
			var retrieveEntityRequest = new RetrieveEntityRequest
			{
				EntityFilters = EntityFilters.All,
				LogicalName = entityName
			};
			var response = await _client.ExecuteAsync(retrieveEntityRequest);
			var metadata = (RetrieveEntityResponse)response;
			return metadata.EntityMetadata;
		}

		///<inheritdoc/>
		public async Task SetState(EntityReference entity, string state, string status)
		{
			var setStateRequest = new SetStateRequest()
			{
				EntityMoniker = new EntityReference
				{
					Id = entity.Id,
					LogicalName = entity.LogicalName,
				},
				State = new OptionSetValue(int.Parse(state)),
				Status = new OptionSetValue(int.Parse(status))
			};

			await _client.ExecuteAsync(setStateRequest);
		}

		///<inheritdoc/>
		public async Task Upsert(Entity entity)
		{
			var request = new UpsertRequest()
			{
				Target = entity
			};

			await _client.ExecuteAsync(request);
		}

		///<inheritdoc/>
		public Task<Guid> Create(Entity targetEntity, bool bypassSynchronousCustomLogic = false, bool suppressPowerAutomateTrigger = false, bool suppressDuplicateDetectionRules = true)
		{
			var createRequest = new CreateRequest
			{
				Target = targetEntity
			};

			createRequest.Parameters.Add("BypassCustomPluginExecution", bypassSynchronousCustomLogic);
			createRequest.Parameters.Add("SuppressCallbackRegistrationExpanderJob", suppressPowerAutomateTrigger);
			createRequest.Parameters.Add("SuppressDuplicateDetection", suppressDuplicateDetectionRules);

			var resultCreate = (CreateResponse)_client.Execute(createRequest);

			return Task.FromResult(resultCreate.id);
		}

		///<inheritdoc/>
		public async Task Update(Entity entity, bool useOptimisticConcurrency = false, bool bypassSynchronousCustomLogic = false, bool suppressPowerAutomateTrigger = false, bool suppressDuplicateDetectionRules = true)
		{
			var updateRequest = new UpdateRequest
			{
				Target = entity
			};

			if (useOptimisticConcurrency)
			{
				updateRequest.ConcurrencyBehavior = ConcurrencyBehavior.IfRowVersionMatches;
			}
			updateRequest.Parameters.Add("BypassCustomPluginExecution", bypassSynchronousCustomLogic);
			updateRequest.Parameters.Add("SuppressCallbackRegistrationExpanderJob", suppressPowerAutomateTrigger);
			updateRequest.Parameters.Add("SuppressDuplicateDetection", suppressDuplicateDetectionRules);

			await _client.ExecuteAsync(updateRequest);
		}

		///<inheritdoc/>
		public async Task Delete(Guid id, string entityName, bool bypassSynchronousLogic = false, bool useOptimisticConcurrency = false)
		{
			var deleteRequest = new DeleteRequest
			{
				Target = new EntityReference(entityName, id)
			};

			if (useOptimisticConcurrency)
			{
				deleteRequest.ConcurrencyBehavior = ConcurrencyBehavior.IfRowVersionMatches;
			}

			deleteRequest.Parameters.Add("BypassCustomPluginExecution", bypassSynchronousLogic);

			await _client.ExecuteAsync(deleteRequest);
		}
		
	}
}