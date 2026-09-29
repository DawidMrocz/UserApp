using Common.Models.Role;

namespace Common.Models.Authentication
{
    public interface IAuthenticationUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public List<RoleModel> Roles { get; set; }
        public string? Culture { get; set; }
    }
}
