using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

namespace Data.Configurations;

public class FollowConfiguration : IEntityTypeConfiguration<Follow>
{
    public void Configure(EntityTypeBuilder<Follow> entityBuilder)
    {
        entityBuilder.HasKey(f => f.Id);
        entityBuilder.ToTable("follows");

        entityBuilder.Property(f => f.Id).HasColumnName("id");
        entityBuilder.Property(f => f.FollowerId).HasColumnName("follower_id");
        entityBuilder.Property(f => f.FollowedId).HasColumnName("followed_id");

        // Relationships
        entityBuilder.HasOne(f => f.Follower)
            .WithMany(u => u.Followings) // A user can follow many users
            .HasForeignKey(f => f.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);

        entityBuilder.HasOne(f => f.Followed)
            .WithMany(u => u.Followers) // A user can have many followers
            .HasForeignKey(f => f.FollowedId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}