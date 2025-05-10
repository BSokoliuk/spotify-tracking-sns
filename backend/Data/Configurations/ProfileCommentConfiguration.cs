using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

namespace Data.Configurations;

public class ProfileCommentConfiguration : IEntityTypeConfiguration<ProfileComment>
{
    public void Configure(EntityTypeBuilder<ProfileComment> entityBuilder)
    {
        entityBuilder.HasKey(pc => pc.Id);
        entityBuilder.ToTable("profile_comments");

        entityBuilder.Property(pc => pc.Id).HasColumnName("id");
        entityBuilder.Property(pc => pc.Content).HasColumnName("content");
        entityBuilder.Property(pc => pc.CreatedAt).HasColumnName("created_at");
        entityBuilder.Property(pc => pc.SenderId).HasColumnName("sender_id");
        entityBuilder.Property(pc => pc.RecipientId).HasColumnName("recipient_id");

        // Relationships
        entityBuilder.HasOne(pc => pc.Sender)
            .WithMany(pc => pc.ProfileComments)
            .HasForeignKey(pc => pc.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        entityBuilder.HasOne(pc => pc.Recipient)
            .WithMany()
            .HasForeignKey(pc => pc.RecipientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}