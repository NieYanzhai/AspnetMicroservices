using System.Threading.Tasks;
using Discount.Api.Entites;

namespace Discount.Api.Repository
{
    public interface ICouponRepository
    {
        Task<bool> CreateCouponAsync(Coupon coupon);
        Task<bool> DeleteCouponAsync(string productName);
        Task<Coupon> GetCouponAsync(string productName);
        Task<bool> UpdateCouponAsync(Coupon coupon);
    }
}