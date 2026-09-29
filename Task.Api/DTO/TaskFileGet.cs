namespace Task.Api.DTO
{
    public class TaskFileGet
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public Guid Guid { get; set; }
        public string FileName { get; set; } = null!;
    }
}
