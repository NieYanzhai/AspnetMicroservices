using System.Text.Json;
using System.Threading.Tasks;
using Basket.Api.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.Api.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache distributedCache;

        public BasketRepository(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        public async Task<ShoppingCart> GetBasketAsync(string name)
        {
            var basketCache = await this.distributedCache.GetStringAsync(name);
            if (string.IsNullOrEmpty(basketCache))
            {
                var newShoppingCart = new ShoppingCart(name);
                return await this.UpdateBasketAsync(newShoppingCart);
            }
            return JsonSerializer.Deserialize<ShoppingCart>(basketCache);
        }

        public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart shoppingCart)
        {
            await this.distributedCache.SetStringAsync(shoppingCart.Name, JsonSerializer.Serialize(shoppingCart));
            return await this.GetBasketAsync(shoppingCart.Name);
        }

        public async Task<ShoppingCart> DeleteBasketAsync(string name)
        {
            var basketCache = await this.GetBasketAsync(name);
            await this.distributedCache.RemoveAsync(name);
            return basketCache;
        }
    }
}