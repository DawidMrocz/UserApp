
using Common.ApiModels.User;
using Common.Filters;
using Microsoft.AspNetCore.Mvc;
using User.Api.Services.Authentication;

namespace User.Api.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [ServerExceptionTypeFilter]
    [ModelStateCheck]
    public abstract class AuthroizedController : ControllerBase
    {
        public string Culture => (HttpContext.Request.Cookies.TryGetValue("culture", out string? value) ? value : "pl-PL") ?? "pl-PL";
        protected AuthenticationJwtUser? AuthorizedUser => AuthenticationService.GetAuthenticatedUser().GetAwaiter().GetResult();
        protected IAuthenticationService AuthenticationService => HttpContext?.RequestServices.GetService(typeof(IAuthenticationService)) as IAuthenticationService
            ?? throw new Exception("Authorization provider not found");
    }
}
