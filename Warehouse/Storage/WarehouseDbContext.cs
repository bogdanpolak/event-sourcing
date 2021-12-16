using Microsoft.EntityFrameworkCore;
using Warehouse.ReadModels;

namespace Warehouse.Storage
{
    public class WarehouseDbContext : DbContext
    {
        public DbSet<ProductFlow> ProductsFlows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductFlow>().HasKey(x => x.Sku);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInMemoryDatabase("Demo");
        }
    }
}