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
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Name).IsUnicode(true).HasMaxLength(100);
            builder.Property(x => x.Describe).IsUnicode(true).HasMaxLength(300).IsRequired(false);
            builder.HasMany<User>(x => x.Users).WithOne(u => u.Role).HasForeignKey(u => u.RoleId);
        }
    }
}
