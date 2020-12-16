using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace InvestigationApp.Application.Extensions
{
    public static class HtmlExtensions
    {
        public static string StripHtml(this string input)
        {
            return string.IsNullOrWhiteSpace(input) ? string.Empty : HttpUtility.HtmlDecode(Regex.Replace(input, "<.*?>", string.Empty));
        }

        public static string GetBetweenStrings(string text, string start, string end)
        {
            var first = text.IndexOf(start, StringComparison.Ordinal) + start.Length;
            var last = text.IndexOf(end, first, StringComparison.Ordinal);

            return end == "" ? text.Substring(first) : text.Substring(first, last - first);
        }
    }
}
