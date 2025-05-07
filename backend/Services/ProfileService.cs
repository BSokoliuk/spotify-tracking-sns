using DTOs;
using Models;
using Results;
using Services.Interfaces;
using UoW;

namespace Services;

public class ProfileService(IUnitOfWork unitOfWork, IUserService userService) : IProfileService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserService _userService = userService;

    public async Task<CustomResult<ApiResponse>> ChangeBio(string userId, string bio)
    {
        // Retrieve the user
        var user = await _userService.GetById(userId);
        if (user is null)
        {
            return CustomResult<ApiResponse>.Failure(CustomError.RecordNotFound($"User with ID '{userId}' not found."));
        }

        // Check if the bio is already the same
        if (user.Bio != null && user.Bio == bio)
        {
            return CustomResult<ApiResponse>.Failure(CustomError.ValidationError("Bio is already the same."));
        }

        // Update the bio
        user.Bio = bio;
        await _unitOfWork.SaveChangesAsync();

        return CustomResult<ApiResponse>.Success(new ApiResponse(true, "Bio updated successfully"));
    }

    public async Task<CustomResult<ApiResponse>> ChangeAvatar(string userId, string avatar)
    {
        // Retrieve the user
        var user = await _userService.GetById(userId);
        if (user is null)
        {
            return CustomResult<ApiResponse>.Failure(CustomError.RecordNotFound($"User with ID '{userId}' not found."));
        }

        // Clean and update the avatar
        avatar = CleanBase64String(avatar);
        user.Avatar = Convert.FromBase64String(avatar);
        await _unitOfWork.SaveChangesAsync();

        return CustomResult<ApiResponse>.Success(new ApiResponse(true, "Avatar updated successfully"));
    }

    public async Task<CustomResult<ProfileResponse>> GetProfileData(string username)
    {
        var user = await _userService.GetByUserName(username);
        if (user is null)
        {
            return CustomResult<ProfileResponse>.Failure(CustomError.RecordNotFound($"User with username '{username}' not found."));
        }

        var followers = await GetFollowersAsync(user.Id);
        var following = await GetFollowingAsync(user.Id);
        var comments = await GetProfileCommentsAsync(user.Id);
        var lastScrobbles = await GetLastScrobblesAsync(user.Id, 10);
        var scrobblesCount = await GetScrobblesCountAsync(user.Id);
        var ratedSongs = await GetRatedSongsAsync(user.Id);
        var ratedAlbums = await GetRatedAlbumsAsync(user.Id);
        var ratedArtists = await GetRatedArtistsAsync(user.Id);
        var favouriteSongs = await GetFavouriteSongsAsync(user.Id);
        var artistCount = await GetDistinctArtistCountAsync(user.Id);;
        var topArtist = await GetTopArtistAsync(user.Id);

        var userData = new ProfileResponse
        {
            Id = user.Id,
            UserName = user.UserName!,
            ProfilePicture = user.Avatar,
            Description = user.Bio,
            ArtistCount = artistCount,
            Followers = followers,
            Following = following,
            ProfileComments = comments,
            Scrobbles = lastScrobbles,
            ScrobblesCount = scrobblesCount,
            RatedSongs = ratedSongs,
            RatedAlbums = ratedAlbums,
            RatedArtists = ratedArtists,
            FavouriteSongs = favouriteSongs,
            Creation_Date = user.Creation_Date,
            TopArtistImage = topArtist != null ? topArtist.Photo : [],
            RefreshToken = user.RefreshToken
        };
        
        return CustomResult<ProfileResponse>.Success(userData);
    }

    private async Task<List<Follows>> GetFollowersAsync(string userId)
    {
        var followers = await _unitOfWork.Repository<Follow>().FindWithIncludesAsync(
            f => f.Id_Followed == userId,
            f => f.Follower
        );

        return [.. followers.Select(f => new Follows
        {
            Id = f.Id,
            Id_Follower = f.Id_Follower,
            Id_Followed = f.Id_Followed,
            Follower = new FollowerData
            {
                Id = f.Follower.Id,
                UserName = f.Follower.UserName!,
                ProfilePicture = f.Follower.Avatar,
                Bio = f.Follower.Bio
            }
        })];
    }

    private async Task<List<Follows>> GetFollowingAsync(string userId)
    {
        var following = await _unitOfWork.Repository<Follow>().FindWithIncludesAsync(
            f => f.Id_Follower == userId,
            f => f.Followed
        );

        return [.. following.Select(f => new Follows
        {
            Id = f.Id,
            Id_Follower = f.Id_Follower,
            Id_Followed = f.Id_Followed,
            Followed = new FollowerData
            {
                Id = f.Followed.Id,
                UserName = f.Followed.UserName!,
                ProfilePicture = f.Followed.Avatar,
                Bio = f.Followed.Bio
            }
        })];
    }

    private async Task<List<ProfileComments>> GetProfileCommentsAsync(string userId)
    {
        var profileComments = await _unitOfWork.Repository<ProfileComment>().FindWithIncludesAsync(
            pc => pc.Id_Recipient == userId,
            pc => pc.Sender
        );

        return [.. profileComments.Select(pc => new ProfileComments
        {
            Id = pc.Id,
            Comment = pc.Comment,
            Creation_Date = pc.Creation_Date,
            Id_Sender = pc.Id_Sender,
            Sender = new Sender
            {
                Id = pc.Sender.Id,
                UserName = pc.Sender.UserName!,
                ProfilePicture = pc.Sender.Avatar
            },
            Id_Recipient = pc.Id_Recipient,
        })];
    }

    private async Task<List<Scrobbles>> GetLastScrobblesAsync(string userId, int count)
    {
        var scrobbles = await _unitOfWork.Repository<Scrobble>().FindAllAsync(
            s => s.Id_User == userId,
            q => q.OrderByDescending(s => s.Scrobble_Date),
            count,
            s => s.Song.Album.Artist,
            s => s.Song.FavouriteSongs,
            s => s.Song.SongRatings
        );

        return [..scrobbles.Select(s => new Scrobbles
        {
            Id = s.Id,
            Scrobble_Date = s.Scrobble_Date,
            Id_User = s.Id_User,
            Id_Song_Internal = s.Id_Song_Internal,
            Song = s.Song,
            AvgRating = s.Song.SongRatings.Count > 0 ? s.Song.SongRatings.Average(sr => sr.Rating) : 0
        })];
        
    }

    private async Task<int> GetScrobblesCountAsync(string userId)
    {
        return await _unitOfWork.Repository<Scrobble>().CountAsync(s => s.Id_User == userId); 
    }

    private async Task<List<RatedSongs>> GetRatedSongsAsync(string userId)
    {
        var songsRating = await _unitOfWork.Repository<SongRating>().FindAllAsync(
            sr => sr.Id_User == userId,
            q => q.OrderByDescending(sr => sr.Rating),
            null,
            sr => sr.Song
        );

        return [..songsRating.Select(sr => new RatedSongs
        {
            Id_Song = sr.Song.Id,
            Rating = sr.Rating,
            Id_Song_Internal = sr.Id_Song_Internal,
            Song = sr.Song
        })];
    }

    private async Task<List<RatedAlbums>> GetRatedAlbumsAsync(string userId)
    {
        var albumsRating = await _unitOfWork.Repository<AlbumRating>().FindAllAsync(
            ar => ar.Id_User == userId,
            q => q.OrderByDescending(ar => ar.Rating),
            null,
            ar => ar.Album
        );

        return [..albumsRating.Select(ar => new RatedAlbums
        {
            Id_Album = ar.Album.Id,
            Rating = ar.Rating,
            Id_Album_Internal = ar.Id_Album_Internal,
            Album = ar.Album
        })];
    }

    private async Task<List<RatedArtists>> GetRatedArtistsAsync(string userId)
    {
        var artistsRating = await _unitOfWork.Repository<ArtistRating>().FindAllAsync(
            ar => ar.Id_User == userId,
            q => q.OrderByDescending(ar => ar.Rating),
            null,
            ar => ar.Artist
        );

        return [.. artistsRating.Select(ar => new RatedArtists
        {
            Id_Artist = ar.Artist.Id,
            Rating = ar.Rating,
            Id_Artist_Internal = ar.Id_Artist_Internal,
            Artist = ar.Artist
        })];
    }

    private async Task<List<FavouriteSongs>> GetFavouriteSongsAsync(string userId)
    {
        var favoriteSongs = await _unitOfWork.Repository<FavouriteSong>().FindAllAsync(
            fs => fs.Id_User == userId,
            q => q.OrderByDescending(fs => fs.Song.FavouriteSongs.Count),
            null,
            fs => fs.Song.Album.Artist
        );

        return [.. favoriteSongs.Select(fs => new FavouriteSongs
        {
            Id_Song = fs.Song.Id,
            Id_Song_Internal = fs.Id_Song_Internal,
            Song = fs.Song
        })];
    }

    private Task<int> GetDistinctArtistCountAsync(string userId)
    {
        return _unitOfWork.Repository<Scrobble>().CountDistinctAsync(
            s => s.Id_User == userId,
            s => s.Song.Album.Artist
        );
    }

    private async Task<Artist?> GetTopArtistAsync(string userId)
    {
        var topArtist = await _unitOfWork.Repository<Scrobble>().GroupByAsync(
            s => s.Id_User == userId, // Filter by user ID
            s => s.Song.Album.Artist, // Group by Artist
            g => new
            {
                Artist = g.Key,
                Count = g.Count()
            }, // Project the grouped data
            orderBy: q => q.OrderByDescending(g => g.Count()), // Order by count descending
            take: 1, // Take the top result
            s => s.Song.Album.Artist // Include related Artist entity
        );

        return topArtist.FirstOrDefault()?.Artist;
    }

    private static string CleanBase64String(string base64String)
    {
        return base64String[(base64String.IndexOf(',') + 1)..];
    }
}