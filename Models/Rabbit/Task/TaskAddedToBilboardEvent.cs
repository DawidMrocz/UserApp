namespace Models.Rabbit.Task
{
    public class TaskAddedToBilboardEvent
    {
        public int TaskId { get; set; }
        public int UserId { get; set; }
    }
}
