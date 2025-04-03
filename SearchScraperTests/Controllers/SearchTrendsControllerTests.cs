using Moq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SearchScraper.Models;
using SearchScraperTests.Mocks;
using YourNamespace.Controllers;

namespace SearchScraperTests.Controllers
{
    public class SearchTrendsControllerTests
    {
        private readonly Mock<ISearchRepository> _mockRepo;
        private readonly SearchTrendsController _controller;

        public SearchTrendsControllerTests()
        {
            _mockRepo = RepositoryMocks.GetSearchRepository();
            _controller = new SearchTrendsController(_mockRepo.Object);
        }

        [Fact]
        public void Index_ShouldReturnViewWithData()
        {
            //Act
            var result = _controller.Index() as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.ViewData["PositionData"]);
            var positionData = JsonConvert.DeserializeObject<List<dynamic>>(result.ViewData["PositionData"].ToString());
            Assert.NotEmpty(positionData);
        }

        [Fact]
        public void Index_ShouldGroupAndSortDataCorrectly()
        {
            //Act
            var result = _controller.Index() as ViewResult;
            var positionDataJson = result.ViewData["PositionData"].ToString();
            var positionData = JsonConvert.DeserializeObject<List<dynamic>>(positionDataJson);

            //Verify data is grouped correctly
            var uniqueGroups = positionData.Select(p => new { p.SearchUrl, p.Date }).Distinct().ToList();
            Assert.Equal(positionData.Count, uniqueGroups.Count);

            //Verify data is sorted by date
            var sortedDates = positionData.Select(p => (DateTime)p.Date).OrderBy(d => d).ToList();
            Assert.Equal(sortedDates, positionData.Select(p => (DateTime)p.Date).ToList());
        }
    }
}
