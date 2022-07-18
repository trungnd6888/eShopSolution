using Microsoft.EntityFrameworkCore;
using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
    public class FunctionConfiguration : IEntityTypeConfiguration<Function>
    {
        public void Configure(EntityTypeBuilder<Function> builder)
        {
            builder.ToTable("Functions");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Name).IsRequired().IsUnicode(true).HasMaxLength(100);
            builder.Property(x => x.Describe).IsUnicode(true).HasMaxLength(500).IsRequired(false);
            builder.Property(x => x.FormName).IsUnicode(false).HasMaxLength(50).IsRequired(false);
        }
    }
}
