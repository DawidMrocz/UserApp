namespace Models.Rabbit.Task
{
    public class TaskUpdatedEvent
    {
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = null!;
        public DateTime? Deadline { get; set; }
        public bool IsImportant { get; set; }
        public decimal? EstimatedHours { get; set; }
    }
}
