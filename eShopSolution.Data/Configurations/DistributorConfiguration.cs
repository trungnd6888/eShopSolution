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
    public class DistributorConfiguration : IEntityTypeConfiguration<Distributor>
    {
        public void Configure(EntityTypeBuilder<Distributor> builder)
        {
            builder.ToTable("Distributors");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Name).IsUnicode(true).HasMaxLength(300);
            builder.Property(x => x.Address).IsUnicode(true).HasMaxLength(300).IsRequired(false);
            builder.Property(x => x.Tel).IsUnicode(false).HasMaxLength(20).IsRequired(false);
            builder.Property(x => x.Email).IsUnicode(true).HasMaxLength(100).IsRequired(false);
        }
    }
}
