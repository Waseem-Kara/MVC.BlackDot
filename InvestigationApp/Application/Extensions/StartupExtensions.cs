using InvestigationApp.Data.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvestigationApp.Models;
using Microsoft.Extensions.Configuration;

namespace InvestigationApp.Application.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ISearchService, SearchService>();

            return services;
        }

        public static IServiceCollection BindConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("Selectors");
            services.Configure<Selectors>(section);

            return services;
        }
    }
}
