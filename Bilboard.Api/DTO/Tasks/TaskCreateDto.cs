namespace Bilboard.Api.DTO.Tasks
{
    public class TaskCreateDto
    {
        public string Title { get; set; } = null!;
        public int UserId { get; set; }
        public DateTime? Deadline { get; set; }
        public bool IsImportant { get; set; }
        public decimal? EstimatedHours { get; set; }
        public int StatusExternalId { get; set; }
        public int ExternalId { get; set; }
    }
}
