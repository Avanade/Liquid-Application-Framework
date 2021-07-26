using System;
using System.Collections.Generic;
using System.Text;
using Liquid.Core.Attributes;

namespace Liquid.Messaging.ServiceBus.Settings
{
    [LiquidSectionName("ServiceBus")]
    public class ServiceBusSettings
    {
        public string ConnectionString { get; set; }
        public string EntityPath { get; set; }
    }
}
