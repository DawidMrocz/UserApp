using Common.Attributes.Validation;
using System.ComponentModel.DataAnnotations;

namespace Common.ApiModels.Access.Request
{
    public sealed class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
       // [PasswordValidation]
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; } = true;
    }
}
