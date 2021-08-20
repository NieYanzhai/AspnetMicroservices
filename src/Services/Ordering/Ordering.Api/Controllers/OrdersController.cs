using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Commands;
using Ordering.Application.Features.Queries;
using Ordering.Application.Models;
using Ordering.Domain.Entities;

namespace Ordering.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrdersController :ControllerBase
    {
        private readonly IMediator mediator;

        public OrdersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{username}", Name = "GetOrdersByUserName")]  // api/v1/orders/{username}
        [ProducesResponseType(typeof(IEnumerable<OrderVm>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OrderVm>>> GetOrdersByUserName(string username)
        {
            var query = new GetOrdersByUsernameQuery { UserName = username };
            return Ok(await this.mediator.Send(query));
        }

        [HttpPost(Name = "CreateOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<int>> CreateOrder([FromBody] Order order)
        {
            var command = new CheckoutOrderCommand { Order = order};
            await this.mediator.Send(command);
            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrder([FromBody] Order order)
        {
            var command = new UpdateOrderCommand {Order = order};
            await this.mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var command = new DeleteOrderCommand { Id = id};
            await this.mediator.Send(command);
            return NoContent();
        }
    }
}