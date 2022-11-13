using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Code).HasMaxLength(30).IsUnicode(false);
            builder.Property(x => x.CreateDate);
            builder.Property(x => x.Name).IsUnicode(true).HasMaxLength(100).IsRequired(false);
            builder.Property(x => x.Email).HasMaxLength(100).IsUnicode(false).IsRequired(false);
            builder.Property(x => x.Tel).HasMaxLength(50).IsUnicode(false).IsRequired(false);
            builder.Property(x => x.Address).IsUnicode(true).HasMaxLength(500).IsRequired(false);
            builder.HasOne<Status>(x => x.Status).WithMany(s => s.Orders).HasForeignKey(x => x.StatusId);
            builder.HasOne<AppUser>(x => x.AppUser).WithMany(c => c.Orders).HasForeignKey(x => x.UserId);
        }
    }
}
