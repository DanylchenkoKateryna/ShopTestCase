using Microsoft.EntityFrameworkCore;
using ShopTestCase.Data.Entities;

namespace ShopTestCase.Data
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
   
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OrderProduct>()
                .Property(p => p.TotalPrice)
                .HasColumnType("decimal(18,2)");
        }
    }
}
