using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MovieDeck.Repository.Extensions
{
    public static class RepoExtension
    {
        public static IServiceCollection AddRepoExt(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddScoped<ICategoryRepository, CategoryRepository>();
          
            return services;
        }
    }
}
