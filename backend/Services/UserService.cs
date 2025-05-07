using DTOs;
using DTOs.Users;
using Microsoft.EntityFrameworkCore;
using Models;
using Results;
using Services.Interfaces;
using UoW;

namespace Services;

public class UserService(IUnitOfWork unitOfWork) : IUserService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<User?> GetByUserName(string username)
    {
        var result = await _unitOfWork.Repository<User>().FindAsync(u => u.UserName == username);
        return result.FirstOrDefault();
    }

    public async Task<User?> GetById(string id)
    {
        var user = await _unitOfWork.Repository<User>().GetByIdAsync(id);
        return user;
    }

    public async Task<List<MostActiveUsers>> FetchMostActiveUsers()
    {
        //get top 10 users with most scrobbles
        var users = await _unitOfWork.Repository<User>().FindAllAsync(
            u => u.UserName != null,
            q => q.OrderByDescending(u => u.Scrobbles.Count),
            10,
            u => u.Scrobbles
        );

        return [.. users.Select(u => new MostActiveUsers
        {
            Id = u.Id,
            UserName = u.UserName!,
            ProfilePicture = u.Avatar,
            ScrobbleCount = u.Scrobbles.Count
        })];
    }

    public async Task<CustomResult<CompatibilityResponse>> CalculateCompatibility(string userId, string senderId)
    {
        // fetch users
        var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId);
        var sender = await _unitOfWork.Repository<User>().GetByIdAsync(senderId);

        if (user is null || sender is null)
        {
            return CustomResult<CompatibilityResponse>.Failure(CustomError.RecordNotFound("One or both users not found"));
        }
        // fetch scrobbles
        var userScrobbles = await _unitOfWork.Repository<Scrobble>().FindWithIncludesAsync(
            s => s.Id_User == user.Id,
            s => s.Song.Album.Artist
        );

        var senderScrobbles = await _unitOfWork.Repository<Scrobble>().FindWithIncludesAsync(
            s => s.Id_User == sender.Id,
            s => s.Song.Album.Artist
        );

        // extract distinct artists from scrobbles
        var userArtists = userScrobbles.Select(s => s.Song.Album.Artist).Distinct().ToList();
        var senderArtists = senderScrobbles.Select(s => s.Song.Album.Artist).Distinct().ToList();
        
        // extract common artists from set of both
        var commonArtists = userArtists.Intersect(senderArtists).ToList();

        // calculate compatibility
        var compatibility = (float)commonArtists.Count / (float)Math.Max(userArtists.Count, senderArtists.Count);
        if (float.IsNaN(compatibility))
        {
            compatibility = 0f;
        }

        //get top 3 common artists names
        var topArtists = commonArtists.Take(3).ToList();
        var topArtistsNames = topArtists.Select(a => a.Name).ToList();

        var response = new CompatibilityResponse
        {
            Compatibility = compatibility,
            TopArtists = topArtistsNames
        };

        return CustomResult<CompatibilityResponse>.Success(response);
    }
}