using InvestigationApp.Data.Services;
using InvestigationApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IActionResult> GetSearchResults(string searchTerm)
        {
            _logger.LogInformation("");
            var results = await _searchService.GetBySearchTerm(searchTerm);

            if (!results.Any())
            {
                return NotFound();
            }

            var searchResultsViewModel = results.Select(x => new SearchResultViewModel(x.Link, x.Title, x.Description)).ToList();

            return PartialView("_SearchResults", searchResultsViewModel);
        }
    }
}
