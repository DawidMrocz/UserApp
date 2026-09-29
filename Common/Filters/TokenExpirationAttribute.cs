using Common.ApiModels.User;
using Common.Models.Token;
using Common.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Net;

namespace Common.Filters
{
    public class TokenExpirationAttribute : ActionFilterAttribute
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

            string? jwtTokenSession = httpContextAccessor.HttpContext?.Session.GetString(JwtTokenSession);

            if (string.IsNullOrWhiteSpace(jwtTokenSession))
            {
                actionContext.Result = new ObjectResult(new { Message = "Użytkownik nie autoryzowany" })
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
                return;
            }

            JsonSerializerSettings settings = new();
            settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
            JwtTokenResponse userFromSession = JsonConvert.DeserializeObject<JwtTokenResponse>(jwtTokenSession, settings)
                ?? throw new Exception("Nie znaleziono sesji dla użytkownika");


            if (userFromSession.JwtTokenExpire <= DateTime.UtcNow)
            {
                actionContext.Result = new RedirectToActionResult("RefreshToken", "UserController", null);
            }
            else
            {
                base.OnActionExecuting(actionContext);
                return;
            }
        }
    }
}
