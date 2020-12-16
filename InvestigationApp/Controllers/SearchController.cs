using InvestigationApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvestigationApp.Data.Models;
using InvestigationApp.Infrastructure.Services;

namespace InvestigationApp.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchService _searchService;
        private readonly ILogger<SearchController> _logger;

        public SearchController(ISearchService searchService, ILogger<SearchController> logger)
        {
            _searchService = searchService;
            _logger = logger;
            _logger.LogTrace($"Created {nameof(SearchController)}.");
        }

        public async Task<IActionResult> GetSearchResults(string searchTerm, int startFrom)
        {
            if (startFrom <= 1)
            {
                startFrom = 0;
            }

            _logger.LogInformation($"Calling {nameof(ISearchService.GetGoogleResultsBySearchTerm)} with term {searchTerm}.");
            var googleResults = await _searchService.GetGoogleResultsBySearchTerm(searchTerm, startFrom);

            _logger.LogInformation($"Calling {nameof(ISearchService.GetBingResultsBySearchTerm)} with term {searchTerm}.");
            var bingResults = await _searchService.GetBingResultsBySearchTerm(searchTerm, startFrom);

            if (googleResults == null && bingResults == null)
            {
                _logger.LogInformation("No results were found by the term.");
                return NotFound();
            }

            _logger.LogInformation("Combining both search results.");
            var results = new List<Search>();
            if (googleResults != null)
            {
                results.AddRange(googleResults);
            }

            if (bingResults != null)
            {
                results.AddRange(bingResults);
            }

            var searchResultsViewModel = results.Select(x => new SearchResultViewModel(x.Link, x.Title, x.Description, x.Provider)).ToList();

            return PartialView("_SearchResults", searchResultsViewModel);
        }
    }
}
