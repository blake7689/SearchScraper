using Microsoft.AspNetCore.Mvc;
using Moq;
using SearchScraper.Controllers;
using SearchScraper.Models;
using Xunit;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace SearchScraperTests.Controllers
{
    public class HomeControllerTests
    {
        private readonly Mock<ILogger<HomeController>> _mockLogger;
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            _mockLogger = new Mock<ILogger<HomeController>>();
            _controller = new HomeController(_mockLogger.Object);
        }

        [Fact]
        public void Index_ReturnsViewResult()
        {
            //Act
            var result = _controller.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Search_ReturnsViewResult()
        {
            //Act
            var result = _controller.Search();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Error_ReturnsErrorView()
        {
            // Arrange
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(ctx => ctx.TraceIdentifier).Returns("12345");

            // Assign the mock HttpContext to the controller
            _controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            // Act
            var result = _controller.Error();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result); 
            var model = Assert.IsAssignableFrom<ErrorViewModel>(viewResult.Model); 
            Assert.Equal("12345", model.RequestId);
        }
    }
}
