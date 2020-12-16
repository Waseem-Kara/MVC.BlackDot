using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using InvestigationApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvestigationApp.Controllers
{
    public class DocumentController : Controller
    {
        [HttpPost]
        [Obsolete]
        public FileResult DownloadDocument(List<SummaryViewModel> sourceSummaries)
        {
            if (sourceSummaries == null || !sourceSummaries.Any())
            {
                return null;
            }

            var sb = new StringBuilder();
            foreach (var summary in sourceSummaries)
                sb.AppendLine($"Title: {summary.Title} ||||| Description: {summary.Description} ||||| Link: {summary.Link} ||||| Provider: {summary.Provider}");


            var summaryString = sb.ToString();
            var byteArray = Encoding.ASCII.GetBytes(summaryString);
            var stream = new MemoryStream(byteArray);
            return File(stream, "text/plain", "SummaryOfSources" + DateTime.Now + ".txt");
        }
    }
}
