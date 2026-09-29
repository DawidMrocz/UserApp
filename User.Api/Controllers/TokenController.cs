using Common.Models.Token;
using Microsoft.AspNetCore.Mvc;
using User.Api.Services.Authentication;

namespace User.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : AuthroizedController
    {
        private readonly IAuthenticationService _authenticationService;

        private readonly ILogger<TokenController> _logger;

        public TokenController(ILogger<TokenController> logger, IAuthenticationService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Akcja do odœwie¿ania tokena
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Akcja do odœwie¿ania tokena</returns>
        [HttpPost("refresh")]
        public async Task<ActionResult<RefreshTokenResponse>> GetToken([FromBody] RefreshTokenRequest request)
        {
            return await _authenticationService.RefreshToken(request.RefreshToken);
        }
    }
}
