using System.ComponentModel.DataAnnotations;

namespace Models.Rabbit.Task
{
    public class TaskCreatedEvent
    {
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = null!;
        public int StatusExternalId { get; set; }
        public DateTime? Deadline { get; set; }
        public bool IsImportant { get; set; }
        public decimal? EstimatedHours { get; set; }
    }
}
