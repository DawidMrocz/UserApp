using System.ComponentModel.DataAnnotations;

namespace Common.ApiModels.User
{
    public class UserForgotPassword
    {
        [Required]
        [EmailAddress(ErrorMessage = "Nie poprawny format adresu e-mail")]
        public string Email { get; set; } = null!;
    }
}
