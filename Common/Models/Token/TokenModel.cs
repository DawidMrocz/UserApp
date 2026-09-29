namespace Common.Models.Token
{
    public class TokenModel
    {
        public int Id { get; set; }
        public string Value { get; set; } = null!;
        public DateTime ExpireDate { get; set; }
        public int UserId { get; set; }
    }
}
