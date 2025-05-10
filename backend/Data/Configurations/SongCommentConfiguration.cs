using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

namespace Data.Configurations;

public class SongCommentConfiguration : IEntityTypeConfiguration<SongComment>
{
    public void Configure(EntityTypeBuilder<SongComment> entityBuilder)
    {
        entityBuilder.HasKey(sc => sc.Id);
        entityBuilder.ToTable("song_comments");

        entityBuilder.Property(sc => sc.Id).HasColumnName("id");
        entityBuilder.Property(sc => sc.Content).HasColumnName("content");
        entityBuilder.Property(sc => sc.CreatedAt).HasColumnName("created_at");
        entityBuilder.Property(sc => sc.SenderId).HasColumnName("sender_id");
        entityBuilder.Property(sc => sc.SongId).HasColumnName("song_id");

        // Relationships
        entityBuilder.HasOne(sc => sc.Sender)
            .WithMany(sc => sc.SongComments)
            .HasForeignKey(sc => sc.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        entityBuilder.HasOne(sc => sc.Song)
            .WithMany(sc => sc.SongComments)
            .HasForeignKey(sc => sc.SongId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}