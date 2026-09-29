using Common.Models.UserXRole;

namespace Common.Services.UserXRole
{
    public interface IUserXRoleService
    {
        Task AddRole(int roleId, int userId);
        Task<GetRoleDto?> GetRoleByStrongName(string strongName);
        Task<IEnumerable<GetRoleDto>> GetRolesForUser(int userId);
        Task RemoveRole(int userId, int roleId);
    }
}
