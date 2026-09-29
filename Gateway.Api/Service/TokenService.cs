using Common.Attributes.DependencyInjection;
using Common.Models.Token;
using Newtonsoft.Json;

namespace Gateway.Api.Service
{
    [DependencyInjection]
    public class TokenService : ITokenService
    {
        private const string JwtTokenSession = "JwtTokenSession";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Dictionary<string, string?> GetToken()
        {
            string? jwtTokenSession = _httpContextAccessor.HttpContext?.Session.GetString(JwtTokenSession);

            if (string.IsNullOrWhiteSpace(jwtTokenSession))
                throw new UnauthorizedAccessException("Użytkownik nie autoryzowany");

            JsonSerializerSettings settings = new();
            settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
            JwtTokenResponse userFromSession = JsonConvert.DeserializeObject<JwtTokenResponse>(jwtTokenSession, settings)
                ?? throw new UnauthorizedAccessException("Użytkownik nie autoryzowany");

            if (userFromSession.RefreshTokenExpire <= DateTime.Now)
                throw new UnauthorizedAccessException("Użytkownik nie autoryzowany");

            return new Dictionary<string, string?>() { { "Authorization", $"Bearer {userFromSession.JwtToken}" } };
        }
    }
}
