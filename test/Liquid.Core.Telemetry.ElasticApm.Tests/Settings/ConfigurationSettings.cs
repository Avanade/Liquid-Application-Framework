using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace Liquid.Core.Telemetry.ElasticApm.Tests.Settings
{
    internal class ConfigurationSettings
    {
        internal IConfiguration AddElasticApm(bool? enable = null)
        {
            var elasticApmSettings = new ElasticApmSettings
            {
                ServerUrl = "http://elasticapm:8200",
                SecretToken = "apm-server-secret-token",
                TransactionSampleRate = 1.0,
                CloudProvider = "none"
            };

            if (enable.HasValue)
            {
                elasticApmSettings.Enabled = enable.Value;
            }

            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            var sb = new StringBuilder("{ \"ElasticApm\": ");
            sb.Append(JsonSerializer.Serialize(elasticApmSettings, options));
            sb.Append(" }");

            using MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(sb.ToString()));
            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            return config;
        }
    }
}
