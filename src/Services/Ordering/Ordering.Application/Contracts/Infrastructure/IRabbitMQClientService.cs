using RabbitMQ.Client;

namespace Ordering.Application.Contracts.Infrastructure
{
    public interface IRabbitMQClientService
    {
        IConnection Connection { get; init; }
        void Close();
        void Publish(string message, string routingKey, string exchangeName);
    }
}