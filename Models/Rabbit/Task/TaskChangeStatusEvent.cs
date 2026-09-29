namespace Models.Rabbit.Task
{
    public class TaskChangeStatusEvent
    {
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public string StatusStrongName { get; set; } = null!;
    }
}
