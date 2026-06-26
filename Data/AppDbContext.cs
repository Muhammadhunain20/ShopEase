using Microsoft.EntityFrameworkCore;
using ShopEase.Models;

namespace ShopEase.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);
        }
    }

    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Categories.Any()) return;

            var categories = new List<Category>
            {
                new Category { Name = "Electronics" },
                new Category { Name = "Clothing" },
                new Category { Name = "Home & Garden" },
                new Category { Name = "Sports" }
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();

            var products = new List<Product>
            {
                new Product { Name = "Wireless Headphones", Description = "Premium noise-cancelling wireless headphones with 30hr battery life.", Price = 8999, DiscountPrice = 6999, Stock = 50, CategoryId = categories[0].Id, ImageUrl = "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=400" },
                new Product { Name = "Smart Watch", Description = "Feature-packed smartwatch with health monitoring and GPS.", Price = 15999, DiscountPrice = 12499, Stock = 30, CategoryId = categories[0].Id, ImageUrl = "https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=400" },
                new Product { Name = "Laptop Stand", Description = "Ergonomic aluminum laptop stand, adjustable height.", Price = 2499, Stock = 100, CategoryId = categories[0].Id, ImageUrl = "https://images.unsplash.com/photo-1527864550417-7fd91fc51a46?w=400" },
                new Product { Name = "Men's Casual T-Shirt", Description = "100% cotton premium casual t-shirt, available in multiple colors.", Price = 999, DiscountPrice = 699, Stock = 200, CategoryId = categories[1].Id, ImageUrl = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=400" },
                new Product { Name = "Women's Sneakers", Description = "Lightweight and comfortable running sneakers for everyday wear.", Price = 4599, DiscountPrice = 3299, Stock = 80, CategoryId = categories[1].Id, ImageUrl = "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=400" },
                new Product { Name = "Denim Jacket", Description = "Classic denim jacket with modern slim fit design.", Price = 3999, Stock = 60, CategoryId = categories[1].Id, ImageUrl = "https://images.unsplash.com/photo-1601333144130-8cbb312386b6?w=400" },
                new Product { Name = "Coffee Maker", Description = "12-cup programmable coffee maker with built-in grinder.", Price = 6499, DiscountPrice = 4999, Stock = 40, CategoryId = categories[2].Id, ImageUrl = "https://images.unsplash.com/photo-1495474472287-4d71bcdd2085?w=400" },
                new Product { Name = "Throw Pillow Set", Description = "Set of 4 decorative throw pillows with removable covers.", Price = 1899, Stock = 150, CategoryId = categories[2].Id, ImageUrl = "https://images.unsplash.com/photo-1586023492125-27b2c045efd7?w=400" },
                new Product { Name = "Yoga Mat", Description = "Extra thick non-slip yoga mat with carrying strap.", Price = 2299, DiscountPrice = 1799, Stock = 120, CategoryId = categories[3].Id, ImageUrl = "https://images.unsplash.com/photo-1601925228965-4c0c0fd20e70?w=400" },
                new Product { Name = "Water Bottle", Description = "Insulated stainless steel water bottle, keeps cold 24hrs.", Price = 1499, Stock = 200, CategoryId = categories[3].Id, ImageUrl = "https://images.unsplash.com/photo-1602143407151-7111542de6e8?w=400" },
                new Product { Name = "Resistance Bands Set", Description = "Set of 5 resistance bands for full body workout.", Price = 1299, DiscountPrice = 999, Stock = 180, CategoryId = categories[3].Id, ImageUrl = "https://images.unsplash.com/photo-1598289431512-b97b0917affc?w=400" },
                new Product { Name = "Bluetooth Speaker", Description = "Portable waterproof Bluetooth speaker with 360° sound.", Price = 5499, DiscountPrice = 3999, Stock = 70, CategoryId = categories[0].Id, ImageUrl = "https://images.unsplash.com/photo-1608043152269-423dbba4e7e1?w=400" },
            };
            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}
