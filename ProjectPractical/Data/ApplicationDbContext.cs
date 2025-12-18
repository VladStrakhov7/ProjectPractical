using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectPractical.Models;

namespace ProjectPractical.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CarItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Настройка для Product.Price (decimal)
            builder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            // Если есть другие модели с decimal, добавьте их тоже
            // Например, если у CartItem есть Price или Total:
            builder.Entity<CarItem>()
                .Property(c => c.Price) // если есть свойство Price
                .HasColumnType("decimal(18,2)");
        }
    }
}
