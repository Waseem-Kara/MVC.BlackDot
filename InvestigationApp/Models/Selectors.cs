using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvestigationApp.Models
{
    public class Selectors
    {
        public GoogleSelector GoogleSelectors { get; set; }
    }

    public class GoogleSelector
    {
        public string GoogleSourceItem { get; set; }
    }
}
