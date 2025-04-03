using Microsoft.AspNetCore.Mvc;
using SearchScraper.Controllers;
using Microsoft.EntityFrameworkCore;
using SearchScraper.Models;
using SearchScraperTests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using Moq;

namespace SearchScraperTests.Repositories
{
    public class SearchRepositoryTests
    {
        private readonly Mock<ISearchRepository> _mockRepo;
        private readonly ISearchRepository _searchRepository;

        public SearchRepositoryTests()
        {
            _mockRepo = RepositoryMocks.GetSearchRepository();
            _searchRepository = _mockRepo.Object;
        }

        [Fact]
        public void GetAllSearchResults_ShouldReturnAllActiveResults()
        {
            //Act
            var results = _searchRepository.GetAllSearchResults;

            //Assert
            Assert.NotNull(results);
            Assert.True(results.All(r => r.InActive == null)); 
            Assert.True(results.SequenceEqual(results.OrderBy(r => r.Position)));
        }

        [Fact]
        public void GetSearchResults_ShouldReturnFilteredResults()
        {
            //Arrange
            string searchPhrase = "land registry searches";

            //Act
            var results = _searchRepository.GetSearchResults(searchPhrase).ToList();

            //Assert
            Assert.NotNull(results);
            Assert.True(results.All(r => r.SearchPhrase == searchPhrase));
            Assert.True(results.SequenceEqual(results.OrderBy(r => r.Position)));
        }

        [Fact]
        public void SaveSearchResult_ShouldAddResult()
        {
            //Arrange
            var newResult = new SearchResult
            {
                SearchPhrase = "test phrase",
                SearchUrl = "https://example.com",
                Position = 5,
                CreatedAt = DateTime.Now
            };

            //Act
            _mockRepo.Object.SaveSearchResult(newResult);

            //Assert
            _mockRepo.Verify(repo => repo.SaveSearchResult(It.IsAny<SearchResult>()), Times.Once);
        }
    }
}
