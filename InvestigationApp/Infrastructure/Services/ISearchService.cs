using InvestigationApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InvestigationApp.Data.Services
{
    public interface ISearchService
    {
        Task<List<Search>> GetBySearchTerm(string searchTerm);
    }
}
