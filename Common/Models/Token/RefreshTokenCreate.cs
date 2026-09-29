namespace Common.Models.Token
{
    public class RefreshTokenCreate
    {
        public string JwtTokenValue { get; set; } = null!;
        public int UserId { get; set; }
        public string RefreshTokenValue { get; set; } = null!;
        public DateTime ExpireDate { get; set; }
    }
}
