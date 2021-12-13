namespace Liquid.Core.Telemetry.ElasticApm.Tests.Settings
{
    internal sealed class ElasticApmSettings : IElasticApmSettings
    {
        public string ServerUrl { get; set; }

        public string SecretToken { get; set; }

        public double TransactionSampleRate { get; set; }

        public string CloudProvider { get; set; }

        public bool? Enabled { get; set; } = null;
    }
}
