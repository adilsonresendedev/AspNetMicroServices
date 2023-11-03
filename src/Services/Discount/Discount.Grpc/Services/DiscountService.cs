using AutoMapper;
using Discount.API.Repositories.Interfaces;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Grpc.Core;
using System.Diagnostics.CodeAnalysis;

namespace Discount.Grpc.Services
{
    public class DiscountService  : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly ILogger<DiscountService> _logger;
        private readonly IMapper _mapper;
        public DiscountService(IDiscountRepository discountRepository, IMapper mapper, ILogger<DiscountService> logger)
        {
            _discountRepository = discountRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            Coupon coupon = await _discountRepository.GetDiscount(request.ProductName);

            if(coupon is null)
            {
                _logger.LogInformation($"Discount for {request.ProductName} not found.");
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount for {request.ProductName} not found."));
            }

            _logger.LogInformation($"Discount retrieved successfully. Product: {coupon.ProductName}, amount: {coupon.Amount}");
            CouponModel couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<CouponModel> CreatDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            Coupon coupon = _mapper.Map<Coupon>(request.CouponModel);

            await _discountRepository.CreateDiscount(coupon);
            _logger.LogInformation($"Discount successfully created. Product name: {coupon.ProductName}");

            coupon = await _discountRepository.GetDiscount(coupon.ProductName);

            return _mapper.Map<CouponModel>(coupon);
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            Coupon coupon = _mapper.Map<Coupon>(request.CouponModel);
            bool result = await _discountRepository.UpdateDiscount(coupon);
            _logger.LogInformation($"Discount successfully updated  . Product name: {coupon.ProductName}");

            coupon = await _discountRepository.GetDiscount(coupon.ProductName);

            return _mapper.Map<CouponModel>(coupon);
        }

        public override async Task<DeleteCounponResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
           bool result = await _discountRepository.DeleteDiscount(request.ProductName);

            return new DeleteCounponResponse
            {
                Success = result
            };
        }
    }
}