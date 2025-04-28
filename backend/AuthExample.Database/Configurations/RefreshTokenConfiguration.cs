using AuthExample.Database.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AuthExample.Database.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
    {
        public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Token).IsRequired();
            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.IssuedAt).IsRequired();
            builder.Property(e => e.ExpiresAt).IsRequired();
            builder.HasIndex(e => e.Token).IsUnique();
            builder.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}