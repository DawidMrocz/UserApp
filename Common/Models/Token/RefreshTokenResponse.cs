namespace Common.Models.Token
{
    public class RefreshTokenResponse
    {
        public string JwtToken { get; set; } = null!;
        public DateTime JwtTokenExpire { get; set; }
        public string RefreshToken { get; set; } = null!;
        public DateTime RefreshTokenExpire { get; set; }
    }
}
