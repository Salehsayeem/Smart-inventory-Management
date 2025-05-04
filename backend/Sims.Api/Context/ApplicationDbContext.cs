using Microsoft.EntityFrameworkCore;
using Sims.Api.Helper;
using Sims.Api.Models;
using Sims.Api.Models.Base;

namespace Sims.Api.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Shop> Shops { get; set; } = null!;
        public DbSet<StockMovement> StockMovements { get; set; } = null!;
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; } = null!;
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Location> Locations { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<ForecastData> ForecastData { get; set; } = null!;
        public DbSet<Inventory> Inventories { get; set; } = null!;
        public DbSet<Supplier> Supplier { get; set; } = null!;
        public DbSet<Module> Modules { get; set; } = null!;
        public DbSet<Permission> Permissions { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            CommonHelper.ApplyCommonConfigurations(modelBuilder);
            modelBuilder.Ignore<BaseModel>();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Module>()
                .HasIndex(m => m.Name)
                .IsUnique();

            modelBuilder.Entity<Module>().HasData(
                new Module { Id = 1, Name = "Profile" },
                new Module { Id = 2, Name = "User Management" },
                new Module { Id = 3, Name = "Product" },
                new Module { Id = 4, Name = "Category" },
                new Module { Id = 5, Name = "Inventory Tracking" },
                new Module { Id = 6, Name = "Purchase Orders" },
                new Module { Id = 7, Name = "Suppliers" },
                new Module { Id = 8, Name = "Reports & Logs" }
            );
        }


    }
}
