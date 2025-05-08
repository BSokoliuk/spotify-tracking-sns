using Microsoft.AspNetCore.Mvc;
using DTOs.Search;
using DTOs;
using Services.Interfaces;

namespace Controllers;

[ApiController]
[Route("api/search")]
public class SearchController(ISearchService searchService) : ControllerBase
{
    private readonly ISearchService _searchService = searchService;

    [HttpGet("{query}")]
    public async Task<IActionResult> Search(string query)
    {
        query = query.Trim().ToLower();
        var result = await _searchService.Search(query);
        if (result.IsFailure)
        {
            return BadRequest(new ApiResponse(false, result.Error!.Message));
        }

        return Ok(new SearchResponse
        {
            Success = true,
            Users = result.Value.Users,
            Songs = result.Value.Songs,
            Albums = result.Value.Albums,
            Artists = result.Value.Artists
        });
    }
}