using Moq;
using SearchScraper.Models;
using SearchScraper.Services;
using SearchScraperTests.Mocks;

namespace SearchScraperTests.Services
{
    public class SearchScraperServiceTests
    {
        private readonly Mock<ISearchRepository> _mockRepo;
        private readonly SearchScraperService _service;

        public SearchScraperServiceTests()
        {
            _mockRepo = RepositoryMocks.GetSearchRepository();
            _service = new SearchScraperService(_mockRepo.Object);
        }

        [Fact]
        public void ExtractURLResults_ShouldReturnValidUrls()
        {
            //Arrange
            string mockHtml = @"
                <html>
                    <body>
                        <div class='b8lM7'><a href='https://example.com/page1'></a></div>
                        <div class='b8lM7'><a href='https://example.com/page2'></a></div>
                        <div class='b8lM7'><a href='/relative-url'></a></div>
                    </body>
                </html>";

            //Act
            var urls = _service.ExtractURLResults(mockHtml);

            //Assert
            Assert.NotNull(urls);
            Assert.Equal(2, urls.Count);
            Assert.All(urls, url => Assert.StartsWith("https://", url));
        }

        [Fact]
        public void ParseHtmlForResults_ShouldFindMatchingUrls()
        {
            //Arrange
            string searchPhrase = "Test";
            string searchUrl = "https://example.com/page1";
            List<string> mockResults = new List<string>
            {
                "https://example.com/page1",
                "https://example.com/page2",
                "https://another-site.com/page"
            };

            //Act
            var filteredResults = _service.ParseHtmlForResults(mockResults, searchPhrase, searchUrl);

            //Assert
            Assert.NotNull(filteredResults);
            Assert.All(filteredResults, result => Assert.Contains(searchUrl, result.SearchUrl));
        }
    }
}
