namespace SearchScraper.Models
{
    public interface ISearchRepository
    {
        void SaveSearchResult(SearchResult result);
        List<SearchResult> GetAllSearchResults { get; }
        IEnumerable<SearchResult> GetSearchResults(string searchPhrase);
    }
}
