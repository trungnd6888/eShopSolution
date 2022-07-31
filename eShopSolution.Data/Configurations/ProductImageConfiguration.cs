using Microsoft.EntityFrameworkCore;
using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImages");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Caption).IsUnicode(true).HasMaxLength(100).IsRequired(false);
            builder.Property(x => x.ImageUrl).IsUnicode(false).HasMaxLength(300).IsRequired(false);
            builder.Property(x => x.SortOrder).HasDefaultValue(0);
            builder.HasOne<Product>(x => x.Product).WithMany(p => p.ProductImages).HasForeignKey(x => x.ProductId);
        }
    }
}
