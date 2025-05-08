
using DTOs.Search;
using Results;

namespace Services.Interfaces;

public interface ISearchService
{
  Task<CustomResult<SearchResponse>> Search(string query);
}