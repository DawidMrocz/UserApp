using Common.ApiModels.Access.Request;
using Common.ApiModels.User;
using Common.Models.User;

namespace Common.Services.User
{
    public interface IUserService
    {
        Task Activate(int userId, string activateCode);
        Task Block(int id, DateTime? blockTo);
        Task ChangePassword(int id, ChangePasswordRequest request);
        Task<int> Create(CreateUserRequest request);
        string CreatePassword(int length);
        Task Delete(int id);
        Task<UserModel?> Get(int userId);
        Task<UserModel?> GetByEmail(string email);
        Task<GetUserResponse> Profile(int userId);
        Task RemindPassword(UserForgotPassword request, string culture);
        Task Unblock(int id);
        Task Update(int id, UpdateUserRequest request);
    }
}