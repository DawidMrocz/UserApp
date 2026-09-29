namespace Bilboard.Api.DTO.Tasks
{
    public class TaskChangeStatusDto
    {
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public string StatusStrongName { get; set; } = null!;
    }
}
