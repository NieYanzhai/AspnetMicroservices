using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Features.Commands;
using Ordering.Application.Features.Exceptions;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Handlers
{
    public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IOrderRepository orderRepository;
        private readonly ILogger<UpdateOrderHandler> logger;

        public DeleteOrderHandler(IOrderRepository orderRepository, ILogger<UpdateOrderHandler> logger)
        {
            this.orderRepository = orderRepository;
            this.logger = logger;
        }
        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await this.orderRepository.GetByIdAsync(request.Id);
            if (order == null)
            {
                this.logger.LogError($"Order ({request.Id}) can not found.");
                throw new NotFoundException(nameof(Order), request.Id);
            }

            await this.orderRepository.DeleteAsync(order);
            this.logger.LogInformation($"Order ({order.Id}) Deleted.");

            return await Task.FromResult(Unit.Value);
        }
    }
}
