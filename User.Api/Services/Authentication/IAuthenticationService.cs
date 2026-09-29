using Common.ApiModels.Access.Request;
using Common.ApiModels.User;
using Common.Models.Token;

namespace User.Api.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<JwtTokenResponse> Login(LoginRequest request);
        Task Logout();
        Task<AuthenticationJwtUser?> GetAuthenticatedUser();
        Task<RefreshTokenResponse> RefreshToken(string incomingRefreshToken);
    }
}
