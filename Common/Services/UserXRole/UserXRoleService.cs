using Common.Attributes.DependencyInjection;
using Common.Models.UserXRole;
using Common.Repositories.UserXRole;
using Common.Services.Database;

namespace Common.Services.UserXRole
{
    [DependencyInjection]
    public class UserXRoleService : IUserXRoleService
    {
        private readonly IDatabaseService _databaseService;

        public UserXRoleService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task AddRole(int roleId, int userId)
        {
            await _databaseService.Create(Sql.User_UserXRole_AddRole, parameters: new { RoleId = roleId, UserId = userId });
        }

        public async Task<GetRoleDto?> GetRoleByStrongName(string strongName)
        {
            return await _databaseService.Get<GetRoleDto>(Sql.User_UserXRole_GetRole, parameters: new { StrongName = strongName });
        }

        public async Task<IEnumerable<GetRoleDto>> GetRolesForUser(int userId)
        {
            return await _databaseService.GetList<GetRoleDto>(Sql.User_UserXRole_GetRolesForUser, parameters: new { UserId = userId });
        }

        public async Task RemoveRole(int userId, int roleId)
        {
            await _databaseService.Create(Sql.User_UserXRole_RemoveRole, parameters: new { RoleId = roleId, UserId = userId });
        }
    }
}
