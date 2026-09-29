using Common.Attributes.Validation;
using System.ComponentModel.DataAnnotations;

namespace Common.Models.Token
{
    [TrimAllStrings]
    public class RefreshTokenRequest
    {
        [Required(ErrorMessage = "Refresh token jest wymagany")]
        public string RefreshToken { get; set; } = null!;
    }
}
