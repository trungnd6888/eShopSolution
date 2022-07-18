using eShopSolution.Data.Configurations;
using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using eShopSolution.Data.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace eShopSolution.Data.EF
{
    public class EShopDbContext : IdentityDbContext
    {
        public EShopDbContext(DbContextOptions options) : base(options) 
        {
         
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configuration using Fluent API
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductDistributorConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new FunctionConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
            modelBuilder.ApplyConfiguration(new StatusConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new AdvertiseConfiguration());
            modelBuilder.ApplyConfiguration(new BannerConfiguration());
            modelBuilder.ApplyConfiguration(new MenuConfiguration());
            modelBuilder.ApplyConfiguration(new NewsConfiguration());
            modelBuilder.ApplyConfiguration(new DistributorConfiguration());

            //Seed Data
            modelBuilder.Seed();

            //base.OnModelCreating(modelBuilder);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Distributor> Distributors { get; set; }
        public DbSet<Advertise> Advertises { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<ProductDistributor> ProductDistributors { get; set; }
    }
}
