using Common.Attributes.Validation;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Common.ApiModels.User
{
    [TrimAllStrings]
    public class CreateUserRequest : IValidatableObject
    {
        [Required(ErrorMessage = "Login jest wymagany")]
        [EmailAddress(ErrorMessage = "Nie prawidłowy e-mail")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [PasswordValidation]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = "Hasło jest wymagane")]
        [PasswordValidation]
        public string ConfirmPassword { get; set; } = null!;
        //public string? Street { get; set; }
        //public string? City { get; set; }
        //public int? CountryId { get; set; }
        //public int? CultureId { get; set; }
        //public string? PostalCode { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Password != ConfirmPassword)
            {
                yield return new ValidationResult(
                    $"Hasła się nie zgadzaja",
                    [nameof(Password), nameof(ConfirmPassword)]);
            }
        }
    }
}
