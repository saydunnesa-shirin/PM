using Microsoft.EntityFrameworkCore;
using PM.Repository.Models;

namespace PM.Repository
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {

        }

        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Store> Stores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductGroup>().ToTable("ProductGroup");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Store>().ToTable("Store");
        }
    }
}
