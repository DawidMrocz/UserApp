using Newtonsoft.Json;
using User.Api.Dto.UserXRole;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace User.Api.Dto.User
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Password { get; set; } = null!;
        [JsonIgnore]
        public byte[] PasswordHash { get; set; } = null!;
        [JsonIgnore]
        public byte[] PasswordSalt { get; set; } = null!;
        public List<GetRoleDto> Roles { get; set; } = [];

        [JsonIgnore]
        public string RolesJson
        {
            set
            {
                Roles = JsonConvert.DeserializeObject<List<GetRoleDto>>(value) ?? [];
            }
        }
    }
}
