using InvestigationApp.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using InvestigationApp.Application.Extensions;
using InvestigationApp.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;

namespace InvestigationApp.Data.Services
{
    public class SearchService : ISearchService
    {
        private readonly ILogger<SearchService> _logger;
        private readonly IOptions<Selectors> _selectors;
        private const string BASE_URL = "http://www.google.com/search?q=";

        public SearchService(ILogger<SearchService> logger, IOptions<Selectors> selectors)
        {
            _logger = logger;
            _selectors = selectors;
            _logger.LogTrace($"Created {nameof(SearchService)}.");
        }

        public async Task<List<Search>> GetBySearchTerm(string searchTerm)
        {
            var search = new Search();



            _logger.LogInformation($"Fetching results by term: {searchTerm}.");
            

            searchTerm = searchTerm.Replace(".", "");
            string fullUrl = BASE_URL + searchTerm;


            var results = new List<Search>();
            var wc = new WebClient();
            string s = wc.DownloadString(fullUrl);
            wc.Dispose();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(s);
            HtmlNodeCollection Links = doc.DocumentNode.SelectNodes("//li[@class='g']");
            string title = "";
            string link = "";
            string desc = "";
            if (Links == null || Links.Count == 0) return null;
            foreach (var node in Links)
            {
                try
                {
                    var sr = new Search();
                    sr.Title = node.FirstChild.FirstChild.InnerHtml;
                    sr.Title = sr.Title.StripHTML();
                    sr.Link = node.FirstChild.FirstChild.Attributes["href"].Value;
                    desc = node.SelectSingleNode("div").InnerText.Trim();
                    sr.Description = desc.StripHTML();
                    results.Add(sr);
                }
                catch (Exception ex)
                {
                    Debug.Write(ex.ToString());
                }
            }
            

            return results;
        }
    }
}
