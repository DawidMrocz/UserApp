using Common.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Data;

namespace Common.Filters
{
    public class CookieAuthorizeAttribute : ActionFilterAttribute
    {
        private List<string>? Roles { get; set; }

        public CookieAuthorizeAttribute() { }

        public CookieAuthorizeAttribute(params string[] roles)
        {
            Roles = [.. roles];
        }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            string? culture = actionContext.HttpContext.Request.Cookies["culture"];

            var message = string.Empty;

            if (actionContext.ActionDescriptor.EndpointMetadata.Any(x => x.GetType() == typeof(AllowAnonymousAttribute)))
                return;

            IAuthenticationCookie authenticationService = actionContext.HttpContext.RequestServices
                .GetService(typeof(IAuthenticationCookie)) as IAuthenticationCookie
                ?? throw new Exception("Nie ustawiono serwisu autentykacyjnego");

            IHttpContextAccessor httpContextAccessor = actionContext.HttpContext.RequestServices.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor
              ?? throw new Exception("Nie znaleziono serwisu HttpContextAccessor");

            Models.Authentication.AuthenticationUser? user = authenticationService.GetAuthenticatedUser().Result;

            if (user is null)
            {
                message = "Użytkownik nie autoryzowany";
                actionContext.Result = new UnauthorizedObjectResult(message);
                return;
            }

            culture ??= user.Culture ?? "pl-PL";

            if (Roles is null || !Roles.Any())
            {
                base.OnActionExecuting(actionContext);
                return;
            }

            if (user.Roles.Select(r => r.StrongName).ToList().Intersect([.. Roles], StringComparer.OrdinalIgnoreCase).Any())
            {
                base.OnActionExecuting(actionContext);
                return;
            }

            message = "Użytkownik nie ma odpowiednich uprawnień";
            actionContext.Result = new ObjectResult(new { Message = message })
            {
                StatusCode = 403
            };
        }
    }
}
