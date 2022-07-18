using Microsoft.EntityFrameworkCore;
using eShopSolution.Data.Entities;
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
            builder.Property(x => x.Content).IsUnicode(true).HasMaxLength(1000).IsRequired(false);
            builder.Property(x => x.ImageUrl).IsUnicode(false).HasMaxLength(500).IsRequired(false);
            builder.Property(x => x.CreateDate);
            builder.Property(x => x.IsApproved).HasDefaultValue(false);
            builder.HasOne<User>(x => x.User).WithMany(u => u.News).HasForeignKey(x => x.UserId);
            builder.HasOne<User>(x => x.User).WithMany(u => u.News).HasForeignKey(x => x.ApprovedId);
        }
    }
}
