using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

namespace Helpers;

public class ScrobbleMap
{
    public ScrobbleMap(EntityTypeBuilder<Scrobble> entityBuilder)
    {
        entityBuilder.HasKey(t => t.Id);
        entityBuilder.ToTable("scrobbles");

        entityBuilder.Property(t => t.Id).HasColumnName("id");
        entityBuilder.Property(t => t.Scrobble_Date).HasColumnName("scrobble_date");
        entityBuilder.Property(t => t.Id_User).HasColumnName("id_user");
        entityBuilder.Property(t => t.Id_Song_Internal).HasColumnName("id_song_internal");
    }
}

public class SongMap
{
    public SongMap(EntityTypeBuilder<Song> entityBuilder)
    {
        entityBuilder.HasKey(t => t.Id);
        entityBuilder.ToTable("songs");

        entityBuilder.Property(t => t.Id).HasColumnName("id");
        entityBuilder.Property(t => t.Description).HasColumnName("description");
        entityBuilder.Property(t => t.Title).HasColumnName("title");
        entityBuilder.Property(t => t.Id_Song_Spotify_API).HasColumnName("id_song_spotify_api");
        entityBuilder.Property(t => t.Id_Album_Internal).HasColumnName("id_album_internal");
    }
}

public class FavouriteSongMap
{
    public FavouriteSongMap(EntityTypeBuilder<FavouriteSong> entityBuilder)
    {
        entityBuilder.HasKey(t => t.Id);
        entityBuilder.ToTable("favouriteSongs");

        entityBuilder.Property(t => t.Id).HasColumnName("id");
        entityBuilder.Property(t => t.Id_User).HasColumnName("id_user");
        entityBuilder.Property(t => t.Id_Song_Internal).HasColumnName("id_song_internal");
    }
}

public class SongRatingMap
{
    public SongRatingMap(EntityTypeBuilder<SongRating> entityBuilder)
    {
        entityBuilder.HasKey(t => t.Id);
        entityBuilder.ToTable("songsRating");

        entityBuilder.Property(t => t.Id).HasColumnName("id");
        entityBuilder.Property(t => t.Rating).HasColumnName("rating");
        entityBuilder.Property(t => t.Id_User).HasColumnName("id_user");
        entityBuilder.Property(t => t.Id_Song_Internal).HasColumnName("id_song_internal");
    }
}

public class ArtistMap
{
    public ArtistMap(EntityTypeBuilder<Artist> entityBuilder)
    {
        entityBuilder.HasKey(t => t.Id);
        entityBuilder.ToTable("artists");

        entityBuilder.Property(t => t.Id).HasColumnName("id");
        entityBuilder.Property(t => t.Name).HasColumnName("name");
        entityBuilder.Property(t => t.Id_Artist_Spotify_API).HasColumnName("id_artist_spotify_api");
        entityBuilder.Property(t => t.Photo).HasColumnName("photo");
        entityBuilder.Property(t => t.Description).HasColumnName("description");
    }
}

public class ArtistRatingMap
{
    public ArtistRatingMap(EntityTypeBuilder<ArtistRating> entityBuilder)
    {
        entityBuilder.HasKey(t => t.Id);
        entityBuilder.ToTable("artistsRating");

        entityBuilder.Property(t => t.Id).HasColumnName("id");
        entityBuilder.Property(t => t.Rating).HasColumnName("rating");
        entityBuilder.Property(t => t.Id_User).HasColumnName("id_user");
        entityBuilder.Property(t => t.Id_Artist_Internal).HasColumnName("id_artist_internal");
    }
}

public class AlbumMap
{
    public AlbumMap(EntityTypeBuilder<Album> entityBuilder)
    {
        entityBuilder.HasKey(t => t.Id);
        entityBuilder.ToTable("albums");

        entityBuilder.Property(t => t.Id).HasColumnName("id");
        entityBuilder.Property(t => t.Name).HasColumnName("name");
        entityBuilder.Property(t => t.Id_Album_Spotify_API).HasColumnName("id_album_spotify_api");
        entityBuilder.Property(t => t.Cover).HasColumnName("cover");
        entityBuilder.Property(t => t.Description).HasColumnName("description");
        entityBuilder.Property(t => t.Id_Artist_Internal).HasColumnName("id_artist_internal");
    }
}

public class AlbumRatingMap
{
    public AlbumRatingMap(EntityTypeBuilder<AlbumRating> entityBuilder)
    {
        entityBuilder.HasKey(t => t.Id);
        entityBuilder.ToTable("albumsRating");

        entityBuilder.Property(t => t.Id).HasColumnName("id");
        entityBuilder.Property(t => t.Rating).HasColumnName("rating");
        entityBuilder.Property(t => t.Id_User).HasColumnName("id_user");
        entityBuilder.Property(t => t.Id_Album_Internal).HasColumnName("id_album_internal");
    }
}