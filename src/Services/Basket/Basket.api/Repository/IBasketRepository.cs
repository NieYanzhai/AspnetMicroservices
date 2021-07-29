using System.Threading.Tasks;
using Basket.Api.Entities;

namespace Basket.Api.Repository
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> DeleteBasketAsync(string name);
        Task<ShoppingCart> GetBasketAsync(string name);
        Task<ShoppingCart> UpdateBasketAsync(ShoppingCart shoppingCart);
    }
}