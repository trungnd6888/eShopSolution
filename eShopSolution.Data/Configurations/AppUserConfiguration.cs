using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUsers");
            builder.Property(x => x.FullName).IsRequired(true).IsUnicode(true).HasMaxLength(100);
            builder.HasMany<Product>(x => x.Products).WithOne(p => p.AppUser).HasForeignKey(p => p.UserId);
            builder.HasMany<Product>(x => x.Products).WithOne(p => p.AppUser).HasForeignKey(p => p.ApprovedId);
            builder.Property(x => x.AvatarImage).IsRequired(false).IsUnicode(false).HasMaxLength(300);
        }
    }
}