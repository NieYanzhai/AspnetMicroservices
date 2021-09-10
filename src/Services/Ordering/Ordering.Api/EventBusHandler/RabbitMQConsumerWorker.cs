using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Features.Commands;
using Ordering.Domain.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Ordering.Api.EventBusHandler
{
    public class RabbitMQConsumerWorker : BackgroundService
    {
        private readonly ILogger<RabbitMQConsumerWorker> logger;
        private readonly IRabbitMQClientService rabbitMQClientService;
        private readonly IServiceProvider serviceProvider;

        public RabbitMQConsumerWorker(
            ILogger<RabbitMQConsumerWorker> logger,
            IRabbitMQClientService rabbitMQClientService,
            IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.rabbitMQClientService = rabbitMQClientService;
            this.serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var channel = this.rabbitMQClientService.Connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (sender, eventArgs) =>
            {
                var body = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                this.logger.LogInformation($"Received: {body}");

                var order = JsonSerializer.Deserialize<Order>(body);
                var command = new CheckoutOrderCommand { Order = order };
                
                using (var scope = this.serviceProvider.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    await mediator.Send(command);
                }
            };
            channel.BasicConsume("queue.checkout", true, consumer);
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            this.rabbitMQClientService.Close();
            return Task.CompletedTask;
        }
    }
}