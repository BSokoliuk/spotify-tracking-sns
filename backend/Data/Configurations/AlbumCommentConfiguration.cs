using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

namespace Data.Configurations;

public class AlbumCommentConfiguration : IEntityTypeConfiguration<AlbumComment>
{
    public void Configure(EntityTypeBuilder<AlbumComment> entityBuilder)
    {
        entityBuilder.HasKey(ac => ac.Id);
        entityBuilder.ToTable("album_comments");

        entityBuilder.Property(ac => ac.Id).HasColumnName("id");
        entityBuilder.Property(ac => ac.Content).HasColumnName("content");
        entityBuilder.Property(ac => ac.CreatedAt).HasColumnName("created_at");
        entityBuilder.Property(ac => ac.SenderId).HasColumnName("sender_id");
        entityBuilder.Property(ac => ac.AlbumId).HasColumnName("album_id");

        // Relationships
        entityBuilder.HasOne(ac => ac.Sender)
            .WithMany(u => u.AlbumComments)
            .HasForeignKey(ac => ac.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        entityBuilder.HasOne(ac => ac.Album)
            .WithMany(a => a.AlbumComments)
            .HasForeignKey(ac => ac.AlbumId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}