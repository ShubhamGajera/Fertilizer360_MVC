using Fertilizer360.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;    

namespace Fertilizer360.Data
{
    public class AppDbContext : IdentityDbContext<Users>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Shop> Shops { get; set; } // EF Core uses PascalCase by default
        public DbSet<Fertilizer> Fertilizers { get; set; } // Added Fertilizer DbSet

        public DbSet<Order> Orders { get; set; } // ✅ Add Orders table


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensuring it maps to the correct table
            modelBuilder.Entity<Shop>().ToTable("shops");

            modelBuilder.Entity<Fertilizer>().ToTable("fertilizer");

            modelBuilder.Entity<Order>().ToTable("orders");

        }
    }
}
