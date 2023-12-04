using Microsoft.Crm.Sdk.Messages;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.Adapter.Dataverse.Tests
{
    public class DataverseAdapterTests
    {
        private readonly IOrganizationServiceAsync _client;
        private readonly ILiquidDataverseAdapter _sut;
        public DataverseAdapterTests()
        {
            var clientFactory = Substitute.For<IDataverseClientFactory>();
            _client = Substitute.For<IOrganizationServiceAsync>();

            clientFactory.GetClient().Returns(_client);

            _sut = new DataverseAdapter(clientFactory);
        }

        [Fact]
        public void Ctor_WhenClientFactoryIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DataverseAdapter(null));
        }

        [Fact]
        public async Task Create_WhenClientCallResultSucessfully_ReturnGuid()
        {
            _client.Create(Arg.Any<Entity>()).Returns(Guid.NewGuid());

            var result = await _sut.Create(new Entity());

            Assert.IsType<Guid>(result);
        }

        [Fact]
        public async Task DeleteById_WhenClientCallResultSucessfully_DeleteMethodCalled()
        {
            _client.DeleteAsync(Arg.Any<string>(),Arg.Any<Guid>()).Returns(Task.CompletedTask);

            var guidId = Guid.NewGuid();

             await _sut.DeleteById(guidId,"entityname");

            await _client.Received(1).DeleteAsync("entityname",guidId);
        }

        [Fact]
        public async Task GetById_WhenClientReturnResults_ReturnEntity()
        {
            _client.RetrieveAsync(Arg.Any<string>(), Arg.Any<Guid>(),Arg.Any<ColumnSet>()).Returns(new Entity());

            var guidId = Guid.NewGuid();

            var result = await _sut.GetById(guidId, "entityname");

            await _client.Received(1).RetrieveAsync("entityname", guidId, Arg.Any<ColumnSet>());
            Assert.NotNull(result);
            Assert.IsType<Entity>(result);
        }

        [Fact]
        public async Task ListByFilter_WhenUseFilterExpression_ReturnListOfEntities()
        {
            _client.RetrieveMultipleAsync(Arg.Any<QueryExpression>()).Returns(new EntityCollection());

            var result = await _sut.ListByFilter("entityname", new FilterExpression());

            Assert.NotNull(result);
            Assert.IsType<List<Entity>>(result);
        }

        [Fact]
        public async Task ListByFilter_WhenUseQueryExpression_ReturnListOfEntities()
        {
            _client.RetrieveMultipleAsync(Arg.Any<QueryExpression>()).Returns(new EntityCollection());

            var result = await _sut.ListByFilter("entityname", new QueryExpression());

            Assert.NotNull(result);
            Assert.IsType<List<Entity>>(result);
        }

        [Fact]
        public async Task Update_WhenClientCallResultSucessfully_UpdateAsyncMethodCalled()
        {
            _client.UpdateAsync(Arg.Any<Entity>()).Returns(Task.CompletedTask);

            var updatedEntity = new Entity();

            await _sut.Update(updatedEntity);

            await _client.Received(1).UpdateAsync(updatedEntity);
        }

        [Fact]
        public async Task GetMetadata_WhenEntityExists_ReturnEntityMetadataInstance()
        {
            var response = new RetrieveEntityResponse();
            response.Results["EntityMetadata"] = new EntityMetadata();
            _client.ExecuteAsync(Arg.Any<RetrieveEntityRequest>()).Returns(response);

            var result = await _sut.GetMetadata("entityname");

            Assert.NotNull(result);
            Assert.IsType<EntityMetadata>(result);
        }

        [Fact]
        public async Task SetState_WhenCallResultSucessfully_ExecuteAsyncMethodCalled()
        {
            _client.ExecuteAsync(Arg.Any<SetStateRequest>()).Returns(new OrganizationResponse());

            var entity = new EntityReference();
            
            await _sut.SetState(entity, "1234", "1212");

            await _client.Received(1).ExecuteAsync(Arg.Any<SetStateRequest>());
        }

        [Fact]
        public async Task Upsert_WhenCallResultSucessfully_ExecuteAsyncMethodCalled()
        {
            _client.ExecuteAsync(Arg.Any<UpsertRequest>()).Returns(new OrganizationResponse());

            var entity = new Entity();

            await _sut.Upsert(entity);

            await _client.Received(1).ExecuteAsync(Arg.Any<UpsertRequest>());
        }

        // Marcos
		[Fact]
		public async Task Update_WhenClientCallResultSucessfully_UpdateAsyncMethodCalled()
		{
			_client.UpdateAsync(Arg.Any<Entity>()).Returns(Task.CompletedTask);

			var updatedEntity = new Entity();

            // use optimistic concurrency use 
			await _sut.Update(updatedEntity, true, true);

			await _client.Received(1).UpdateAsync(updatedEntity);
		}

		[Fact]
		public async Task Create_WhenClientCallResultSucessfully_ReturnGuid()
		{
			_client.Create(Arg.Any<Entity>()).Returns(Guid.NewGuid());

			var result = await _sut.Create(new Entity());

			Assert.IsType<Guid>(result);
		}

		[Fact]
		public async Task DeleteById_WhenClientCallResultSucessfully_DeleteMethodCalled()
		{
			_client.DeleteAsync(Arg.Any<string>(), Arg.Any<Guid>()).Returns(Task.CompletedTask);

			var guidId = Guid.NewGuid();

			await _sut.DeleteById(guidId, "entityname");

			await _client.Received(1).DeleteAsync("entityname", guidId);
		}
	}
}
