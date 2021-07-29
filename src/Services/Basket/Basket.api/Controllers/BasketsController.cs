using System.Threading.Tasks;
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

        public BasketsController(IBasketRepository basketRepository)
        {
            this.basketRepository = basketRepository;
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