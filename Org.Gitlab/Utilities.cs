using System.Collections.Generic;
using Org.Gitlab.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Org.Gitlab
{
    public static class Utilities
    {
        public static IServiceCollection AddGitlabClient(this IServiceCollection services)
        {            
            services.AddHttpClient<GitLabClient>();
            services.AddScoped<PageInfo>();

            return services;
        }

        public static void PopulatePageInfoFromHeader(this PageInfo pi, Dictionary<string, string> header)
        {
            pi.Page = int.Parse(header["X-Page"]);
            pi.PerPage = int.Parse(header["X-Per-Page"]);
            pi.PrevPage = int.TryParse(header["X-Prev-Page"], out int prevPage) ? prevPage : 0;
            pi.NextPage = int.TryParse(header["X-Next-Page"], out int nextPage) ? nextPage : 0;
            pi.Total = int.Parse(header["X-Total"]);
            pi.TotalPages = int.Parse(header["X-Total-Pages"]);
        }

    }
}