namespace Bilboard.Api.DTO.Tasks
{
    public class TaskUpdateDto
    {
        public string Title { get; set; } = null!;
        public DateTime? Deadline { get; set; }
        public bool IsImportant { get; set; }
        public decimal? EstimatedHours { get; set; }
        public int ExternalId { get; set; }
        public int UserId { get; set; }
    }
}
