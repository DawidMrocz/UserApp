namespace Models.Rabbit.Task
{
    public class TaskDeletedEvent
    {
        public int TaskId { get; set; }
        public int UserId { get; set; }
    }
}
