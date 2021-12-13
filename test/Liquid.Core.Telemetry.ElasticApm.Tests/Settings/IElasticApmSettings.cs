namespace Liquid.Core.Telemetry.ElasticApm.Tests.Settings
{
    internal interface IElasticApmSettings
    {
        string ServerUrl { get; set; }

        string SecretToken { get; set; }

        double TransactionSampleRate { get; set; }

        string CloudProvider { get; set; }

        bool? Enabled { get; set; }
    }
}
