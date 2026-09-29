using Common.Models.Role;

namespace Common.ApiModels.User
{
    public class AuthenticationJwtUser
    {
        public int Id { get; set; } = default!;
        public string Email { get; set; } = null!;
        public string? FirstName { get; set; } = null!;
        public string? LastName { get; set; } = null!;
        public List<RoleModel> Roles { get; set; } = [];
    }
}
