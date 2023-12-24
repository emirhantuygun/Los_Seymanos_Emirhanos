using CafeApp.Models;
using Microsoft.EntityFrameworkCore;
using WebProject.Models;

namespace CafeApp.Data
{
    public class DataContext : DbContext
    {

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<OrderProduct> OrderProducts { get; set; }

        public DbSet<Barista> Baristas { get; set; }

        public Barista? FindByEmail(string email)
        {
            return Baristas.SingleOrDefault(b => b.Email == email);
        }

        public async void ClearNullOrders()
        {
            var existingOrderList = await Orders.Where(o => o.TableNo == null).ToListAsync();

            if (existingOrderList != null)
            {

                var orderIds = existingOrderList.Select(op => op.OrderId).ToList();

                var orderProductWiththeOrderIdList = await OrderProducts
                .Where(op => orderIds.Contains(op.OrderId))
                .ToListAsync();

                OrderProducts.RemoveRange(orderProductWiththeOrderIdList);
                Orders.RemoveRange(existingOrderList);
            }

            await SaveChangesAsync();
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    }
}
