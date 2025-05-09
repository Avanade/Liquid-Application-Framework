﻿using Microsoft.Crm.Sdk.Messages;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using NSubstitute;
using NSubstitute.ReceivedExtensions;

namespace Liquid.Dataverse.Tests
{
    public class LiquidDataverseTests
    {
        private readonly IOrganizationServiceAsync _client;
        private readonly ILiquidDataverse _sut;
        public LiquidDataverseTests()
        {
            var clientFactory = Substitute.For<IDataverseClientFactory>();
            _client = Substitute.For<IOrganizationServiceAsync>();

            clientFactory.GetClient().Returns(_client);

            _sut = new LiquidDataverse(clientFactory);
        }

        [Fact]
        public void Ctor_WhenClientFactoryIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new LiquidDataverse(null));
        }

        [Fact]
        public async Task GetById_WhenClientReturnResults_ReturnEntity()
        {
            _client.RetrieveAsync(Arg.Any<string>(), Arg.Any<Guid>(), Arg.Any<ColumnSet>()).Returns(new Entity());

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

        [Fact]
        public async Task Update_WithOptions_UseRightOrganizationRequestType_RequestParameters()
        {
            _client.UpdateAsync(Arg.Any<Entity>()).Returns(Task.CompletedTask);

            var updatedEntity = new Entity();

            await _sut.Update(updatedEntity, true, true, true);

            await _client.Received(1).ExecuteAsync(Arg.Is<UpdateRequest>(ur =>
            ur.Parameters.ContainsKey("BypassCustomPluginExecution")
            && ur.Parameters.ContainsKey("SuppressCallbackRegistrationExpanderJob")
            && ur.Parameters.ContainsKey("SuppressDuplicateDetection")
            ));
        }

        [Fact]
        public async Task DeleteById_WithOptions_UseRightOrganizationRequestType_RequestParameters()
        {
            _client.DeleteAsync(Arg.Any<string>(), Arg.Any<Guid>()).Returns(Task.CompletedTask);

            var guidId = Guid.NewGuid();

            await _sut.Delete(guidId, "entityname", true, true);

            await _client.Received(1).ExecuteAsync(Arg.Is<DeleteRequest>(dr =>
            dr.Parameters.ContainsKey("BypassCustomPluginExecution")
            ));
        }

        [Fact]
        public async Task Create_WithOptions_UseRightOrganizationRequestType_RequestParameters()
        {
            _client.Execute(Arg.Any<CreateRequest>()).Returns(new CreateResponse());

            var result = await _sut.Create(new Entity(), false, false, true);

            _client.Received(1).Execute(Arg.Is<CreateRequest>(cr =>
            cr.Parameters.ContainsKey("BypassCustomPluginExecution")
            && cr.Parameters.ContainsKey("SuppressCallbackRegistrationExpanderJob")
            && cr.Parameters.ContainsKey("SuppressDuplicateDetection")
            ));

            Assert.IsType<Guid>(result);
        }

    }
}
