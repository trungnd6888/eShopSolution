using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
    public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.ToTable("AppRoles");
            builder.Property(x => x.Description).IsUnicode(true).HasMaxLength(300);
            builder.HasMany<AppUserRole>(x => x.AppUserRoles).WithOne(p => p.AppRole).HasForeignKey(p => p.RoleId);
            builder.HasMany<AppRoleClaim>(x => x.AppRoleClaims).WithOne(p => p.AppRole).HasForeignKey(p => p.RoleId);
        }
    }
}
