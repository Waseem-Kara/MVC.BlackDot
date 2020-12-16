using System.Collections.Generic;
using System.Threading.Tasks;
using InvestigationApp.Data.Models;

namespace InvestigationApp.Infrastructure.Services
{
    public interface ISearchService
    {
        Task<List<Search>> GetGoogleResultsBySearchTerm(string searchTerm, int startFrom);
        Task<List<Search>> GetBingResultsBySearchTerm(string searchTerm, int startFrom);
    }
}
