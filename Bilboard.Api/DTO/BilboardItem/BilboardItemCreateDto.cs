namespace Bilboard.Api.DTO.BilboardItem
{
    public class BilboardItemCreateDto
    {
        public int BilboardId { get; set; }
        public int? Hours { get; set; }
        public int TaskId { get; set; }
        public int UserId { get; set; }
    }
}
