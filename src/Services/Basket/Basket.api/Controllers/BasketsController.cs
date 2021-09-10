using System.Threading.Tasks;
using Basket.Api.GrpcServices;
using Basket.Api.Entities;
using Basket.Api.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using BuildingBlocks.EventBus.Messages.Events;
using Basket.Api.Services;
using System.Text.Json;

namespace Basket.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketRepository basketRepository;
        private readonly ICouponsGrpcService couponsGrpcService;
        private readonly IRabbitMQClientService rabbitMQClientService;

        public BasketsController(
            IBasketRepository basketRepository, 
            ICouponsGrpcService couponsGrpcService,
            IRabbitMQClientService rabbitMQClientService)
        {
            this.basketRepository = basketRepository;
            this.couponsGrpcService = couponsGrpcService;
            this.rabbitMQClientService = rabbitMQClientService;
        }

        [HttpGet("{name}", Name="GetBasket")] // api/v1/baskets/name
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasketAsync(string name)
        {
            return await this.basketRepository.GetBasketAsync(name);
        }

        [HttpPost] // api/v1/baskets
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
        public async Task<ActionResult<ShoppingCart>> PostBasketAsync(ShoppingCart shoppingCart)
        {
            foreach (var item in shoppingCart.Items)
            {
                var coupon = await this.couponsGrpcService.GetCoupon(item.ProductName);
                if (coupon != null)
                {
                    item.Price -= (decimal)coupon.Amount;
                }
            }
            return await this.basketRepository.UpdateBasketAsync(shoppingCart);
        }

        [HttpDelete("name")] // api/v1/baskets/name
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
        public async Task<ActionResult<ShoppingCart>> DeleteBasketAsync(string name)
        {
            return await this.basketRepository.DeleteBasketAsync(name);
        }

        [Route("[action]")] // action must be lower
        [HttpPost] // api/v1/baskets/Checkout
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CheckoutAsync([FromBody] BasketCheckoutEvent basketCheckoutEvent)
        {
            // Get Basket for user - userName
            var basket = (await this.GetBasketAsync(basketCheckoutEvent.UserName)).Value;
            if(basket.Items is null || basket.Items.ToList().Count == 0) return BadRequest();

            basketCheckoutEvent.TotalPrice = basket.TotalPrice;

            // Send to rabbitmq
            var message = JsonSerializer.Serialize(basketCheckoutEvent);
            this.rabbitMQClientService.Publish(message, "key.checkout.basket", "exchange.checkout");

            // Remove basket
            await this.DeleteBasketAsync(basketCheckoutEvent.UserName);
            return Accepted();
        }

    }
}