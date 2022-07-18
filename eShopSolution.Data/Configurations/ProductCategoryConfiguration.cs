using Microsoft.EntityFrameworkCore;
using eShopSolution.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.ToTable("ProductCategories");
            builder.HasKey(x => new { x.ProductId, x.CategoryId });
            builder.HasOne<Product>(x => x.Product).WithMany(p => p.ProductCategories).HasForeignKey(x => x.ProductId);
            builder.HasOne<Category>(x => x.Category).WithMany(c => c.ProductCategories).HasForeignKey(x => x.CategoryId);
        }
    }
}
