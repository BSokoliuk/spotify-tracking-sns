using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

namespace Data.Configurations;

public class ArtistCommentConfiguration : IEntityTypeConfiguration<ArtistComment>
{
    public void Configure(EntityTypeBuilder<ArtistComment> entityBuilder)
    {
        entityBuilder.HasKey(ac => ac.Id);
        entityBuilder.ToTable("artist_comments");

        entityBuilder.Property(ac => ac.Id).HasColumnName("id");
        entityBuilder.Property(ac => ac.Content).HasColumnName("content");
        entityBuilder.Property(ac => ac.CreatedAt).HasColumnName("created_at");
        entityBuilder.Property(ac => ac.SenderId).HasColumnName("sender_id");
        entityBuilder.Property(ac => ac.ArtistId).HasColumnName("artist_id");

        // Relationships
        entityBuilder.HasOne(ac => ac.Sender)
            .WithMany(ac => ac.ArtistComments)
            .HasForeignKey(ac => ac.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        entityBuilder.HasOne(ac => ac.Artist)
            .WithMany(ac => ac.ArtistComments)
            .HasForeignKey(ac => ac.ArtistId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}