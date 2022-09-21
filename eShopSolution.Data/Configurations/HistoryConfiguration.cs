using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Action = eShopSolution.Data.Entities.Action;

namespace eShopSolution.Data.Configurations
{
    public class HistoryConfiguration : IEntityTypeConfiguration<History>
    {
        public void Configure(EntityTypeBuilder<History> builder)
        {
            builder.ToTable("Histories");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Time);
            builder.Property(x => x.UserId).IsRequired(false);
            builder.Property(x => x.FormId).IsRequired(false);
            builder.Property(x => x.ActionId).IsRequired(false);
            builder.HasOne<AppUser>(x => x.AppUser).WithMany(y => y.Histories).HasForeignKey(x => x.UserId);
            builder.HasOne<Action>(x => x.Action).WithMany(y => y.Histories).HasForeignKey(x => x.ActionId);
            builder.HasOne<Form>(x => x.Form).WithMany(y => y.Histories).HasForeignKey(x => x.FormId);

        }
    }
}
