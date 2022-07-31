using Microsoft.EntityFrameworkCore;
using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetails");
            builder.HasKey(x => new { x.OrderId, x.ProductId});
            builder.Property(x => x.Quantity).HasDefaultValue(0);
            builder.HasOne<Order>(x => x.Order).WithMany(o => o.OrderDetails).HasForeignKey(x => x.OrderId);
            builder.HasOne<Product>(x => x.Product).WithMany(o => o.OrderDetails).HasForeignKey(x => x.ProductId);
        }
    }
}
 