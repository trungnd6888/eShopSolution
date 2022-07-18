using Microsoft.EntityFrameworkCore;
using eShopSolution.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Name).IsUnicode(true).HasMaxLength(100);
            builder.Property(x => x.Order).HasDefaultValue(0);
            builder.Property(x => x.ImageUrl).IsUnicode(false).HasMaxLength(500).IsRequired(false);
        }
    }
}
