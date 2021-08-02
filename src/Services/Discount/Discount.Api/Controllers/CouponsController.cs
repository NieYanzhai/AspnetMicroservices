using System.Threading.Tasks;
using Discount.Api.Entites;
using Discount.Api.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Discount.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CouponsController:ControllerBase
    {
        private readonly ICouponRepository couponRepository;

        public CouponsController(ICouponRepository couponRepository)
        {
            this.couponRepository = couponRepository;
        }

        [HttpGet("{productName}", Name = "GetCoupon")] // api/v1/coupons/{productName}
        [ProducesResponseType(typeof(Coupon), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Coupon>> GetCouponAsync(string productName)
        {
            var coupon = await this.couponRepository.GetCouponAsync(productName);
            if (coupon.Id == 0) return NotFound();
            return Ok(coupon);
        }

        [HttpPost] // api/v1/coupons
        [ProducesResponseType(typeof(Coupon), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Coupon>> PostCouponAsync([FromBody] Coupon coupon)
        {
            var result = await this.couponRepository.CreateCouponAsync(coupon);
            if (!result) return StatusCode(500);
            return await this.GetCouponAsync(coupon.ProductName);
        }

        [HttpPut] // api/v1/coupons
        [ProducesResponseType(typeof(Coupon), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Coupon>> PutCouponAsync([FromBody] Coupon coupon)
        {
            var result = await this.couponRepository.UpdateCouponAsync(coupon);
            if (!result) return StatusCode(500);
            return await this.GetCouponAsync(coupon.ProductName);
        }

        [HttpDelete("{productName}")] // api/v1/coupons/{productName}
        [ProducesResponseType(typeof(Coupon), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> DeleteCouponAsync(string productName)
        {
            var result = await this.couponRepository.DeleteCouponAsync(productName);
            if (!result) return NotFound();
            return true;
        }
    }
}