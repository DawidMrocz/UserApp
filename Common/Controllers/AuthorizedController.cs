using Common.Filters;
using Common.Models.Authentication;
using Common.Services.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Controllers
{
    [CsrfProtection]
    [CookieAuthorize]
    public abstract class CookieAuthorizedController(IServiceProvider serviceProvider) : ControllerBase
    {
        public string Culture => (HttpContext.Request.Cookies.TryGetValue("culture", out string? value) ? value : "pl-PL") ?? "pl-PL";
        protected AuthenticationUser? AuthorizedUser => AuthenticationService.GetAuthenticatedUser().Result;
        protected IAuthenticationCookie AuthenticationService => serviceProvider
                .GetRequiredService<IAuthenticationCookie>()
                    ?? throw new Exception("Authorization provider not found");
    }
}
