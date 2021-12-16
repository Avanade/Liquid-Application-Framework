using System.IO;
using System.Text;
using Liquid.Core.Telemetry.ElasticApm.Extensions.DependencyInjection;
using Liquid.Core.Telemetry.ElasticApm.Tests.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Liquid.Core.Telemetry.ElasticApm.Tests.Extensions
{
    internal static class IServiceCollectionExtension
    {
        public static IServiceCollection AddElasticApmByConfiguration(this IServiceCollection services, bool enable)
        {
            var sb = new StringBuilder("{ \"ElasticApm\": {");
            sb.Append(" \"ServerUrl\": \"http://elasticapm:8200\",");
            sb.Append(" \"SecretToken\": \"apm-server-secret-token\",");
            sb.Append(" \"TransactionSampleRate\": 1.0,");
            sb.Append(" \"CloudProvider\": \"none\",");
            sb.Append(" \"Enabled\": ");
            sb.Append(enable.ToString().ToLower());
            sb.Append(" } }");

            using MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(sb.ToString()));

            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            services.Configure<ElasticApmSettings>(config.GetSection(nameof(ElasticApmSettings)));

            services.AddLiquidElasticApmTelemetry(config);

            return services;
        }
    }
}
