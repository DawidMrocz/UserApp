using Common.ApiModels.User;
using Common.Models.Authentication;
using Common.Models.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Net;

namespace Common.Filters
{
    /// <summary>
    /// Ochrona CSRF
    /// </summary>
    public class CsrfJwtProtectionAttribute : ActionFilterAttribute
    {
        private const string JwtTokenSession = "JwtTokenSession";
        private const string CSRFTokenSession = "CSRFTokenSession";

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (actionContext.ActionDescriptor.EndpointMetadata.Any(d => d.GetType() == typeof(AllowAnonymousAttribute)))
            {
                base.OnActionExecuting(actionContext);
                return;
            }

            IHttpContextAccessor httpContextAccessor = actionContext.HttpContext.RequestServices.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor
            ?? throw new Exception("Nie znaleziono serwisu HttpContextAccessor");

            string? jwtToken = httpContextAccessor.HttpContext?.Session.GetString(JwtTokenSession);
            

            if (string.IsNullOrWhiteSpace(jwtToken))
            {
                actionContext.Result = new ObjectResult(new { Message = "Użytkownik nie autoryzowany" })
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
                return;
            }

            string? csrfToken = httpContextAccessor.HttpContext?.Session.GetString(CSRFTokenSession);

            if (string.IsNullOrWhiteSpace(csrfToken))
            {
                actionContext.Result = new ObjectResult(new { Message = "Brak tokenu CSRF" })
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
                return;
            }

            JsonSerializerSettings settings = new();
            settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
            JwtTokenResponse jwtTokenResponse = JsonConvert.DeserializeObject<JwtTokenResponse>(jwtToken, settings)
                ?? throw new Exception("Nie znaleziono sesji dla użytkownika");

            // Sprawdzenie, czy nagłówek X-CSRF-Token jest obecny
            if (actionContext.HttpContext.Request.Headers.TryGetValue("X-Csrf-Token", out Microsoft.Extensions.Primitives.StringValues value))
            {
                if (csrfToken != value)
                {
                    actionContext.Result = new ObjectResult(new { Message = "Nie poprawny token CSRF" })
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized,
                    };
                    return;
                }
                else
                {
                    base.OnActionExecuting(actionContext);
                    return;
                }
            }

            actionContext.Result = new ObjectResult(new { Message = "Brak autoryzacji" })
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
            };

            return;
        }
    }
}
