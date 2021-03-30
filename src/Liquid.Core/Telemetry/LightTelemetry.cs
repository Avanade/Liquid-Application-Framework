using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Liquid.Core.Context;
using Liquid.Core.Utils;
using Microsoft.Extensions.Logging;

namespace Liquid.Core.Telemetry
{
    /// <summary>
    /// Liquid Global Telemetry Class.
    /// </summary>
    /// <seealso cref="Liquid.Core.Telemetry.ILightTelemetry" />
    public class LightTelemetry : ILightTelemetry
    {
        private readonly ILogger<LightTelemetry> _logger;
        private readonly ConcurrentStack<string> _stack;
        private readonly ILightContextFactory _contextFactory;
        private readonly ConcurrentDictionary<string, List<TelemetryData>> _telemetryData;
        private readonly ConcurrentDictionary<string, Stopwatch> _telemetryTimer;
        private string _currentPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightTelemetry" /> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public LightTelemetry(ILightContextFactory contextFactory,
                              ILoggerFactory loggerFactory)
        {
            _telemetryData = new ConcurrentDictionary<string, List<TelemetryData>>();
            _telemetryTimer = new ConcurrentDictionary<string, Stopwatch>();
            _telemetryData.TryAdd("Context>", new List<TelemetryData>());
            _telemetryTimer.TryAdd("Context>", new Stopwatch());

            _stack = new ConcurrentStack<string>();
            _contextFactory = contextFactory;
            _logger = loggerFactory.CreateLogger<LightTelemetry>();
            _currentPath = "Context>";
        }

        /// <summary>
        /// Adds the context into telemetry stack.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <exception cref="ArgumentNullException">context</exception>
        public void AddContext(string context)
        {
            if (context.IsNullOrEmpty()) throw new ArgumentNullException(nameof(context));
            _stack.Push(context);
            _currentPath = $"Context>{_stack.Reverse().ToSeparatedString('>')}";
        }

        /// <summary>
        /// Removes the context from the telemetry stack.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <exception cref="ArgumentNullException">context</exception>
        public void RemoveContext(string context)
        {
            if (context.IsNullOrEmpty()) throw new ArgumentNullException(nameof(context));
            if (_stack.TryPeek(out var result) && result.Equals(context, StringComparison.InvariantCultureIgnoreCase))
            {
                _stack.TryPop(out _);
                _currentPath = $"Context>{_stack.Reverse().ToSeparatedString('>')}";
            }
        }

        /// <summary>
        /// Adds data to telemetry.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="telemetryData">The telemetry data.</param>
        public void AddTelemetry(TelemetryType type, object telemetryData)
        {
            var telemetry = new TelemetryData
            {
                TelemetryDateTime = DateTime.Now,
                Type = type,
                Data = telemetryData
            };

            if (_telemetryData.ContainsKey(_currentPath))
            {
                _telemetryData[_currentPath].Add(telemetry);
            }
            else
            {
                _telemetryData.TryAdd(_currentPath, new List<TelemetryData> { telemetry });
            }
        }

        /// <summary>
        /// Adds information data to telemetry.
        /// </summary>
        /// <param name="telemetryData">The telemetry data.</param>
        public void AddInformationTelemetry(object telemetryData)
        {
            AddTelemetry(TelemetryType.Information, telemetryData);
        }

        /// <summary>
        /// Adds warning data to telemetry.
        /// </summary>
        /// <param name="telemetryData">The telemetry data.</param>
        public void AddWarningTelemetry(object telemetryData)
        {
            AddTelemetry(TelemetryType.Warning, telemetryData);
        }

        /// <summary>
        /// Adds error data to telemetry.
        /// </summary>
        /// <param name="telemetryData">The telemetry data.</param>
        public void AddErrorTelemetry(object telemetryData)
        {
            AddTelemetry(TelemetryType.Error, telemetryData);
        }

        /// <summary>
        /// Starts a stop watch to measure the execution time of a process related to the key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <exception cref="ArgumentNullException">key</exception>
        public void StartTelemetryStopWatchMetric(string key)
        {
            if (key.IsNullOrEmpty()) throw new ArgumentNullException(nameof(key));
            var metricPathKey = $"{_currentPath?.TrimEnd('>')}>{key}";

            if (_telemetryTimer.ContainsKey(metricPathKey))
            {
                _telemetryTimer[metricPathKey].Restart();
            }
            else
            {
                _telemetryTimer.TryAdd(metricPathKey, Stopwatch.StartNew());
            }
        }

        /// <summary>
        /// Stops the execution time measurement and collects the data to add to telemetry.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="telemetryData">The telemetry data.</param>
        /// <exception cref="ArgumentNullException">key</exception>
        public void CollectTelemetryStopWatchMetric(string key, object telemetryData = null)
        {
            if (key.IsNullOrEmpty()) throw new ArgumentNullException(nameof(key));
            var metricPathKey = $"{_currentPath?.TrimEnd('>')}>{key}";
            
            if (_telemetryTimer.TryGetValue(metricPathKey, out var timer))
            {
                timer.Stop();
                AddTelemetry(TelemetryType.Metric, new { key, elapsed = timer.Elapsed, data = telemetryData });
            }
        }

        /// <summary>
        /// Flushes all telemetries to log and clear the telemetry data.
        /// </summary>
        public void Flush()
        {
            var context = _contextFactory.GetContext();

            var telemetry = new
            {
                contextId = context.ContextId,
                businessContextId = context.BusinessContextId,
                contextCulture = context.ContextCulture,
                contextChannel = context.ContextChannel,
                date = DateTime.Now,
                telemetryData = _telemetryData
            };

            _logger.LogInformation(telemetry.ToJson());
            _telemetryData.Clear();
            _telemetryTimer.Clear();
            _telemetryData.TryAdd("Context>", new List<TelemetryData>());
            _telemetryTimer.TryAdd("Context>", new Stopwatch());
        }
    }
}