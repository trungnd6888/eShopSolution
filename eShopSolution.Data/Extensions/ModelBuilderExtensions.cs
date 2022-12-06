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
            modelBuilder.Entity<Status>().HasData(
                new Status { Id = 1, Name = "Đang chuẩn bị" },
                new Status { Id = 2, Name = "Đang giao" },
                new Status { Id = 3, Name = "Đã giao" }
            );

            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, Name = "Nguyễn Trung", Address = "Hà Nội", Email = "trungk47s5@gmail.com", Tel = "0985052368", Birthday = DateTime.Parse("1993-09-20") },
                new Customer { Id = 2, Name = "Nguyễn Tuân", Address = "Hà Nội", Email = "tuank47s5@gmail.com", Tel = "098121322", Birthday = DateTime.Parse("1992-06-09") }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Đồng hồ Rolex", Detail = "Automatic", Code = "DH0001", Price = 88000, ApprovedId = 1 },
                new Product { Id = 2, Name = "Đồng hồ Patek Phillip", Detail = "Automatic", Code = "DH0002", Price = 10000, ApprovedId = 1 },
                new Product { Id = 3, Name = "Đồng hồ Hublot", Detail = "Automatic", Code = "DH0003", Price = 2000, ApprovedId = 1 }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Đồng hồ nam" }
            );

            modelBuilder.Entity<Brand>().HasData(
              new Brand { Id = 1, Name = "Rolex" },
              new Brand { Id = 2, Name = "Patek Philippe" },
              new Brand { Id = 3, Name = "Omega" },
              new Brand { Id = 4, Name = "Cartier" }
            );

            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory { CategoryId = 1, ProductId = 1 },
                new ProductCategory { CategoryId = 1, ProductId = 2 },
                new ProductCategory { CategoryId = 1, ProductId = 3 }
            );

            modelBuilder.Entity<AppRole>().HasData(
                new AppRole { Id = 1, Name = "admin", NormalizedName = "admin", Description = "Quản trị viên" },
                new AppRole { Id = 2, Name = "member", NormalizedName = "member", Description = "Thành viên" },
                new AppRole { Id = 3, Name = "customer", NormalizedName = "customer", Description = "Khách hàng" }
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
