using Moq;
using SearchScraper.Models;

namespace SearchScraperTests.Mocks
{
    public class RepositoryMocks
    {
        public static Mock<ISearchRepository> GetSearchRepository(string searchPhrase = "land registry searches")
        {
            Dictionary<int, string> searchUrls = new Dictionary<int, string>
            {
                { 1, "https://www.infotrack.co.uk/solutions/conveyancing/land-registry-searches/"},
                { 2, "https://www.infotrack.co.uk/solutions/conveyancing/land-registry-searches/_2"},
                { 3, "https://www.infotrack.co.uk/solutions/conveyancing/property-searches/"}
            };

            List<DateTime> dates = new List<DateTime>
                {
                    new DateTime(2025, 04, 02),
                    new DateTime(2025, 04, 01),
                    new DateTime(2025, 03, 31),
                    new DateTime(2025, 03, 30),
                    new DateTime(2025, 03, 29),
                    new DateTime(2025, 03, 23),
                    new DateTime(2025, 03, 19),
                    new DateTime(2025, 03, 14),
                    new DateTime(2025, 03, 09),
                    new DateTime(2025, 03, 04),
                    new DateTime(2025, 02, 22),
                    new DateTime(2025, 02, 12),
                    new DateTime(2024, 12, 24),
                    new DateTime(2024, 11, 04),
                    new DateTime(2024, 09, 15),
                    new DateTime(2024, 07, 27),
                    new DateTime(2024, 06, 07)
                };

            List<SearchResult> searchResults = new List<SearchResult>();

            foreach (DateTime date in dates)
            {
                foreach (KeyValuePair<int, string> searchUrl in searchUrls)
                {
                    int position;
                    var rand = new Random();

                    switch (searchUrl.Key)
                    {
                        case 1:
                            position = rand.Next(10, 22);
                            break;
                        case 2:
                            position = rand.Next(28, 60);
                            break;
                        case 3:
                            position = rand.Next(65, 98);
                            break;
                        default:
                            position = 1;
                            break;
                    }

                    searchResults.Add(new SearchResult
                    {
                        SearchPhrase = searchPhrase,
                        SearchUrl = searchUrl.Value,
                        Position = position,
                        CreatedAt = date
                    });
                }
            }

            var mockSearchRepository = new Mock<ISearchRepository>();
            mockSearchRepository.Setup(repo => repo.GetAllSearchResults).Returns(searchResults.OrderBy(s => s.Position).ToList());
            mockSearchRepository.Setup(repo => repo.GetSearchResults(searchPhrase)).Returns(searchResults.Where(r => r.SearchPhrase.Equals(searchPhrase)).OrderBy(s => s.Position));
            mockSearchRepository.Setup(repo => repo.SaveSearchResult(It.IsAny<SearchResult>())).Callback<SearchResult>(s => searchResults.Add(s));
            return mockSearchRepository;
        }
    }
}
