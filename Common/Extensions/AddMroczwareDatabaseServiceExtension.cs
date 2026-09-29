using Common.Services.Database;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Extensions
{
    public static class AddMroczwareDatabaseServiceExtension
    {
        public static IServiceCollection AddMroczwareDatabaseService(this IServiceCollection services)
        {
            services.AddSingleton<IDatabaseService, DatabaseService>();
            return services;
        }
    }
}
