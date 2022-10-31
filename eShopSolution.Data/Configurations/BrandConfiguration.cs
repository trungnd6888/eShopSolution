using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable("Brands");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Name).IsUnicode(true).HasMaxLength(100);
            builder.Property(x => x.Order).HasDefaultValue(0);
            builder.Property(x => x.ImageUrl).IsUnicode(false).HasMaxLength(500).IsRequired(false);
            builder.HasMany<Product>(x => x.Products).WithOne(p => p.Brand).HasForeignKey(p => p.BrandId);
        }
    }
}
