using Liquid.Core.Exceptions;
using Liquid.Messaging.Kafka.Settings;
using Xunit;

namespace Liquid.Messaging.Kafka.Tests
{
    public class KafkaFactoryTest
    {
        private KafkaSettings _settings;
        private readonly IKafkaFactory _sut;

        public KafkaFactoryTest()
        {
            
            _sut = new KafkaFactory();
        }
        [Fact]
        public void GetConsumer_WhenSettingsINull_ThrowLiquidMessagingException()
        {
            Assert.Throws<MessagingMissingConfigurationException>(() => _sut.GetConsumer(_settings));
        }

        [Fact]
        public void GetProducer_WhenSettingsIsNull_ThrowLiquidMessagingException()
        {
            Assert.Throws<MessagingMissingConfigurationException>(() => _sut.GetProducer(_settings));
        }

        [Fact]
        public void GetConsumer_WhenSettingsIsNotValid_ThrowLiquidMessagingException()
        {
            _settings = new KafkaSettings()
            {
                ConnectionId = "test"
            };

            Assert.Throws<MessagingMissingConfigurationException>(() => _sut.GetConsumer(_settings));
        }

        [Fact]
        public void GetProducer_WhenSettingsIsValid_ResposeIsNotNull()
        {
            _settings = new KafkaSettings();

            var producer = _sut.GetProducer(_settings);
            Assert.NotNull(producer);
        }
    }
}
