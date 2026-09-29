namespace User.Api.Dto.JwtToken
{
    public class JwtTokenDto
    {
        public string Value { get; set; } = null!;
        public DateTime Expire { get; set; }
    }
}
