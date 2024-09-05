using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopAppAPI.Entities;
using System.Reflection;

namespace ShopAppAPI.Data
{
    public class ShopAppContext:IdentityDbContext<AppUser>
    {
        public ShopAppContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new ProductEntityConfiguration());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
