using Common.Models.Role;

namespace Common.Models.Authentication
{
    public class AuthenticationUser : IAuthenticationUser
    {
        public int Id { get; set; } = default!;
        public string Email { get; set; } = null!;
        public List<RoleModel> Roles { get; set; } = null!;
        public string? Culture { get; set; } = null!;
    }
}
