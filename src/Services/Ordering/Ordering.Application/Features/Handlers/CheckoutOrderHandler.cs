using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentEmail.Core.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Features.Commands;
using Ordering.Application.Models.Email;

namespace Ordering.Application.Features.Handlers
{
    public class CheckoutOrderHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository orderRepository;
        private readonly ILogger<CheckoutOrderHandler> logger;
        private readonly IEmailService emailService;
        private readonly string mailTemplate;

        public CheckoutOrderHandler(IOrderRepository orderRepository, ILogger<CheckoutOrderHandler> logger, IEmailService emailService)
        {
            this.orderRepository = orderRepository;
            this.logger = logger;
            this.emailService = emailService;

            this.mailTemplate = File.ReadAllText("../../Models/Mail/MailTemplates/DefaultMailTemplate.txt");
        }
        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await this.orderRepository.AddAsync(request.Order);

            await this.emailService.SendMail(new Email{
                ToMailAddresses = new List<Address>{
                    new Address("yanzhai.nie@csisolar.com"),
                    new Address("f130ff@163.com")
                },
                Subject = $"Checkout Order",
                Template = this.mailTemplate
            });
            this.logger.LogInformation($"Order ({order.Id}) checkout success.");

            return order.Id;
        }
    }
}
