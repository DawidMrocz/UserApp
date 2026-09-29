using System.Text.Json.Serialization;

namespace Common.Models.User
{
    internal class CreateUserModel
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Street { get; set; }
        public string? City { get; set; }
        public int? CountryId { get; set; }
        public int? CultureId { get; set; }
        public string? PostalCode { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [JsonIgnore]
        public byte[] PasswordHash { get; set; } = null!;
        [JsonIgnore]
        public byte[] PasswordSalt { get; set; } = null!;
        public bool? Blocked { get; set; } = false;
        public bool Activated { get; set; } = false;
        public DateTime? UnblockTime { get; set; }
    }
}
