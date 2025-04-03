using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SearchScraper.Models;

namespace YourNamespace.Controllers
{
    public class SearchTrendsController : Controller
    {
        private readonly ISearchRepository _searchRepository;

        public SearchTrendsController(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }

        public IActionResult Index()
        {
            var allSearchResults = _searchRepository.GetAllSearchResults;

            var positionData = allSearchResults
                .GroupBy(r => new { r.SearchUrl, r.CreatedAt.Date })
                .Select(g => new
                {
                    SearchUrl = g.Key.SearchUrl,
                    Date = g.Key.Date,
                    Positions = g.Select(r => r.Position).ToList()
                })
                .OrderBy(t => t.Date)
                .ToList();

            ViewData["PositionData"] = JsonConvert.SerializeObject(positionData);

            return View();
        }
    }
}
