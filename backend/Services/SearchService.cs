using DTOs.Profile;
using DTOs.Search;
using Models;
using Results;
using Services.Interfaces;
using UoW;

namespace Services;

public class SearchService(IUnitOfWork unitOfWork) : ISearchService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CustomResult<SearchResponse>> Search(string query)
    {
        // Perform case-insensitive search for users, songs, albums, and artists
        var users = await _unitOfWork.Repository<User>().FindAllAsync(
            u => u.UserName != null && u.UserName.ToLower().Contains(query.ToLower())
        );

        var songs = await _unitOfWork.Repository<Song>().FindWithIncludesAsync(
            s => s.Title != null && s.Title.ToLower().Contains(query.ToLower()),
            s => s.Album // include album to get the cover from the album
        );

        var albums = await _unitOfWork.Repository<Album>().FindAllAsync(
            a => a.Name != null && a.Name.ToLower().Contains(query.ToLower())
        );

        var artists = await _unitOfWork.Repository<Artist>().FindAllAsync(
            a => a.Name != null && a.Name.ToLower().Contains(query.ToLower())
        );

        return CustomResult<SearchResponse>.Success(new SearchResponse
        {
            Users = users.Select(u => new CompactUserProfileDTO
            {
                Id = u.Id,
                UserName = u.UserName!,
                ProfilePicture = u.Avatar
            }).ToList(),
            Songs = [..songs],
            Albums = [..albums],
            Artists = [..artists]
        });
    }
}