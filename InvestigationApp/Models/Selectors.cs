using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvestigationApp.Models
{
    public abstract class BaseSelector
    {
        public string ProviderName { get; set; }
        public string SourceItem { get; set; }
    }

    public class Selectors
    {
        public GoogleSelector GoogleSelectors { get; set; }
        public BingSelector BingSelectors { get; set; }
    }

    public class GoogleSelector : BaseSelector
    {
    }

    public class BingSelector : BaseSelector
    {

    }
}
