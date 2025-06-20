using Cafe_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Cafe_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category() { Id = 1, Name = "Food" },
                new Category() { Id = 2, Name = "Drink" }
                );
        }
    }
}
