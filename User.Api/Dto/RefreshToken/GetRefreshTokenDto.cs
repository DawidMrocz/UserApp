namespace User.Api.Dto.RefreshToken
{
    public class GetRefreshTokenDto
    {
        public string JwtTokenValue { get; set; } = null!;
        public int UserId { get; set; }
        public string RefreshTokenValue { get; set; } = null!;
        public int Id { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
