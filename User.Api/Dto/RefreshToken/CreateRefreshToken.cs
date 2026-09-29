namespace User.Api.Dto.RefreshToken
{
    public class CreateRefreshToken
    {
        public string JwtTokenValue { get; set; } = null!;
        public string RefreshTokenValue { get; set; } = null!;
        public DateTime ExpireDate { get; set; }
        public int UserId { get; set; }
    }
}
