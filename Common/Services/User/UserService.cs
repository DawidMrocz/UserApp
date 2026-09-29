using Common.ApiModels.Access.Request;
using Common.ApiModels.User;
using Common.Enums.UserRole;
using Common.Models.User;
using Common.Repositories.User;
using Common.Services.Database;
using Common.Services.Email;
using Common.Services.UserXRole;
using MassTransit;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.User
{
    public class UserService : IUserService
    {
        private readonly IDatabaseService _databaseService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IUserXRoleService _userXRoleService;
        public UserService(IDatabaseService databaseService, IEmailService emailService, IConfiguration configuration, IUserXRoleService userXRoleService)
        {
            _databaseService = databaseService;
            _emailService = emailService;
            _configuration = configuration;
            _userXRoleService = userXRoleService;
        }


        public async Task ChangePassword(int id, ChangePasswordRequest request)
        {
            UserModel entity = await Get(id)
                ?? throw new Exception("User not found");

            if (!VerifyPasswordHash(request.OldPassword, entity.PasswordHash, entity.PasswordSalt))
                throw new Exception("Nie poprawne hasło");

            (byte[], byte[]) result = CreatePasswordHash(request.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);

            entity.PasswordHash = result.Item2;
            entity.PasswordSalt = result.Item1;
        }

        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (HMACSHA512 hmac = new(passwordSalt))
            {
                byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private static (byte[], byte[]) CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (HMACSHA512 hmac = new())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
            return (passwordSalt, passwordHash);
        }

        public async Task Delete(int id)
        {
            await _databaseService.Delete(Sql.User_User_Delete, parameters: new { UserId = id });
        }

        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public async Task RemindPassword(UserForgotPassword request, string culture)
        {
            UserModel? entity = await GetByEmail(request.Email);

            if (entity is null) return;

            string newPassword = CreatePassword(8);

            CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);

            entity.PasswordHash = passwordHash;
            entity.PasswordSalt = passwordSalt;

            List<MailAddress> mailAddresses = [new MailAddress(request.Email)];

            StringBuilder title = new();

            if (entity.FirstName is null)
            {
                title.Append("użytkowniku");
            }
            else
            {
                title.Append($"{entity.FirstName} ");
                if (entity.LastName is not null)
                    title.Append(entity.LastName);
            }

            Dictionary<string, string> bodyParmas = new()
            {
                { "UserName", title.ToString() },
                { "Password", newPassword }
            };

            Dictionary<string, string> subjectParmas = new()
            {
                { "UserName", title.ToString() },
            };

            await _emailService.SendEmail(mailAddresses, "Reminder", true, culture, new MailAddress(_configuration["AppEmail"] ?? throw new Exception("Nie ustawinono emaila aplikacji")), bodyParmas, subjectParmas); ;
        }

        public async Task<int> Create(CreateUserRequest request)
        {
            CreateUserModel model = new()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                //Street = request.Street,
                //City = request.City,
                //CountryId = request.CountryId,
                //CultureId = request.CountryId,
                //PostalCode = request.PostalCode
            };

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            model.PasswordHash = passwordHash;
            model.PasswordSalt = passwordSalt;

            using var transaction = _databaseService.GetTransactionScope();
            int userId = await _databaseService.GetValue<int>(Sql.User_User_Create, parameters: model);

            Models.UserXRole.GetRoleDto role = await _userXRoleService.GetRoleByStrongName(UserRoleEnum.User.ToString())
                ?? throw new Exception("Role not found");

            await _userXRoleService.AddRole(role.RoleId, userId);

            transaction.Complete();

            return userId;
        }

        public async Task Update(int id, UpdateUserRequest request)
        {
            UserModel entity = await Get(id)
                ?? throw new Exception("User not found");

            await _databaseService.Update(Sql.User_User_Update, parameters: [request, new { UserId = id }]);
        }

        public async Task Activate(int userId, string activateCode)
        {
            await _databaseService.Update(Sql.User_User_Activate, parameters: new { UserId = userId });
        }

        public async Task<UserModel?> Get(int userId)
        {
            return await _databaseService.Get<UserModel>(Sql.User_User_Get, parameters: new { UserId = userId });
        }

        public async Task<UserModel?> GetByEmail(string email)
        {
            return await _databaseService.Get<UserModel>(Sql.User_User_GetByEmail, parameters: new { Email = email });
        }

        public async Task<GetUserResponse> Profile(int userId)
        {
            UserModel entity = await _databaseService.Get<UserModel>(Sql.User_User_Get, parameters: new { UserId = userId })
                ?? throw new Exception("User not found");

            return new GetUserResponse()
            {
                Id = entity.Id,
                Email = entity.Email,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                City = entity.City,
                Country = entity.Country,
                PostalCode = entity.PostalCode,
                Street = entity.Street,
            };
        }

        public async Task Block(int userId, DateTime? blockTo)
        {
            await _databaseService.Update(Sql.User_User_Block, parameters: new { UserId = userId, blockTo });
        }

        public async Task Unblock(int userId)
        {
            await _databaseService.Update(Sql.User_User_Unblock, parameters: new { UserId = userId });
        }
    }
}
