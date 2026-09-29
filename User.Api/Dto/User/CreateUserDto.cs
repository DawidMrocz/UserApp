using System.Text.Json.Serialization;

namespace User.Api.Dto.User
{
    public class CreateUserDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        [JsonIgnore]
        public byte[] PasswordHash { get; set; } = null!;
        [JsonIgnore]
        public byte[] PasswordSalt { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Street { get; set; }
        public string? City { get; set; }
        public int? CountryId { get; set; }
        public int? CultureId { get; set; }
        public string? PostalCode { get; set; }
    }
}
