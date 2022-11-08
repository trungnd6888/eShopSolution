using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
    public class NewsConfiguration : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            builder.ToTable("News");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Title).IsRequired().IsUnicode(true).HasMaxLength(300);
            builder.Property(x => x.Summary).IsUnicode(true).HasMaxLength(500).IsRequired(false);
            builder.Property(x => x.Content).HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(x => x.ImageUrl).IsUnicode(false).HasMaxLength(500).IsRequired(false);
            builder.Property(x => x.CreateDate);
            builder.Property(x => x.IsApproved).HasDefaultValue(false);
            builder.HasOne<AppUser>(x => x.AppUser).WithMany(u => u.News).HasForeignKey(x => x.UserId);
            builder.HasOne<AppUser>(x => x.AppUser).WithMany(u => u.News).HasForeignKey(x => x.ApprovedId);
        }
    }
}
