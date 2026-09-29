namespace Models.Clients.Bilboards
{
    public class BilboardItemItem
    {
        public int BilboardItemId { get; set; }
        public decimal? Hours { get; set; }
        public string Title { get; set; } = null!;
        public DateTime? Deadline { get; set; }
        public string Status { get; set; } = null!;
        public decimal? EstimatedHours { get; set; }
        public bool IsImportant { get; set; }
    }
}
