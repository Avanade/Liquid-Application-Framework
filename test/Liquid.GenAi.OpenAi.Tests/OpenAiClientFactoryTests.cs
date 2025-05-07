using Liquid.Core.Entities;
using Liquid.GenAi.OpenAi.Settings;
using Microsoft.Extensions.Options;
using NSubstitute;
using OpenAI.Chat;
using System.Reflection;

namespace Liquid.GenAi.OpenAi.Tests
{
    public class OpenAiClientFactoryTests
    {
        private readonly IOptions<OpenAiOptions> _mockOptions;
        private readonly OpenAiClientFactory _factory;

        public OpenAiClientFactoryTests()
        {
            _mockOptions = Substitute.For<IOptions<OpenAiOptions>>();
            _factory = new OpenAiClientFactory(_mockOptions);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenOptionsIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new OpenAiClientFactory(null));
        }

        [Fact]
        public void GetOpenAIClient_ShouldThrowKeyNotFoundException_WhenNoSettingsForClientId()
        {
            // Arrange
            _mockOptions.Value.Returns(new OpenAiOptions
            {
                Settings = new List<OpenAiSettings>()
            });

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _factory.GetOpenAIClient("invalid-client-id"));
        }

        [Fact]
        public void GetOpenAIClient_ShouldReturnExistingClient_WhenClientAlreadyExists()
        {
            // Arrange
            var clientId = "test-client";
            var chatClient = Substitute.For<ChatClient>();
            var clientDictionary = new ClientDictionary<ChatClient>(clientId, chatClient) { Executions = 0 };

            var settings = new List<OpenAiSettings>
            {
                new OpenAiSettings { ClientId = clientId, Url = "https://example.com", Key = "test-key", DeploymentName = "test-deployment" }
            };

            _mockOptions.Value.Returns(new OpenAiOptions { Settings = settings });

            // Simulate an existing client
            _factory.GetType()
                .GetField("_openAiClients", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(_factory, new List<ClientDictionary<ChatClient>> { clientDictionary });

            // Act
            var result = _factory.GetOpenAIClient(clientId);

            // Assert
            Assert.Equal(chatClient, result);
            Assert.Equal(1, clientDictionary.Executions);
        }

        [Fact]
        public void GetOpenAIClient_ShouldCreateAndReturnNewClient_WhenClientDoesNotExist()
        {
            // Arrange
            var clientId = "new-client";
            var settings = new List<OpenAiSettings>
            {
                new OpenAiSettings
                {
                    ClientId = clientId,
                    Url = "https://example.com",
                    Key = "test-key",
                    DeploymentName = "test-deployment",
                    MaxRetries = 3
                }
            };

            _mockOptions.Value.Returns(new OpenAiOptions { Settings = settings });

            // Act
            var result = _factory.GetOpenAIClient(clientId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateClient_ShouldThrowArgumentNullException_WhenSettingsIsNull()
        {            
            // Act & Assert
            Assert.Throws<TargetInvocationException>(() => _factory.GetType()
                .GetMethod("CreateClient", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(_factory, new object[] { null, "test-client" }));
        }

        [Fact]
        public void CreateClient_ShouldThrowArgumentNullException_WhenSettingsIsEmpty()
        {
            // Act & Assert
            Assert.Throws<TargetInvocationException>(() => _factory.GetType()
                .GetMethod("CreateClient", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(_factory, [new List<OpenAiSettings>(), "test-client"]));

        }

        [Fact]
        public void CreateClient_ShouldCreateClients_WhenValidSettingsProvided()
        {
            // Arrange
            var clientId = "test-client";
            var settings = new List<OpenAiSettings>
            {
                new OpenAiSettings
                {
                    ClientId = clientId,
                    Url = "https://example.com",
                    Key = "test-key",
                    DeploymentName = "test-deployment",
                    MaxRetries = 3
                }
            };

            // Act
            var result = _factory.GetType()
                .GetMethod("CreateClient", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(_factory, new object[] { settings, clientId });

            // Assert
            Assert.NotNull(result);
        }
    }
}
