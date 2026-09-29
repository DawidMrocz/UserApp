namespace Common.Models.OTP
{
    public class OneTimePasscodeModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Passcode { get; set; } = null!;
        public DateTime ExpireDate { get; set; }
        public Guid Guid { get; set; }
        public bool IsAuthTokenPersistent { get; set; }
    }
}
