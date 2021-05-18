using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Liquid.Services.Configuration
{
    /// <summary>
    /// Service client setting class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LightServiceSetting
    {
        private int _timeout;

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the url address of service.
        /// </summary>
        /// <value>
        /// The url address.
        /// </value>
        [JsonProperty("address")]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the timeout in seconds.
        /// </summary>
        /// <value>
        /// The timeout.
        /// </value>
        [JsonProperty("timeout")]
        public int Timeout
        {
            get => _timeout <= 0 ? 1000 : _timeout;
            set => _timeout = value;
        }

        /// <summary>
        /// Gets or sets the maximum size of the message.
        /// </summary>
        /// <value>
        /// The maximum size of the message.
        /// </value>
        [JsonProperty("messageSize")]
        public long MessageSize { get; set; }

        /// <summary>
        /// Gets or sets custom parameters.
        /// </summary>
        /// <value>
        /// The list of custom parameters.
        /// </value>
        [JsonProperty("parameters")]
        public List<ServiceParameter> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the resilience settings.
        /// </summary>
        /// <value>
        /// The resilience.
        /// </value>
        [JsonProperty("resilience")]
        public ResilienceSetting Resilience { get; set; }
    }

    /// <summary>
    /// Service parameter setting class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ServiceParameter
    {
        /// <summary>
        /// Gets or sets the parameter key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the parameter value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [JsonProperty("value")]
        public object Value { get; set; }
    }

    /// <summary>
    /// Resilience settings class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ResilienceSetting
    {
        /// <summary>
        /// Gets or sets the circuit breaker settings.
        /// </summary>
        /// <value>
        /// The circuit breaker.
        /// </value>
        [JsonProperty("circuitBreaker")]
        public CircuitBreakerSetting CircuitBreaker { get; set; }
        
        /// <summary>
        /// Gets or sets the retry.
        /// </summary>
        /// <value>
        /// The retry.
        /// </value>
        [JsonProperty("retry")]
        public RetrySetting Retry { get; set; }
    }

    /// <summary>
    /// Circuit breaker settings class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CircuitBreakerSetting
    {
        /// <summary>
        /// Gets or sets a value indicating whether the Circuitbreaker mechanism is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the failure threshold at which the circuit breaker will break. It must be a number between 0 and 1 (eg: 0.5 for 50% failures).
        /// </summary>
        /// <value>
        /// The failure threshold.
        /// </value>
        [JsonProperty("failureThreshold")]
        public double FailureThreshold { get; set; }

        /// <summary>
        /// Gets or sets the time slice of duration the failure threshold will be analyzed and compared with <see cref="FailureThreshold"/> property.
        /// </summary>
        /// <value>
        /// The duration of the sampling (in seconds).
        /// </value>
        [JsonProperty("samplingDuration")]
        public int SamplingDuration { get; set; }

        /// <summary>
        /// Gets or sets the minimum throughput of failures necessary to be analyzed.
        /// </summary>
        /// <value>
        /// The minimum throughput.
        /// </value>
        [JsonProperty("minimumThroughput")]
        public int MinimumThroughput { get; set; }

        /// <summary>
        /// Gets or sets the duration the circuit breaker will stay open.
        /// </summary>
        /// <value>
        /// The duration of break (in seconds).
        /// </value>
        [JsonProperty("durationOfBreak")]
        public int DurationOfBreak { get; set; }
    }

    /// <summary>
    /// Retry settings class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RetrySetting
    {
        /// <summary>
        /// Gets or sets a value indicating whether the Retry mechanism is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the attempts.
        /// </summary>
        /// <value>
        /// The attempts.
        /// </value>
        [JsonProperty("attempts")]
        public int Attempts { get; set; }

        /// <summary>
        /// Gets or sets the wait duration between request attempts.
        /// </summary>
        /// <value>
        /// The wait duration between attempts (in milliseconds).
        /// </value>
        [JsonProperty("waitDuration")]
        public int WaitDuration { get; set; }
    }

    /// <summary>
    /// Service client configuration extension class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ServiceConfigurationExtensions
    {
        /// <summary>
        /// Gets the service parameter value.
        /// </summary>
        /// <typeparam name="TObject">The type of the parameter value.</typeparam>
        /// <param name="parameters">The parameter list.</param>
        /// <param name="key">The parameter key.</param>
        /// <returns>the parameter value in TObject type.</returns>
        public static TObject GetServiceParameter<TObject>(this List<ServiceParameter> parameters, string key)
        {
            var parameter = parameters.FirstOrDefault(p => key.Equals(p.Key, StringComparison.InvariantCultureIgnoreCase));
            if (parameter != null)
            {

                var objectValue = parameter.Value;

                switch (typeof(TObject).Name.ToLowerInvariant())
                {
                    case "string":
                    case "int32":
                    case "int64":
                    case "boolean":
                        return (TObject)Convert.ChangeType(objectValue, typeof(TObject));
                    case "timespan":
                        TimeSpan.TryParse((string)objectValue, out _);
                        break;
                }
                throw new ArgumentOutOfRangeException(nameof(key), $"{key} key type error, expected {typeof(TObject)}. Sent {parameter.Value.GetType()}");
            }
            throw new ArgumentOutOfRangeException(nameof(key), $"{key} key Not found in parameters collection.");
        }
    }
}