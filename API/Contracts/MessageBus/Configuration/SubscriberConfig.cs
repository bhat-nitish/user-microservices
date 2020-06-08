using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.MessageBus.Configuration
{
    public class SubscriberConfig
    {
        public List<RabbitMqConfig> Queues { get; set; }
    }
}
