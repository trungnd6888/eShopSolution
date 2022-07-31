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
    public class ProductDistributorConfiguration : IEntityTypeConfiguration<ProductDistributor>
    {
        public void Configure(EntityTypeBuilder<ProductDistributor> builder)
        {
            builder.ToTable("ProductDistributors");
            builder.HasKey(pd => new {pd.ProductId, pd.DistributorId });
            builder.HasOne<Distributor>(x => x.Distributor).WithMany(d => d.ProductDistributors).HasForeignKey(x => x.DistributorId);
            builder.HasOne<Product>(x => x.Product).WithMany(p => p.ProductDistributors).HasForeignKey(x => x.ProductId);
        }
    }
}
