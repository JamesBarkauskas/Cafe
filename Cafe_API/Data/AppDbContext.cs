using Cafe_API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cafe_API.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<LocalUser> LocalUsers { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category() { Id = 1, Name = "Food" },
                new Category() { Id = 2, Name = "Drink" }
                );

            modelBuilder.Entity<Product>().HasData(
                new Product() { Id = 1, Name = "Coffee", Calories = 0, Description = "Black coffee", Price = 1.99, CategoryId=2 },
                new Product() { Id=2, Name="Iced Coffee", Calories=100, Description="Iced coffee with vanilla cream", Price=2.99, CategoryId=2}
                );
        }
    }
}
