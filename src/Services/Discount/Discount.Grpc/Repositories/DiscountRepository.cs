using Dapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Repositories.IRepository;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Threading.Tasks;

namespace Discount.Grpc.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;
        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                ("Select * from Coupon where ProductName=@ProductName", new { ProductName = productName });
            if (coupon == null)
            {
                return new Coupon { ProductName = productName, Amount = 0, Description = "No Discount Description" };
            }
            return coupon;
        }
        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync
                ("Insert into Coupon (ProductName,Description,Amount) values(@ProductName,@Description,@Amount)",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });
            if (affected == 0)
                return false;

            return true;
        }
        public async Task<bool> UpdateDiscount(Coupon coupon)
        { 
        using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var affected = await connection.ExecuteAsync
                ("Update Coupon set ProductName=@ProductName, Description=@Description, Amount=@Amount Where Id=@Id", 
                new { ProductName = coupon.ProductName,Description=coupon.Description,Amount=coupon.Amount,Id=coupon.Id });
            if (affected == 0)
                return false;

              return true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync
                ("Delete from Coupon where ProductName=@ProductName", new { ProductName = productName });
            if (affected == 0)
                return false;

            return true;
        }
    }
}
