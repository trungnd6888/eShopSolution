using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShopSolution.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            //Data seeding
            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, Name = "Nguyễn Trung", Address = "Hà Nội", Email = "trungk47s5@gmail.com", Tel = "0985052368",  Birthday = DateTime.Parse("1993-09-20") },
                new Customer { Id = 2, Name = "Nguyễn Tuân", Address = "Hà Nội", Email = "tuank47s5@gmail.com", Tel = "098121322", Birthday = DateTime.Parse("1992-06-09") }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Đồng hồ Rolex", Detail = "Automatic", Code = "DH0001", Price = 88000, CreateDate = DateTime.Now, UserId = 1, ApprovedId = 1 },
                new Product { Id = 2, Name = "Đồng hồ Patek Phillip", Detail = "Automatic", Code = "DH0002", Price = 10000, CreateDate = DateTime.Now, UserId = 1, ApprovedId = 1 },
                new Product { Id = 3, Name = "Đồng hồ Hublot", Detail = "Automatic", Code = "DH0003", Price = 2000, CreateDate = DateTime.Now, UserId = 1, ApprovedId = 1 }
            );

            modelBuilder.Entity<AppUser>().HasData(
                new AppUser { Id = 1, FullName = "Nguyễn Phúc Đức", UserName="ducnp", PasswordHash = "123456",  Email = "duc@gmail.com", PhoneNumber = "098765446" }
            );

            modelBuilder.Entity<AppUserRole>().HasData(
                new AppUserRole { UserId = 1, RoleId = 1 }
            );

            modelBuilder.Entity<AppRole>().HasData(
                new AppRole { Id = 1, Name = "admin",  Description = "Quản trị viên" },
                new AppRole { Id = 2, Name = "member", Description = "Thành viên" }
            );
        }
    }
}
