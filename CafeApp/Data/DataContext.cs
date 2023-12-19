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

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    }
}
