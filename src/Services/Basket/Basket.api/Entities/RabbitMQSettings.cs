using System.Collections.Generic;

namespace Basket.Api.Entities
{
    public class RabbitMQSettings
    {
        public IEnumerable<Exchange> Exchanges { get; set; }
        public IEnumerable<Queue> Queues { get; set; }
        public Info Info { get; set; }
    }
    
    public class Exchange
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class Queue
    {
        public string Name { get; set; }
        public string ExchangeToBind { get; set; }
        public string RoutingKey { get; set; }
    }

    public class Info
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
    }
    
}