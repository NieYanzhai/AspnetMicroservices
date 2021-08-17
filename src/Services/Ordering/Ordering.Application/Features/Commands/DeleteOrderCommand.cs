using MediatR;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Commands
{
    public class DeleteOrderCommand : IRequest
    {
        public int Id { get; set; }
    }
}