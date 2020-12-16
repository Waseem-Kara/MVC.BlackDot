using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using InvestigationApp.Controllers;
using InvestigationApp.Data.Models;
using InvestigationApp.Infrastructure.Services;
using InvestigationApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace InvestigationApp.Tests.Controllers
{
    public class SearchControllerTests
    {
        private readonly Mock<ISearchService> _searchService;
        private readonly SearchController _controller;

        public SearchControllerTests()
        {
            var logger = new Mock<ILogger<SearchController>>();

            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();
            var mockHeaderDictionary = new Mock<HeaderDictionary>();

            mockHttpContext.Setup(s => s.Request).Returns(mockHttpRequest.Object);
            mockHttpRequest.Setup(s => s.Headers).Returns(mockHeaderDictionary.Object);

            _searchService = new Mock<ISearchService>();

            _controller = new SearchController(_searchService.Object, logger.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };
        }



        [Fact]
        [Trait("Category", nameof(SearchController.GetSearchResults))]
        public async Task GetSearchResults_ResultNotNull_Test()
        {
            #region Arrange

            var googleSearchResults = new List<Search>();
            var bingSearchResults = new List<Search>();

            _searchService.Setup(s => s.GetGoogleResultsBySearchTerm(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(googleSearchResults));

            _searchService.Setup(s => s.GetBingResultsBySearchTerm(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(bingSearchResults));

            #endregion

            #region Act

            var response = await _controller.GetSearchResults(It.IsAny<string>(), It.IsAny<int>());

            #endregion

            #region Assert

            Assert.NotNull(response);

            #endregion
        }

        [Fact]
        [Trait("Category", nameof(SearchController.GetSearchResults))]
        public async Task GetSearchResults_ResultTypeOfSearchType_Test()
        {
            #region Arrange

            var googleSearchResults = new List<Search>();
            var bingSearchResults = new List<Search>();

            _searchService.Setup(s => s.GetGoogleResultsBySearchTerm(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(googleSearchResults));

            _searchService.Setup(s => s.GetBingResultsBySearchTerm(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(bingSearchResults));

            #endregion

            #region Act

            var response = await _controller.GetSearchResults(It.IsAny<string>(), It.IsAny<int>());

            #endregion

            #region Assert

            Assert.True(response is PartialViewResult);

            #endregion
        }

        [Theory]
        [ClassData(typeof(ControllerTestData.NullObject))]
        [Trait("Category", nameof(SearchController.GetSearchResults))]
        public async Task GetSearchResults_ResultTypeOfSearchType_WithStatusCodeNotFound_Test(List<Search> searchResults)
        {
            #region Arrange

            _searchService.Setup(s => s.GetGoogleResultsBySearchTerm(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(searchResults));

            _searchService.Setup(s => s.GetBingResultsBySearchTerm(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(searchResults));

            #endregion

            #region Act

            var response = await _controller.GetSearchResults(It.IsAny<string>(), It.IsAny<int>());

            #endregion

            #region Assert

            Assert.True((response as NotFoundResult).StatusCode is StatusCodes.Status404NotFound);

            #endregion
        }

    }
}
