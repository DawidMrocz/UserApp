namespace Models.Clients.Bilboards
{
    public class BilboardGetResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public List<BilboardItemItem> BilboardItems { get; set; } = [];
    }
}
