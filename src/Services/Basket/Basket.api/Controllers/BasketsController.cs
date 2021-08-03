using System.Threading.Tasks;
using Basket.Api.GrpcServices;
using Basket.Api.Entities;
using Basket.Api.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketRepository basketRepository;
        private readonly ICouponsGrpcService couponsGrpcService;

        public BasketsController(IBasketRepository basketRepository, ICouponsGrpcService couponsGrpcService)
        {
            this.basketRepository = basketRepository;
            this.couponsGrpcService = couponsGrpcService;
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

    }
}