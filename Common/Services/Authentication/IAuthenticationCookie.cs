using Common.ApiModels.Access.Request;
using Common.ApiModels.User;
using Common.Models.Authentication;

namespace Common.Services.Authentication
{
    public interface IAuthenticationCookie
    {
        Task<SessionUser> Login(LoginRequest dto, string? ipAddress, string? userAgent);
        void Logout();
        Task<AuthenticationUser?> GetAuthenticatedUser();
        //Task<string> OTPLogin(LoginRequest dto, string? ipAddress, string? userAgent, bool twoFactorAuth, string culture);
        // Task VerifyOneTimePasscode(Guid guid, string passcode);
    }
}
