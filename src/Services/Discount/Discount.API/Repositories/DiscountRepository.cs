using Dapper;
using Discount.API.Entities;
using Discount.API.Repositories.Interfaces;
using Npgsql;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;
        private readonly NpgsqlConnection _npgsqlConnection;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _npgsqlConnection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            int rowsInserted = await _npgsqlConnection
                .ExecuteAsync
                ("Insert into Coupon (ProductName, Description, Amount) Values (@ProductName, @Description, @Amount)",
                new {coupon.ProductName, coupon.Description, coupon.Amount});

            return rowsInserted == 1;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            int rowsUpdated = await _npgsqlConnection
                .ExecuteAsync
                ("Delete from Coupon Where productName = @productName",
                new { productName });

            return rowsUpdated == 1;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            Coupon couponOnDataBase = await _npgsqlConnection
                .QueryFirstOrDefaultAsync<Coupon>
                ("Select * from Coupon Where ProductName = @productName", new { productName });

            if(couponOnDataBase is null)
            {
                couponOnDataBase = new Coupon();
            }

            return couponOnDataBase;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            int rowsUpdated = await _npgsqlConnection
                .ExecuteAsync
                ("Update Coupon set ProductName = @ProductName, Description = @Description, Amount = @Amount Where Id = @Id",
                new { coupon.ProductName, coupon.Description, coupon.Amount, coupon.Id });

            return rowsUpdated == 1;
        }
    }
}
