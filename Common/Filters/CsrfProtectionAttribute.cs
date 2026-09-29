using Common.ApiModels.User;
using Common.Models.Authentication;
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
    public class CsrfProtectionAttribute : ActionFilterAttribute
    {
        private const string LoggedUserKey = "UserSessionKey";

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (actionContext.ActionDescriptor.EndpointMetadata.Any(d => d.GetType() == typeof(AllowAnonymousAttribute)))
            {
                base.OnActionExecuting(actionContext);
                return;
            }

            if (actionContext.HttpContext.Request.Method == "GET")
            {
                //Tylko chronimy POST, PUT, DELETE, bo GET beda chronione przez Same Origin Policy
                base.OnActionExecuting(actionContext);
                return;
            }

            IHttpContextAccessor httpContextAccessor = actionContext.HttpContext.RequestServices.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor
            ?? throw new Exception("Nie znaleziono serwisu HttpContextAccessor");

            string? userSession = httpContextAccessor.HttpContext?.Session.GetString(LoggedUserKey);

            if (string.IsNullOrWhiteSpace(userSession))
            {
                actionContext.Result = new ObjectResult(new { Message = "Użytkownik nie autoryzowany" })
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
                return;
            }

            JsonSerializerSettings settings = new();
            settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
            SessionUser userFromSession = JsonConvert.DeserializeObject<SessionUser>(userSession, settings)
                ?? throw new Exception("Nie znaleziono sesji dla użytkownika");

            // Sprawdzenie, czy nagłówek X-CSRF-Token jest obecny
            if (actionContext.HttpContext.Request.Headers.TryGetValue("TokenCSRF", out Microsoft.Extensions.Primitives.StringValues value))
            {
                if (userFromSession.CsrfToken != value)
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

            actionContext.Result = new ObjectResult(new { Message = "Brak tokenu CSRF" })
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };

            return;
        }
    }
}
