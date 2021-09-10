using System;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Ordering.Infrastructure.RabbitMq
{
    public class RabbitMQClientService : IRabbitMQClientService
    {
        private readonly ILogger<RabbitMQClientService> logger;
        private readonly IConfiguration configuration;

        public IConnection Connection { get; init; }
        private readonly IModel channel;


        public RabbitMQClientService(ILogger<RabbitMQClientService> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;

            // Get RabbitMQSetting
            var rabbitMQSettings = configuration.GetSection("RabbitMQSettings").Get<RabbitMQSettings>();

            // Create RabbitMQ Connection and Channel
            var factory = new ConnectionFactory();
            factory.Uri = new Uri($"amqp://{rabbitMQSettings.Info.User}:{rabbitMQSettings.Info.Password}@{rabbitMQSettings.Info.Host}:{rabbitMQSettings.Info.Port}");
            this.Connection = factory.CreateConnection("Ordering.APi");
            this.channel = this.Connection.CreateModel();

            // Declare RabbitMQ Exchanges
            foreach (var exchange in rabbitMQSettings.Exchanges)
            {
                channel.ExchangeDeclare(exchange.Name, exchange.Type, true, false);
            }

            // Declare RabbitMQ Queues
            foreach (var queue in rabbitMQSettings.Queues)
            {
                channel.QueueDeclare(queue.Name, true, false, false);

                // Bind Queue to Exchange with RoutingKey Applied
                channel.QueueBind(queue.Name, queue.ExchangeToBind, queue.RoutingKey);
            }
        }

        // Publish message to queue
        // Users can make use of Connection to define their own publish methods.
        public void Publish(string message, string routingKey, string exchangeName)
        {
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchangeName, routingKey, null, body);
            logger.LogInformation($"{DateTimeOffset.Now} Message Published {Encoding.UTF8.GetString(body)}");
        }

        public void Close()
        {
            this.channel.Close();
            this.Connection.Close();
            logger.LogInformation($"{DateTimeOffset.Now} connection closed.");
        }
    }
}