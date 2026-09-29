using Models.Enums;

namespace Bilboard.Api.DTO.Tasks
{
    public class TaskGetDto
    {
        public int Id { get; set; }
        public int CreateUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ModifyUserId { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string Title { get; set; } = null!;
        public DateTime? Deadline { get; set; }
        public int? UserId { get; set; }
        public bool IsImportant { get; set; }
        public decimal? EstimatedHours { get; set; }
        public int StatusId { get; set; }
        public int ExternalId { get; set; }
        public TaskStatusEnum Status => Enum.IsDefined(typeof(TaskStatusEnum), StatusId) ? (TaskStatusEnum)StatusId : TaskStatusEnum.Unknown;
    }
}
