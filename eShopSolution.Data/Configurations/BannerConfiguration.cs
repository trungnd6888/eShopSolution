using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
    public class BannerConfiguration : IEntityTypeConfiguration<Banner>
    {
        public void Configure(EntityTypeBuilder<Banner> builder)
        {
            builder.ToTable("Banners");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Title).IsUnicode(true).HasMaxLength(300);
            builder.Property(x => x.Summary).IsUnicode(true).HasMaxLength(500).IsRequired(false);
            builder.Property(x => x.ImageUrl).IsUnicode(false).HasMaxLength(300).IsRequired(false);
            builder.Property(x => x.IsApproved).HasDefaultValue(false);
            builder.Property(x => x.Order).HasDefaultValue(0);
        }
    }
}
