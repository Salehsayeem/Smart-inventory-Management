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
        public DbSet<UserShop> UserShops { get; set; } = null!;
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
            modelBuilder.Entity<UserShop>()
                .HasIndex(us => new { us.UserId, us.ShopId })
                .IsUnique();

            modelBuilder.Entity<Module>().HasData(
                new Module { Id = 1, Name = "Dashboard", ModuleIcon = "bx bxs-dashboard",Path = "dashboard" },
                new Module { Id = 2, Name = "Profile", ModuleIcon = "bx bx-user",Path = "profile" },
                new Module { Id = 3, Name = "User Management", ModuleIcon = "bx bx-group" ,Path = "user-management" },
                new Module { Id = 4, Name = "Product", ModuleIcon = "bx bx-box", Path = "product" },
                new Module { Id = 5, Name = "Category", ModuleIcon = "bx bx-category", Path = "category" },
                new Module { Id = 6, Name = "Inventory Tracking", ModuleIcon = "bx bx-archive" , Path = "inventory-tracking" },
                new Module { Id = 7, Name = "Purchase Orders", ModuleIcon = "bx bx-purchase-tag" , Path = "purchase-orders" },
                new Module { Id = 8, Name = "Suppliers", ModuleIcon = "bx bx-store" , Path = "suppliers" },
                new Module { Id = 9, Name = "Reports & Logs", ModuleIcon = "bx bx-bar-chart" , Path = "reports-logs" }
            );
        }

    }
}
