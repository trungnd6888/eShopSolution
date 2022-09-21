using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Action = eShopSolution.Data.Entities.Action;

namespace eShopSolution.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            //Data seeding
            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, Name = "Nguyễn Trung", Address = "Hà Nội", Email = "trungk47s5@gmail.com", Tel = "0985052368", Birthday = DateTime.Parse("1993-09-20") },
                new Customer { Id = 2, Name = "Nguyễn Tuân", Address = "Hà Nội", Email = "tuank47s5@gmail.com", Tel = "098121322", Birthday = DateTime.Parse("1992-06-09") }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Đồng hồ Rolex", Detail = "Automatic", Code = "DH0001", Price = 88000, UserId = 1, ApprovedId = 1 },
                new Product { Id = 2, Name = "Đồng hồ Patek Phillip", Detail = "Automatic", Code = "DH0002", Price = 10000, UserId = 1, ApprovedId = 1 },
                new Product { Id = 3, Name = "Đồng hồ Hublot", Detail = "Automatic", Code = "DH0003", Price = 2000, UserId = 1, ApprovedId = 1 }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Đồng hồ nam" }
            );

            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory { CategoryId = 1, ProductId = 1 },
                new ProductCategory { CategoryId = 1, ProductId = 2 },
                new ProductCategory { CategoryId = 1, ProductId = 3 }
            );

            modelBuilder.Entity<AppUser>().HasData(
                new AppUser { Id = 1, FullName = "Nguyễn Phúc Đức", UserName = "ducnp", PasswordHash = "123456", Email = "duc@gmail.com", PhoneNumber = "098765446" }
            );

            modelBuilder.Entity<AppRole>().HasData(
                new AppRole { Id = 1, Name = "admin", Description = "Quản trị viên" },
                new AppRole { Id = 2, Name = "member", Description = "Thành viên" }
            );

            modelBuilder.Entity<Action>().HasData(
                new Action { Id = 1, Name = "Thêm" },
                new Action { Id = 2, Name = "Sửa" },
                new Action { Id = 3, Name = "Xóa" }
            );

            modelBuilder.Entity<Form>().HasData(
            new Form { Id = 1, Name = "Sản phẩm" },
            new Form { Id = 2, Name = "Tin tức" },
            new Form { Id = 3, Name = "Nhà phân phối" },
            new Form { Id = 4, Name = "Bộ sưu tập" },
            new Form { Id = 5, Name = "Khách hàng" },
            new Form { Id = 6, Name = "Người dùng" },
            new Form { Id = 7, Name = "Vai trò" },
            new Form { Id = 8, Name = "Đơn hàng" }
            );
        }
    }
}
