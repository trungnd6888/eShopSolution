using Microsoft.EntityFrameworkCore;
using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
    public class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.ToTable("Menus");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Name).IsUnicode(true).HasMaxLength(100);
            builder.Property(x => x.Url).IsUnicode(false).HasMaxLength(300).IsRequired(false);
            builder.Property(x => x.Order).HasDefaultValue(0);
            builder.Property(x => x.IsApproved).HasDefaultValue(false);
            builder.Property(x => x.Target).IsUnicode(false).HasMaxLength(100).IsRequired(false);
        }
    }
}
