using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Liquid.Services.Http.Tests.MockServer
{
    /// <summary>
    /// Http Mock Server class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class HttpMockServer
    {
        /// <summary>
        /// Establishes the fluent mock server.
        /// </summary>
        /// <returns></returns>
        public WireMockServer EstablishWireMockServer()
        {
            var server = WireMockServer.Start("http://localhost:1234");
            GenerateMockEndpoints(server);
            return server;
        }

        /// <summary>
        /// Generates the mock endpoints.
        /// </summary>
        /// <param name="server">The server.</param>
        private void GenerateMockEndpoints(WireMockServer server)
        {
            GenerateGetMockEndpoints(server);
            GeneratePostMockEndpoints(server);
            GenerateDeleteMockEndpoints(server);
            GeneratePutMockEndpoints(server);
            GenerateGraphQlGetMockEndpoints(server);
            GenerateGraphQlPostMockEndpoints(server);
        }

        /// <summary>
        /// Generates the get mock endpoints.
        /// </summary>
        /// <param name="server">The server.</param>
        private void GenerateGetMockEndpoints(WireMockServer server)
        {
            server.Given(
                    Request.Create()
                        .WithPath("/api/values")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("content-type", "application/json")
                        .WithBody(@"{ ""firstName"": ""Will"", ""lastName"": ""Doe"" }"));

            server.Given(
                    Request.Create()
                        .WithPath("/api/values")
                        .WithHeader("Authorization", "bearer 1234")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("content-type", "application/json")
                        .WithBody(@"{ ""firstName"": ""Will"", ""lastName"": ""Doe"" }"));

            server.Given(
                    Request.Create()
                        .WithPath("/api/values1")
                        .WithHeader("Authorization", "hmB4F6+ozno=|zUR4HZbhf5LNexj72h9kqAFS+/1VnGoDYyaW2bOcmhCX0dxu6GXfhYLWQJoJwW7awA1oLd0wJnEDu+qgAsGbAzcSR8CG1FIWy4UqnuNRDLQYYrWNDNu2HmLrCvf58bbSUxb4ut0RA74=")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("content-type", "application/json")
                        .WithBody(@"{ ""firstName"": ""Will"", ""lastName"": ""Doe"" }"));


            server.Given(
                    Request.Create()
                        .WithPath("/api/values/xml")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("content-type", "application/xml")
                        .WithBody(@"<?xml version=""1.0"" encoding=""utf-8""?>
<PersonTestEntity xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <FirstName>Will</FirstName>
    <LastName>Doe</LastName>
</PersonTestEntity>"));

            server.Given(
                    Request.Create()
                        .WithPath("/api/values/notfound")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(404));
        }

        /// <summary>
        /// Generates the post mock endpoints.
        /// </summary>
        /// <param name="server">The server.</param>
        private void GeneratePostMockEndpoints(WireMockServer server)
        {
            server.Given(
                    Request.Create()
                        .WithPath("/api/values")
                        .UsingPost()
                        .WithBody(new JsonMatcher(JObject.Parse(@"{
	""firstName"": ""Will"",
    ""lastName"": ""Doe""	
}"))))
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(201)
                        .WithHeader("content-type", "application/json")
                        .WithBody(@"{ ""firstName"": ""Will"", ""lastName"": ""Doe"" }"));

            server.Given(
                    Request.Create()
                        .WithPath("/api/values")
                        .UsingPost()
                        .WithBody("FirstName=Will&LastName=Doe"))
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(201)
                        .WithHeader("content-type", "application/json")
                        .WithBody(@"{ ""firstName"": ""Will"", ""lastName"": ""Doe"" }"));

            server.Given(
                    Request.Create()
                        .WithPath("/api/values")
                        .UsingPost()
                        .WithBody(new XPathMatcher("/PersonTestEntity/FirstName = 'Will'")))
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(201)
                        .WithHeader("content-type", "application/json")
                        .WithBody(@"{ ""firstName"": ""Will"", ""lastName"": ""Doe"" }"));

            server.Given(
                    Request.Create()
                        .WithPath("/api/values/stream")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(201)
                        .WithHeader("content-type", "application/json")
                        .WithBody(@"{ ""firstName"": ""Will"", ""lastName"": ""Doe"" }"));

        }

        /// <summary>
        /// Generates the delete mock endpoints.
        /// </summary>
        /// <param name="server">The server.</param>
        private void GenerateDeleteMockEndpoints(WireMockServer server)
        {
            server.Given(
                    Request.Create()
                        .WithPath("/api/values")
                        .UsingDelete())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(204));

            server.Given(
                    Request.Create()
                        .WithPath("/api/values/del")
                        //.UsingDelete()
                        .WithBody(new JsonMatcher(JObject.Parse(@"{
	""firstName"": ""Will"",
    ""lastName"": ""Doe""	
}"))))
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(204));
        }

        /// <summary>
        /// Generates the put mock endpoints.
        /// </summary>
        /// <param name="server">The server.</param>
        private void GeneratePutMockEndpoints(WireMockServer server)
        {
            server.Given(
                    Request.Create()
                        .WithPath("/api/values")
                        .UsingPut()
                        .WithBody(new JsonMatcher(JObject.Parse(@"{
	""firstName"": ""Will"",
    ""lastName"": ""Doe""	
}"))))
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(201)
                        .WithHeader("content-type", "application/json")
                        .WithBody(@"{ ""firstName"": ""Will"", ""lastName"": ""Doe"" }"));

            server.Given(
                    Request.Create()
                        .WithPath("/api/values")
                        .UsingPut()
                        .WithBody("FirstName=Will&LastName=Doe"))
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(201)
                        .WithHeader("content-type", "application/json")
                        .WithBody(@"{ ""firstName"": ""Will"", ""lastName"": ""Doe"" }"));

            server.Given(
                    Request.Create()
                        .WithPath("/api/values")
                        .UsingPut()
                        .WithBody(new XPathMatcher("/PersonTestEntity/FirstName = 'Will'")))
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(201)
                        .WithHeader("content-type", "application/json")
                        .WithBody(@"{ ""firstName"": ""Will"", ""lastName"": ""Doe"" }"));
        }

        /// <summary>
        /// Generates the graph ql get mock endpoints.
        /// </summary>
        /// <param name="server">The server.</param>
        private void GenerateGraphQlGetMockEndpoints(WireMockServer server)
        {
            server.Given(
                    Request.Create()
                        .WithPath("/api/graph/")
                        .WithParam("query", "test")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("content-type", "application/json")
                        .WithBody(@"{ ""data"":{ ""test"": { ""firstName"": ""Will"", ""lastName"": ""Doe"" }}}"));

            server.Given(
                    Request.Create()
                        .WithPath("/api/graph/")
                        .WithParam("query", "test")
                        .WithHeader("Authorization", "bearer 1234")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("content-type", "application/json")
                        .WithBody(@"{ ""data"":{ ""test"": { ""firstName"": ""Will"", ""lastName"": ""Doe"" }}}"));

            server.Given(
                    Request.Create()
                        .WithPath("/api/graph/")
                        .WithParam("query", "notFound")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(404));
        }

        /// <summary>
        /// Generates the graph ql post mock endpoints.
        /// </summary>
        /// <param name="server">The server.</param>
        private void GenerateGraphQlPostMockEndpoints(WireMockServer server)
        {
            server.Given(
                    Request.Create()
                        .WithPath("/api/graph")
                        .UsingPost()
                        .WithBody(new JsonMatcher(JObject.Parse(@"{
	""query"": ""test""
}"))))
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("content-type", "application/json")
                        .WithBody(@"{ ""data"":{ ""test"": { ""firstName"": ""Will"", ""lastName"": ""Doe"" }}}"));

            server.Given(
                    Request.Create()
                        .WithPath("/api/graph")
                        .WithHeader("Authorization", "bearer 1234")
                        .UsingPost()
                        .WithBody(new JsonMatcher(JObject.Parse(@"{
    ""query"": ""test""
}"))))
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("content-type", "application/json")
                        .WithBody(@"{ ""data"":{ ""test"": { ""firstName"": ""Will"", ""lastName"": ""Doe"" }}}"));

            server.Given(
                    Request.Create()
                        .WithPath("/api/graph")
                        .WithHeader("Authorization", "bearer 1234")
                        .UsingPost()
                        .WithBody(new JsonMatcher(JObject.Parse(@"{
    ""query"": ""notFound""
}"))))
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(404));
        }
    }
}