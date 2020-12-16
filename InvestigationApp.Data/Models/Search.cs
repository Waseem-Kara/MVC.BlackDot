using System;
using System.Collections.Generic;
using System.Text;

namespace InvestigationApp.Data.Models
{
    public class Search
    {
        public string Title { get; set; }
        public string Provider { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }

        public Search(string title, string provider, string link, string description)
        {
            Title = title;
            Provider = provider;
            Link = link;
            Description = description;
        }
    }
}
