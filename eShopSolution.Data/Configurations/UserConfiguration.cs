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
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Name).IsUnicode(true).HasMaxLength(100);
            builder.Property(x => x.UserName).IsUnicode(false).HasMaxLength(50);
            builder.Property(x => x.Password).IsUnicode(false).HasMaxLength(300);
            builder.Property(x => x.Tel).IsUnicode(false).HasMaxLength(20).IsRequired(false);
            builder.Property(x => x.Email).IsUnicode(false).HasMaxLength(50).IsRequired(false);
            builder.Property(x => x.Address).IsUnicode(true).HasMaxLength(500).IsRequired(false);
            builder.Property(x => x.CreateDate);
            builder.Property(x => x.ImageUrl).IsUnicode(false).HasMaxLength(300).IsRequired(false);
            builder.HasMany<Product>(x => x.Products).WithOne(p => p.User).HasForeignKey(p => p.UserId);
            builder.HasMany<Product>(x => x.Products).WithOne(p => p.User).HasForeignKey(p => p.ApprovedId);
        }
    }
}
