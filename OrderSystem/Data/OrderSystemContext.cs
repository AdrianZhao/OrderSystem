using Microsoft.EntityFrameworkCore;
using OrderSystem.Models;
using System.Data;

namespace OrderSystem.Data
{
    public class OrderSystemContext : DbContext
    {
        public OrderSystemContext(DbContextOptions<OrderSystemContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18, 2)");
            modelBuilder.Entity<Order>().Property(o => o.TotalPrice).HasColumnType("decimal(18, 2)");
            modelBuilder.Entity<Country>().Property(c => c.ConversionRate).HasColumnType("decimal(18, 2)");
            var products = new List<Product>
            {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Cat toy",
                    Description = "XiaoGou's favorite",
                    AvailableQuantity = 1,
                    Price = 10.99m
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Cat licking toy",
                    Description = "NuoMi's favorite",
                    AvailableQuantity = 5,
                    Price = 19.99m
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Cat tree",
                    Description = "ZhaZha's favorite",
                    AvailableQuantity = 2,
                    Price = 9.99m
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Cat food",
                    Description = "Best cat food",
                    AvailableQuantity = 0,
                    Price = 4.99m
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Cat scratch post",
                    Description = "Best Cat scratch post",
                    AvailableQuantity = 3,
                    Price = 12.99m
                }
            };
            modelBuilder.Entity<Product>().HasData(products);
            var countries = new List<Country>
            {
                new Country
                {
                    Id = Guid.NewGuid(),
                    Name = "Canada",
                    ConversionRate = 1.0m
                },
                new Country
                {
                    Id = Guid.NewGuid(),
                    Name = "USA",
                    ConversionRate = 0.73m
                },
                new Country
                {
                    Id = Guid.NewGuid(),
                    Name = "Mexico",
                    ConversionRate = 13.29m
                },
            };
            modelBuilder.Entity<Country>().HasData(countries);
        }
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<Country> Countries { get; set; } = default!;
        public DbSet<Order> Orders { get; set; } = default!;
        public DbSet<InCartProduct> InCartProducts { get; set; } = default!;
        public DbSet<InOrderProduct> InOrderProducts { get; set; } = default!;
        public DbSet<Cart> Carts { get; set; } = default!;

    }
}