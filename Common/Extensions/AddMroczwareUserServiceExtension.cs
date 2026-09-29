using Common.Services.Authentication;
using Common.Services.OTP;
using Common.Services.Token;
using Common.Services.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace Common.Extensions
{
    public static class AddMroczwareUserServiceExtension
    {
        public static IServiceCollection AddMroczwareUserService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserService, UserService>();
                  
            return services;
        }
    }
}
