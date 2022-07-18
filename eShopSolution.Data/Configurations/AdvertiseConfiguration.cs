using Microsoft.EntityFrameworkCore;
using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
    public class AdvertiseConfiguration : IEntityTypeConfiguration<Advertise>
    {
        public void Configure(EntityTypeBuilder<Advertise> builder)
        {
            builder.ToTable("Advertises");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Title).IsUnicode(true).HasMaxLength(300);
            builder.Property(x => x.ImageUrl).IsUnicode(false).IsRequired(false).HasMaxLength(500);
            builder.Property(x => x.IsApproved).HasDefaultValue(false);
        }
    }
}
