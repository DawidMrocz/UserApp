using Common.Services.Email;
using Common.Services.Template;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Extensions
{
    /// <summary>
    /// Rejestracja HttpClients z appsettings.json
    /// </summary>
    public static class AddMroczwareEmailServiceExtension
    {
        public static IServiceCollection AddMroczwareEmailService(this IServiceCollection services)
        {
            services.AddScoped<ITemplateService, TemplateService>();
            services.AddScoped<IEmailService, EmailService>();
            return services;
        }
    }
}
