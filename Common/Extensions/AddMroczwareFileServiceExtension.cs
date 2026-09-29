using Common.Services.File;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Extensions
{
    /// <summary>
    /// Rejestracja HttpClients z appsettings.json
    /// </summary>
    public static class AddFileServiceExtensionExtension
    {
        public static IServiceCollection AddMroczwareFileService(this IServiceCollection services)
        {
            services.AddScoped<IFileService, FileService>();
            return services;
        }
    }
}
