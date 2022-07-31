using Microsoft.EntityFrameworkCore;
using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Name).IsUnicode(true).HasMaxLength(100);
            builder.Property(x => x.Birthday); 
            builder.Property(x => x.Address).IsUnicode(true).HasMaxLength(500).IsRequired(false);
            builder.Property(x => x.Tel).IsUnicode(false).HasMaxLength(20).IsRequired(false);
            builder.Property(x => x.Email).IsUnicode(false).HasMaxLength(100).IsRequired(false);
        }
    }
}
