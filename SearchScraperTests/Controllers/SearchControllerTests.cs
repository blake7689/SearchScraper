using Moq;
using Microsoft.AspNetCore.Mvc;
using SearchScraper.Services;
using SearchScraper.Models;

namespace SearchScraperTests.Controllers
{
    public class SearchControllerTests
    {
        private readonly Mock<SearchScraperService> _mockScraperService;
        private readonly SearchController _controller;

        public SearchControllerTests()
        {
            _mockScraperService = new Mock<SearchScraperService>(Mock.Of<ISearchRepository>());
            _controller = new SearchController(_mockScraperService.Object);
        }

        [Fact]
        public void GetResults_ReturnsBadRequest_WhenQueryParamsAreMissing()
        {
            //Act
            var result = _controller.GetResults(null, null);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Contains("Search query and URL are required", badRequestResult.Value.ToString());
        }
    }
}
