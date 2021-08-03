using System.Threading.Tasks;
using AutoMapper;
using Discount.Grpc.Entites;
using Discount.Grpc.Protos;
using Discount.Grpc.Repository;
using Grpc.Core;
using static Discount.Grpc.Protos.Coupons;

namespace Discount.Grpc.Services
{
    public class CouponsService : CouponsBase
    {
        private readonly ICouponRepository couponRepository;
        private readonly IMapper mapper;

        public CouponsService(ICouponRepository couponRepository, IMapper mapper)
        {
            this.couponRepository = couponRepository;
            this.mapper = mapper;
        }

        public override async Task<CouponModel> GetCoupon(GetCouponRequest request, ServerCallContext context)
        {
            var coupon = await this.couponRepository.GetCouponAsync(request.ProductName);
            if (coupon.Id == 0) return null;
            return this.mapper.Map<CouponModel>(coupon);
        }

        public override async Task<CouponModel> CreateCoupon(CreateCouponRequest request, ServerCallContext context)
        {
            var coupon = this.mapper.Map<Coupon>(request.Coupon);
            var result = await this.couponRepository.CreateCouponAsync(coupon);
            if (!result) return null;
            return this.mapper.Map<CouponModel>(coupon);
        }

        public override async Task<CouponModel> UpdateCoupon(UpdateCouponRequest request, ServerCallContext context)
        {
            var coupon = this.mapper.Map<Coupon>(request.Coupon);
            var result = await this.couponRepository.UpdateCouponAsync(coupon);
            if (!result) return null;
            return this.mapper.Map<CouponModel>(coupon);
        }

        public override async Task<DeleteCouponResponse> DeleteCoupon(DeleteCouponRequest request, ServerCallContext context)
        {
            var result = await this.couponRepository.DeleteCouponAsync(request.ProductName);
            if (!result) return new DeleteCouponResponse { Success = false };
            return new DeleteCouponResponse { Success = true };
        }
    }
}