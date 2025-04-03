using Microsoft.AspNetCore.Mvc;
using SearchScraper.Models;
using SearchScraper.Services;

[Route("api/[controller]")]
[ApiController]
public class SearchController : ControllerBase
{
    private readonly SearchScraperService _searchScraperService;

    public SearchController(SearchScraperService scraperService)
    {
        _searchScraperService = scraperService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<SearchResult>> GetResults([FromQuery] string searchPhrase, [FromQuery] string searchUrl)
    {
        if (string.IsNullOrWhiteSpace(searchPhrase) || string.IsNullOrWhiteSpace(searchUrl))
        {
            return BadRequest(new { message = "Search query and URL are required." });
        }

        var results = _searchScraperService.ScrapeSearchResults(searchPhrase, searchUrl);

        if (results == null || results.Count == 0)
        {
            return NotFound(new { message = "No results found." });
        }

        return Ok(results);
    }
}
