using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Features.Commands;

namespace Ordering.Application.Features.Handlers
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IOrderRepository orderRepository;
        private readonly ILogger<UpdateOrderHandler> logger;

        public UpdateOrderHandler(IOrderRepository orderRepository, ILogger<UpdateOrderHandler> logger)
        {
            this.orderRepository = orderRepository;
            this.logger = logger;
        }
        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await this.orderRepository.GetByIdAsync(request.Order.Id);
            if (order == null)
            {
                this.logger.LogError($"Order ({request.Order.Id}) can not found.");
                // TODO: throw
            }

            await this.orderRepository.UpdateAsync(order);
            logger.LogInformation($"Order ({order.Id}) Updated.");

            return await Task.FromResult(Unit.Value);
        }
    }
}
