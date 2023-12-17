using CafeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeApp.Data
{
    public class DataContext : DbContext
    {

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<SelectedProduct> SelectedProducts { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options){}

    }

}