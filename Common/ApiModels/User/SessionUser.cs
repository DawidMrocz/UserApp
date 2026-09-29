using Common.Models.Authentication;

namespace Common.ApiModels.User
{
    public class SessionUser : AuthenticationUser
    {
        public string CsrfToken { get; set; } = null!;
    }
}
