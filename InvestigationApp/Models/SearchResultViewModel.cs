﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvestigationApp.Models
{
    public class SearchResultViewModel
    {
        public string SourceURL { get; set; }
        public string SourceTitle { get; set; }
        public string Summary { get; set; }
        public string Provider { get; set; }

        public SearchResultViewModel(string sourceUrl, string sourceTitle, string summary, string provider)
        {
            Provider = provider;
            SourceURL = sourceUrl;
            SourceTitle = sourceTitle;
            Summary = summary;
        }

        public SearchResultViewModel()
        {
            
        }

    }
}
