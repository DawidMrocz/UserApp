using Common.Services.Setting;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Extensions
{
    internal static class AddMroczwareSettingServiceExtension
    {
        public static IServiceCollection AddMroczwareSettingService(this IServiceCollection services)
        {
            services.AddScoped<ISettingService, SettingService>();
            return services;
        }
    }
}
