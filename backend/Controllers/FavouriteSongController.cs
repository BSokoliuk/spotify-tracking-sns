using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DTOs;
using Helpers;
using Services.Interfaces;
using Results;

namespace Controllers;

[ApiController]
[Route("api/favourite-song")]
public class FavoriteSongController(IFavoriteSongService favoriteSongService) : ControllerBase
{
    private readonly IFavoriteSongService _favoriteSongService = favoriteSongService;

    [HttpPost("create")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> AddSongToFavorites([FromBody] FavouriteSongRequest request)
    {
        var userId = User.GetUserId();
        var result = await _favoriteSongService.AddFavoriteSong(request.SongId, userId);
        if (result.IsFailure)
        {
            if (result.Error!.Code == CustomError.ValidationErrorCode)
                return BadRequest(new { Success = false, Error = result.Error.Message });

            return NotFound(new { Success = false, Error = result.Error.Message });
        }
        
        return Ok(result.Value);
    }

    [HttpDelete("delete")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> DeleteSongFromFavorites([FromBody] FavouriteSongRequest request)
    {

        var userId = User.GetUserId();
        var result = await _favoriteSongService.DeleteFavoriteSong(request.SongId, userId);
        if (result.IsFailure)
        {
            if (result.Error!.Code == CustomError.ValidationErrorCode)
                return BadRequest(new { Success = false, Error = result.Error.Message });

            return NotFound(new { Success = false, Error = result.Error.Message });
        }
        
        return Ok(result.Value);
    }

    //for all users
    //most liked songs
    [HttpGet("most-liked")]
    public async Task<ActionResult> GetMostLikedSongs()
    {
        var songs = await _favoriteSongService.GetMostLikedSongs(10);
        return Ok(new FavouriteSongListResponse
        {
            Success = true,
            Message = songs.Count != 0 ? "Songs retrieved successfully" : "No songs found",
            FavouriteSongs = songs
        });
    }
}