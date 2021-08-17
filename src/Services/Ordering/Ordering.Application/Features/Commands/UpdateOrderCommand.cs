using MediatR;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Commands
{
    public class UpdateOrderCommand : IRequest
    {
        public Order Order { get; set; }
    }
}