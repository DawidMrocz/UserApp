using Common.ApiModels.Access.Request;
using Common.ApiModels.User;
using Common.Extensions;
using Common.Models.Token;
using Gateway.Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Gateway.Api.Controllers.User
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : GatewayController
    {
        private const string JwtTokenSession = "JwtTokenSession";
        private const string CSRFTokenSession = "CSRFTokenSession";
        private readonly HttpClient _userClient;
        private readonly ITokenService _tokenService;

        public UserController(IHttpClientFactory httpClientFactory, ITokenService tokenService)
        {
            _userClient = httpClientFactory.CreateClient("UserApi");
            _tokenService = tokenService;
        }

        /// <summary>
        /// Zalogowanie użytkownika
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Zalogowanie użytkownika</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            JwtTokenResponse? response = await _userClient.Create<JwtTokenResponse, LoginRequest>("user/login", request);

            string csrfTokenGuid = Guid.NewGuid().ToString();
            HttpContext.Session.SetString(JwtTokenSession, JsonConvert.SerializeObject(response));
            HttpContext.Session.SetString(CSRFTokenSession, csrfTokenGuid);
            return Ok(new { CSRFToken = csrfTokenGuid });
        }

        /// <summary>
        /// Wylogowanie użytkownika
        /// </summary>
        /// <returns>Wylogowanie użytkownika</returns>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _userClient.Create("user/logout", _tokenService.GetToken());
            HttpContext?.Session.Clear();
            Response.Cookies.Delete("SessionCookie");
            return Ok("Pomyślnie wylogowano użytkownika");
        }

        /// <summary>
        /// Wylogowanie użytkownika
        /// </summary>
        /// <returns>Wylogowanie użytkownika</returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
        {
            await _userClient.Create<CreateUserRequest>("user/register",request);
            return Ok("Pomyślna rejsteracja");
        }

        /// <summary>
        /// Wylogowanie użytkownika
        /// </summary>
        /// <returns>Wylogowanie użytkownika</returns>
        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            var response = await _userClient.Get<AuthenticationJwtUser>("user/profile", _tokenService.GetToken());
            return Ok(response);
        }

        /// <summary>
        /// Odświeżenie tokena
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Odświeżenie tokena</returns>
        [HttpPost("tokens/refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> RefreshToken()
        {
            string? jwtTokenSession = HttpContext?.Session.GetString(JwtTokenSession);

            if (string.IsNullOrWhiteSpace(jwtTokenSession))
                return Unauthorized("Użytkownik nie autoryzowany");

            JsonSerializerSettings settings = new();
            settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
            JwtTokenResponse userFromSession = JsonConvert.DeserializeObject<JwtTokenResponse>(jwtTokenSession, settings)
                ?? throw new Exception("Nie znaleziono sesji dla użytkownika");

            if (userFromSession.RefreshTokenExpire <= DateTime.Now)
                return Unauthorized("Użytkownik nie autoryzowany");

            RefreshTokenResponse? response = await _userClient.Create<RefreshTokenResponse, string>("token/refresh", userFromSession.RefreshToken, _tokenService.GetToken());

            string csrfTokenGuid = Guid.NewGuid().ToString();
            HttpContext?.Session.SetString(JwtTokenSession, JsonConvert.SerializeObject(response));
            HttpContext?.Session.SetString(CSRFTokenSession, csrfTokenGuid);
            return Ok(new { CSRFToken = csrfTokenGuid });
        }
    }
}
