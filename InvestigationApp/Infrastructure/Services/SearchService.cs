using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using InvestigationApp.Application.Extensions;
using InvestigationApp.Data.Models;
using InvestigationApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;

namespace InvestigationApp.Infrastructure.Services
{
    public class SearchService : ISearchService
    {
        private readonly ILogger<SearchService> _logger;
        private readonly IOptions<Selectors> _selectors;
        private const string BASE_GOOGLE_URL = "http://www.google.com/search?q=";
        private const string BASE_BING_URL = "https://www.bing.com/search?q=";

        public SearchService(ILogger<SearchService> logger, IOptions<Selectors> selectors)
        {
            _logger = logger;
            _selectors = selectors;
            _logger.LogTrace($"Created {nameof(SearchService)}.");
        }

        public async Task<List<Search>> GetGoogleResultsBySearchTerm(string searchTerm, int startFrom)
        {
            _logger.LogInformation($"Fetching {_selectors.Value.GoogleSelectors.ProviderName} results by term: {searchTerm}.");

            var results = new List<Search>();
            var fullUrl = BASE_GOOGLE_URL + searchTerm;
            if (startFrom > 1)
            {
                fullUrl += $"&start={startFrom * 10}"; 
            }
            var doc = await CreateHtmlDocumentFromResults(fullUrl);
            var sources = doc.DocumentNode.SelectNodes($"//div[@class='{_selectors.Value.GoogleSelectors.SourceItem}']");

            if (sources == null || sources.Count == 0) return null;
            foreach (var node in sources)
            {
                try
                {
                    var title = node.FirstChild.FirstChild.InnerHtml.StripHtml();
                    var link = node.FirstChild.FirstChild.Attributes["href"]?.Value == null ? "" : HttpUtility.UrlDecode(HtmlExtensions.GetBetweenStrings(node.FirstChild.FirstChild.Attributes["href"]?.Value, "/url?q=", "&"));
                    var description = node.InnerText.Trim().StripHtml();
                    var provider = _selectors.Value.GoogleSelectors.ProviderName;

                    var search = new Search(title, provider, link, description);


                    if (!string.IsNullOrWhiteSpace(search.Link) && !string.IsNullOrWhiteSpace(search.Description))
                    {
                        results.Add(search);
                        _logger.LogInformation("Source valid and added to list.");
                    }
                    
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    Debug.Write(ex.ToString());
                }
            }

            return results;
        }

        public async Task<List<Search>> GetBingResultsBySearchTerm(string searchTerm, int startFrom)
        {
            _logger.LogInformation($"Fetching {_selectors.Value.BingSelectors.ProviderName} results by term: {searchTerm}.");

            var results = new List<Search>();
            var fullUrl = BASE_BING_URL + searchTerm;
            if (startFrom > 1)
            {
                fullUrl += $"&first={startFrom * 10}"; ;
            }
            var doc = await CreateHtmlDocumentFromResults(fullUrl);
            var sources = doc.DocumentNode.SelectNodes($"//li[@class='{_selectors.Value.BingSelectors.SourceItem}']");

            if (sources == null || sources.Count == 0) return null;
            foreach (var node in sources)
            {
                try
                {
                   
                    var title = node.FirstChild.FirstChild.InnerHtml.StripHtml();
                    var link = node.FirstChild.FirstChild.Attributes["href"]?.Value;
                    var description = node.SelectSingleNode("div").InnerText.Trim().StripHtml();
                    var provider = _selectors.Value.BingSelectors.ProviderName;

                    var search = new Search(title, provider, link, description);

                    if (!string.IsNullOrWhiteSpace(search.Link) && !string.IsNullOrWhiteSpace(search.Description))
                    {
                        results.Add(search);
                        _logger.LogInformation("Source valid and added to list.");
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    Debug.Write(ex.ToString());
                }
            }

            return results;
        }


        private async Task<HtmlDocument> CreateHtmlDocumentFromResults(string searchUrl)
        {
            string documentString;
            using (var client = new WebClient())
            {
                documentString = await client.DownloadStringTaskAsync(searchUrl);
            }

            var doc = new HtmlDocument();
            doc.LoadHtml(documentString);

            return doc;
        }
    }
}
