using Common.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Task.Api.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [ServerExceptionTypeFilter]
    [ModelStateCheck]
    public abstract class AuthroizedController : ControllerBase
    {
        public string Culture => (HttpContext.Request.Cookies.TryGetValue("culture", out string? value) ? value : "pl-PL") ?? "pl-PL";
        protected int? AuthorizedUserId => int.TryParse(HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId) ? userId : null;
        protected Dictionary<string, string?> AuthorizationHeaders => new Dictionary<string, string?> { { "Authorization", HttpContext.Request.Headers["Authorization"].FirstOrDefault() } };
    }
}
