using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Features.Commands;
using Ordering.Application.Models;

namespace Ordering.Application.Features.Handlers
{
    public class CheckoutOrderHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository orderRepository;
        private readonly ILogger<CheckoutOrderHandler> logger;
        private readonly IEmailService emailService;

        public CheckoutOrderHandler(IOrderRepository orderRepository, ILogger<CheckoutOrderHandler> logger, IEmailService emailService)
        {
            this.orderRepository = orderRepository;
            this.logger = logger;
            this.emailService = emailService;
        }
        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await this.orderRepository.AddAsync(request.Order);
            if (order == null)
            {
                this.logger.LogError($"Order checkout failed.");
                // TODO : throw exception
            }

            await this.emailService.SendMail(new Email{
                To="test@domain.com", 
                Subject=$"Order ({order.Id}) checkout success.",
                Body=$"Order ({order.Id}) checkout success."
            });
            this.logger.LogInformation($"Order ({order.Id}) checkout success.");

            return order.Id;
        }
    }
}
