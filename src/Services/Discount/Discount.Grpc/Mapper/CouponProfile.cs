using AutoMapper;
using Discount.Grpc.Entites;
using Discount.Grpc.Protos;

namespace Discount.Grpc.Mapper
{
    public class CouponProfile:Profile
    {
        public CouponProfile()
        {  
            CreateMap<Coupon, CouponModel>().ReverseMap();            
        }
    }
}