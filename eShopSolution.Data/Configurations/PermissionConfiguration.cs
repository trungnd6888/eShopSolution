using Microsoft.EntityFrameworkCore;
using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions");
            builder.HasKey(x => new { x.RoleId, x.FunctionId });
            builder.HasOne<Role>(x => x.Role).WithMany(r => r.Permissions).HasForeignKey(x => x.RoleId);
            builder.HasOne<Function>(x => x.Function).WithMany(r => r.Permissions).HasForeignKey(x => x.FunctionId);
            builder.Property(x => x.Add).HasDefaultValue(false);
            builder.Property(x => x.Update).HasDefaultValue(false);
            builder.Property(x => x.Remove).HasDefaultValue(false);
            builder.Property(x => x.View).HasDefaultValue(false);
        }
    }
}
