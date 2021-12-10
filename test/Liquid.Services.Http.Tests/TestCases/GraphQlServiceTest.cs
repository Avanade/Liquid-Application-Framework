using Liquid.Services.Http.Entities;
using Liquid.Services.Http.Tests.Entities;
using Liquid.Services.Http.Tests.MockServer;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using WireMock.Server;

namespace Liquid.Services.Http.Tests.TestCases
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class GraphQlServiceTest : ServiceBaseTest
    {
        private WireMockServer _server;
        private IServiceProvider _serviceProvider;

        /// <summary>
        /// Establishes the context.
        /// </summary>
        /// <returns></returns>
        [SetUp]
        protected void EstablishContext()
        {
            //_server = new HttpMockServer().EstablishWireMockServer();
            //_serviceProvider = BuildServiceProvider();
            //SubjectUnderTest = _serviceProvider.GetService<ILightHttpService>();
        }

        /// <summary>
        /// Tests the cleanup.
        /// </summary>
        [TearDown]
        protected void TestCleanup()
        {
            //var allRequests = _server.LogEntries;
            //Console.WriteLine(JsonConvert.SerializeObject(allRequests, Formatting.Indented));
            //_server.Stop();
        }

        /// <summary>
        /// Asserts the get.
        /// </summary>
        [Test]
        public async Task Verify_Get()
        {
            //var result = await SubjectUnderTest.GraphQlGetAsync<PersonTestEntity>("/api/graph", "test");
            //Assert.IsNotNull(result);
            //Assert.AreEqual(HttpStatusCode.OK, result.HttpResponse.StatusCode);
            //var obj = await result.GetContentObjectAsync("test");
            //Assert.IsNotNull(obj);
            //Assert.IsInstanceOf<PersonTestEntity>(obj);
            //Assert.AreEqual("Will", obj.FirstName);

            //result = await SubjectUnderTest.GraphQlGetAsync<PersonTestEntity>("/api/graph", "test", new Dictionary<string, string> { { "Authorization", "bearer 1234" } });
            //Assert.IsNotNull(result);
            //Assert.AreEqual(HttpStatusCode.OK, result.HttpResponse.StatusCode);
            //obj = await result.GetContentObjectAsync("test");
            //Assert.IsNotNull(obj);
            //Assert.IsInstanceOf<PersonTestEntity>(obj);
            //Assert.AreEqual("Will", obj.FirstName);

            //result = await SubjectUnderTest.GraphQlGetAsync<PersonTestEntity>("/api/graph", "notFound");
            //Assert.IsNotNull(result);
            //Assert.AreEqual(HttpStatusCode.NotFound, result.HttpResponse.StatusCode);
        }

        /// <summary>
        /// Asserts the get graph ql request.
        /// </summary>
        [Test]
        public async Task Verify_GetGraphQlRequest()
        {
            //var result = await SubjectUnderTest.GraphQlGetAsync<PersonTestEntity>("/api/graph", new GraphQlRequest { Query = "test" });

            //Assert.IsNotNull(result);
            //Assert.AreEqual(HttpStatusCode.OK, result.HttpResponse.StatusCode);
            //var obj = await result.GetContentObjectAsync("test");
            //Assert.IsNotNull(obj);
            //Assert.IsInstanceOf<PersonTestEntity>(obj);
            //Assert.AreEqual("Will", obj.FirstName);

            //result = await SubjectUnderTest.GraphQlGetAsync<PersonTestEntity>("/api/graph", new GraphQlRequest { Query = "test" }, new Dictionary<string, string> { { "Authorization", "bearer 1234" } });
            //Assert.IsNotNull(result);
            //Assert.AreEqual(HttpStatusCode.OK, result.HttpResponse.StatusCode);
            //obj = await result.GetContentObjectAsync("test");
            //Assert.IsNotNull(obj);
            //Assert.IsInstanceOf<PersonTestEntity>(obj);
            //Assert.AreEqual("Will", obj.FirstName);

            //var dataResult = await SubjectUnderTest.GraphQlGetAsync<DataTestEntity>("/api/graph", new GraphQlRequest { Query = "test" }, new Dictionary<string, string> { { "Authorization", "bearer 1234" } });
            //Assert.IsNotNull(dataResult);
            //Assert.AreEqual(HttpStatusCode.OK, result.HttpResponse.StatusCode);
            //var dataObj = await dataResult.GetContentObjectAsync();
            //Assert.IsNotNull(dataObj);
            //Assert.IsInstanceOf<DataTestEntity>(dataObj);
            //Assert.AreEqual("Will", dataObj.Test.FirstName);

            //result = await SubjectUnderTest.GraphQlGetAsync<PersonTestEntity>("/api/graph", new GraphQlRequest { Query = "notFound" });
            //Assert.IsNotNull(result);
            //Assert.AreEqual(HttpStatusCode.NotFound, result.HttpResponse.StatusCode);
        }

        /// <summary>
        /// Asserts the post.
        /// </summary>
        [Test]
        public async Task Verify_Post()
        {
            //var result = await SubjectUnderTest.GraphQlPostAsync<PersonTestEntity>("/api/graph", "test");

            //Assert.IsNotNull(result);
            //Assert.AreEqual(HttpStatusCode.OK, result.HttpResponse.StatusCode);
            //var obj = await result.GetContentObjectAsync("test");
            //Assert.IsNotNull(obj);
            //Assert.IsInstanceOf<PersonTestEntity>(obj);
            //Assert.AreEqual("Will", obj.FirstName);

            //result = await SubjectUnderTest.GraphQlPostAsync<PersonTestEntity>("/api/graph", "test", new Dictionary<string, string> { { "Authorization", "bearer 1234" } });
            //Assert.IsNotNull(result);
            //Assert.AreEqual(HttpStatusCode.OK, result.HttpResponse.StatusCode);
            //obj = await result.GetContentObjectAsync("test");
            //Assert.IsNotNull(obj);
            //Assert.IsInstanceOf<PersonTestEntity>(obj);
            //Assert.AreEqual("Will", obj.FirstName);

            //result = await SubjectUnderTest.GraphQlPostAsync<PersonTestEntity>("/api/graph", "notFound");
            //Assert.IsNotNull(result);
            //Assert.AreEqual(HttpStatusCode.NotFound, result.HttpResponse.StatusCode);
        }

        /// <summary>
        /// Asserts the post.
        /// </summary>
        [Test]
        public async Task Verify_PostGraphQlRequest()
        {
            //var result = await SubjectUnderTest.GraphQlPostAsync<PersonTestEntity>("/api/graph", new GraphQlRequest { Query = "test" });

            //Assert.IsNotNull(result);
            //Assert.AreEqual(HttpStatusCode.OK, result.HttpResponse.StatusCode);
            //var obj = await result.GetContentObjectAsync("test");
            //Assert.IsNotNull(obj);
            //Assert.IsInstanceOf<PersonTestEntity>(obj);
            //Assert.AreEqual("Will", obj.FirstName);

            //result = await SubjectUnderTest.GraphQlPostAsync<PersonTestEntity>("/api/graph", new GraphQlRequest { Query = "test" }, new Dictionary<string, string> { { "Authorization", "bearer 1234" } });
            //Assert.IsNotNull(result);
            //Assert.AreEqual(HttpStatusCode.OK, result.HttpResponse.StatusCode);
            //obj = await result.GetContentObjectAsync("test");
            //Assert.IsNotNull(obj);
            //Assert.IsInstanceOf<PersonTestEntity>(obj);
            //Assert.AreEqual("Will", obj.FirstName);

            //result = await SubjectUnderTest.GraphQlPostAsync<PersonTestEntity>("/api/graph", new GraphQlRequest { Query = "notFound" });
            //Assert.IsNotNull(result);
            //Assert.AreEqual(HttpStatusCode.NotFound, result.HttpResponse.StatusCode);
        }
    }
}