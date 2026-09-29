using Common.Models.Role;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Common.Models.User
{
    public class UserModel
    {
        public int Id { get; set; }
        [EmailAddress]
        public string Email { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public List<RoleModel> Roles { get; set; } = [];
        [JsonIgnore]
        public byte[] PasswordHash { get; set; } = null!;
        [JsonIgnore]
        public byte[] PasswordSalt { get; set; } = null!;
        public bool? Blocked { get; set; } = false;
        public bool Activated { get; set; } = false;
        public DateTime? UnblockTime { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public string? Culture { get; set; }

        public string RolesJson
        {
            set { Roles = JsonConvert.DeserializeObject<List<RoleModel>>(value) ?? []; }
        }
    }
}
