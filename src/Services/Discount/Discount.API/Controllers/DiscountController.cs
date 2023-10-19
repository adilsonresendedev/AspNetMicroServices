using Discount.API.Entities;
using Discount.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Discount.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _discountepository;

        public DiscountController(IDiscountRepository discountepository)
        {
            _discountepository = discountepository;
        }

        [HttpGet("{productName}", Name = "GetDiscount")]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDiscount(string productName)
        {
            Coupon coupon = await _discountepository.GetDiscount(productName);
            return Ok(coupon);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateDiscount([FromBody] Coupon coupon)
        {
            bool result = await _discountepository.CreateDiscount(coupon);
            Coupon couponOnDatabase = await _discountepository.GetDiscount(coupon.ProductName);
            return CreatedAtRoute("GetDiscount", new { couponOnDatabase.Id, couponOnDatabase.ProductName, couponOnDatabase.Description, couponOnDatabase.Amount }, couponOnDatabase);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateCoupon([FromBody] Coupon coupon)
        {
            bool result = await _discountepository.UpdateDiscount(coupon);
            return Ok(result);
        }

        [HttpDelete("{productName}", Name = "DeleteDiscount")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteCoupon(string productName)
        {
            bool result = await _discountepository.DeleteDiscount(productName);
            return Ok(result);
        }
    }
}
