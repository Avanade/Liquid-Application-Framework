using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Liquid.Services.Http.Enum;
using Liquid.Services.Http.Tests.Entities;
using Liquid.Services.Http.Tests.MockServer;
using Liquid.Core.Utils;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using WireMock.Server;

namespace Liquid.Services.Http.Tests.TestCases
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class HttpServiceTest : ServiceBaseTest
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
            _server = new HttpMockServer().EstablishWireMockServer();
            _serviceProvider = BuildServiceProvider();
            SubjectUnderTest = _serviceProvider.GetService<ILightHttpService>();
        }

        /// <summary>
        /// Tests the cleanup.
        /// </summary>
        [TearDown]
        protected void TestCleanup()
        {
            var allRequests = _server.LogEntries;
            Console.WriteLine(JsonConvert.SerializeObject(allRequests, Formatting.Indented));
            _server.Stop();
        }

        /// <summary>
        /// Asserts the get.
        /// </summary>
        [Test]
        public async Task Verify_Get()
        {
            var result = await SubjectUnderTest.GetAsync<PersonTestEntity>("/api/values");
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.HttpResponse.StatusCode);
            var obj = await result.GetContentObjectAsync();
            Assert.IsNotNull(obj);
            Assert.IsInstanceOf<PersonTestEntity>(obj);
            Assert.AreEqual("Will", obj.FirstName);

            result = await SubjectUnderTest.GetAsync<PersonTestEntity>("/api/values", new Dictionary<string, string> { { "Authorization", "bearer 1234" } });
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.HttpResponse.StatusCode);
            obj = await result.GetContentObjectAsync();
            Assert.IsNotNull(obj);
            Assert.IsInstanceOf<PersonTestEntity>(obj);
            Assert.AreEqual("Will", obj.FirstName);

            result = await SubjectUnderTest.GetAsync<PersonTestEntity>("/api/values1", new Dictionary<string, string> { { "Authorization", "hmB4F6+ozno=|zUR4HZbhf5LNexj72h9kqAFS+/1VnGoDYyaW2bOcmhCX0dxu6GXfhYLWQJoJwW7awA1oLd0wJnEDu+qgAsGbAzcSR8CG1FIWy4UqnuNRDLQYYrWNDNu2HmLrCvf58bbSUxb4ut0RA74=" } });
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.HttpResponse.StatusCode);
            obj = await result.GetContentObjectAsync();
            Assert.IsNotNull(obj);
            Assert.IsInstanceOf<PersonTestEntity>(obj);
            Assert.AreEqual("Will", obj.FirstName);

            result = await SubjectUnderTest.GetAsync<PersonTestEntity>("/api/values/xml");
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.HttpResponse.StatusCode);
            obj = await result.GetContentObjectAsync();
            Assert.IsNotNull(obj);
            Assert.IsInstanceOf<PersonTestEntity>(obj);
            Assert.AreEqual("Will", obj.FirstName);


            result = await SubjectUnderTest.GetAsync<PersonTestEntity>("/api/values/notfound");
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.HttpResponse.StatusCode);
        }

        /// <summary>
        /// Asserts the post.
        /// </summary>
        [Test]
        public async Task Verify_Post()
        {
            var person = new PersonTestEntity { FirstName = "Will", LastName = "Doe" };
            var result = await SubjectUnderTest.PostAsync<PersonTestEntity, PersonTestEntity>("/api/values", person);
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.Created, result.HttpResponse.StatusCode);
            var obj = await result.GetContentObjectAsync();
            Assert.IsNotNull(obj);
            Assert.IsInstanceOf<PersonTestEntity>(obj);
            Assert.AreEqual("Will", obj.FirstName);

            result = await SubjectUnderTest.PostAsync<PersonTestEntity, PersonTestEntity>("/api/values",
                                                                               person,
                                                                               new Dictionary<string, string> { { "Authorization", "bearer 1234" } });
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.Created, result.HttpResponse.StatusCode);
            obj = await result.GetContentObjectAsync();
            Assert.IsNotNull(obj);
            Assert.IsInstanceOf<PersonTestEntity>(obj);
            Assert.AreEqual("Will", obj.FirstName);
        }

        /// <summary>
        /// Asserts the post form data.
        /// </summary>
        [Test]
        public async Task Verify_PostFormData()
        {
            var person = new PersonTestEntity { FirstName = "Will", LastName = "Doe" };

            var result = await SubjectUnderTest.PostAsync<PersonTestEntity, PersonTestEntity>("/api/values", person, null, ContentTypeFormat.FormData);
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.Created, result.HttpResponse.StatusCode);
            var obj = await result.GetContentObjectAsync();
            Assert.IsNotNull(obj);
            Assert.IsInstanceOf<PersonTestEntity>(obj);
            Assert.AreEqual("Will", obj.FirstName);
        }

        /// <summary>
        /// Asserts the post XML.
        /// </summary>
        [Test]
        public async Task Verify_PostXml()
        {
            var person = new PersonTestEntity { FirstName = "Will", LastName = "Doe" };

            var result = await SubjectUnderTest.PostAsync<PersonTestEntity, PersonTestEntity>("/api/values", person, null, ContentTypeFormat.Xml);
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.Created, result.HttpResponse.StatusCode);
            var obj = await result.GetContentObjectAsync();
            Assert.IsNotNull(obj);
            Assert.IsInstanceOf<PersonTestEntity>(obj);
            Assert.AreEqual("Will", obj.FirstName);
        }

        /// <summary>
        /// Asserts the put.
        /// </summary>
        [Test]
        public async Task Verify_Put()
        {
            var person = new PersonTestEntity { FirstName = "Will", LastName = "Doe" };
            var result = await SubjectUnderTest.PutAsync<PersonTestEntity, PersonTestEntity>("/api/values", person);
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.Created, result.HttpResponse.StatusCode);
            var obj = await result.GetContentObjectAsync();
            Assert.IsNotNull(obj);
            Assert.IsInstanceOf<PersonTestEntity>(obj);
            Assert.AreEqual("Will", obj.FirstName);

            result = await SubjectUnderTest.PutAsync<PersonTestEntity, PersonTestEntity>("/api/values",
                                                                               person,
                                                                               new Dictionary<string, string> { { "Authorization", "bearer 1234" } });
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.Created, result.HttpResponse.StatusCode);
            obj = await result.GetContentObjectAsync();
            Assert.IsNotNull(obj);
            Assert.IsInstanceOf<PersonTestEntity>(obj);
            Assert.AreEqual("Will", obj.FirstName);

            result = await SubjectUnderTest.PutAsync<PersonTestEntity, PersonTestEntity>("/api/values", person, null, ContentTypeFormat.FormData);
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.Created, result.HttpResponse.StatusCode);
            obj = await result.GetContentObjectAsync();
            Assert.IsNotNull(obj);
            Assert.IsInstanceOf<PersonTestEntity>(obj);
            Assert.AreEqual("Will", obj.FirstName);

            result = await SubjectUnderTest.PutAsync<PersonTestEntity, PersonTestEntity>("/api/values", person, null, ContentTypeFormat.Xml);
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.Created, result.HttpResponse.StatusCode);
            obj = await result.GetContentObjectAsync();
            Assert.IsNotNull(obj);
            Assert.IsInstanceOf<PersonTestEntity>(obj);
            Assert.AreEqual("Will", obj.FirstName);
        }

        /// <summary>
        /// Asserts the delete.
        /// </summary>
        [Test]
        public async Task Verify_Delete()
        {
            var result = await SubjectUnderTest.DeleteAsync<PersonTestEntity>("/api/values");
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NoContent, result.HttpResponse.StatusCode);
            var obj = await result.GetContentObjectAsync();
            Assert.IsNull(obj);

            result = await SubjectUnderTest.DeleteAsync<PersonTestEntity>("/api/values", new Dictionary<string, string> { { "Authorization", "bearer 1234" } });
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NoContent, result.HttpResponse.StatusCode);
            obj = await result.GetContentObjectAsync();
            Assert.IsNull(obj);

            //TODO: Comentado, pois o servidor mock não possui recepcao de metodo delete com body request.
            //var person = new PersonTestEntity { FirstName = "Will", LastName = "Doe" };
            //result = await SubjectUnderTest.DeleteAsync<PersonTestEntity, PersonTestEntity>("/api/values/del", person);
            //Assert.IsNotNull(result);
            //Assert.AreEqual(HttpStatusCode.NoContent, result.HttpResponse.StatusCode);
            //obj = await result.GetContentObjectAsync();
            //Assert.IsNull(obj);
        }

        /// <summary>
        /// Asserts the send request.
        /// </summary>
        [Test]
        public async Task Verify_SendRequest()
        {
            var result = await SubjectUnderTest.SendRequestAsync<PersonTestEntity, PersonTestEntity>("/api/values", HttpMethod.Get);
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.HttpResponse.StatusCode);
            var obj = await result.GetContentObjectAsync();
            Assert.IsNotNull(obj);
            Assert.IsInstanceOf<PersonTestEntity>(obj);
            Assert.AreEqual("Will", obj.FirstName);

            result = await SubjectUnderTest.SendRequestAsync<PersonTestEntity, PersonTestEntity>("/api/values",
                                                                                      HttpMethod.Get,
                                                                                      null,
                                                                                      new Dictionary<string, string> { { "Authorization", "bearer 1234" } });
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.HttpResponse.StatusCode);
            obj = await result.GetContentObjectAsync();
            Assert.IsNotNull(obj);
            Assert.IsInstanceOf<PersonTestEntity>(obj);
            Assert.AreEqual("Will", obj.FirstName);

            var person = new PersonTestEntity { FirstName = "Will", LastName = "Doe" };
            result = await SubjectUnderTest.SendRequestAsync<PersonTestEntity, PersonTestEntity>("/api/values",
                                                                                      HttpMethod.Post,
                                                                                      person,
                                                                                      new Dictionary<string, string> { { "Authorization", "bearer 1234" } });
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.Created, result.HttpResponse.StatusCode);
            obj = await result.GetContentObjectAsync();
            Assert.IsNotNull(obj);
            Assert.IsInstanceOf<PersonTestEntity>(obj);
            Assert.AreEqual("Will", obj.FirstName);

            result = await SubjectUnderTest.SendRequestAsync<PersonTestEntity, PersonTestEntity>("/api/values/notfound",
                                                                                      HttpMethod.Get);
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.HttpResponse.StatusCode);
        }

        /// <summary>
        /// Asserts the send stream request.
        /// </summary>
        [Test]
        public async Task Verify_SendStreamRequest()
        {
            var result = await SubjectUnderTest.SendStreamRequestAsync<PersonTestEntity>("/api/values/stream",
                                                                              HttpMethod.Post,
                                                                              "teste".ToStreamUtf8(),
                                                                              new Dictionary<string, string> { { "Authorization", "bearer 1234" } });
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.Created, result.HttpResponse.StatusCode);
            var obj = await result.GetContentObjectAsync();
            Assert.IsNotNull(obj);
            Assert.IsInstanceOf<PersonTestEntity>(obj);
            Assert.AreEqual("Will", obj.FirstName);
        }
    }
}