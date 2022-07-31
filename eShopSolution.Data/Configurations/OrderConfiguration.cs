using Microsoft.EntityFrameworkCore;
using eShopSolution.Data.Entities;
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
            builder.Property(x => x.CreateDate); 
            builder.HasOne<Status>(x => x.Status).WithMany(s => s.Orders).HasForeignKey(x => x.StatusId);
            builder.HasOne<Customer>(x => x.Customer).WithMany(c => c.Orders).HasForeignKey(x => x.CustomerId);
        }
    }
}
