namespace SearchScraper.Models
{
    public class SearchRepository : ISearchRepository
    {
        private readonly SearchScraperDbContext _searchScraperDbContext;

        public SearchRepository(SearchScraperDbContext searchScraperDbContext)
        {
            _searchScraperDbContext = searchScraperDbContext;
        }

        public void SaveSearchResult(SearchResult result)
        {
            _searchScraperDbContext.SearchResults.Add(result);
            _searchScraperDbContext.SaveChanges();
        }

        public List<SearchResult> GetAllSearchResults
        {
            get
            {
                return _searchScraperDbContext.SearchResults
                               .Where(r => r.InActive.Equals(null))
                               .OrderBy(r => r.Position)
                               .ToList();
            }
        }

        public IEnumerable<SearchResult> GetSearchResults(string searchPhrase)
        {
            return _searchScraperDbContext.SearchResults
                           .Where(r => r.SearchPhrase == searchPhrase && r.InActive.Equals(null))
                           .OrderBy(r => r.Position)
                           .ToList();
        }
    }
}
