using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Basket.Api.Services
{
    public interface IRabbitMQClientService
    {
        IConnection Connection { get; init; }
        void Close();
        void Publish(string message, string routingKey, string exchangeName);
    }
}