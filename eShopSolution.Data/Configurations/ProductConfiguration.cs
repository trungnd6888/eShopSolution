using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Name).IsUnicode(true).HasMaxLength(300);
            builder.Property(x => x.Code).IsUnicode(false).HasMaxLength(30);
            builder.Property(x => x.Detail).IsUnicode(true).HasMaxLength(1000).IsRequired(false);
            builder.Property(x => x.Price).HasDefaultValue(0);
            builder.Property(x => x.ImageUrl).IsUnicode(false).HasMaxLength(1000).IsRequired(false);
            builder.Property(x => x.CreateDate);
            builder.Property(x => x.IsApproved).HasDefaultValue(false);
            builder.Property(x => x.IsNew).HasDefaultValue(false);
            builder.Property(x => x.IsBestSale).HasDefaultValue(false);
        }
    }
}
