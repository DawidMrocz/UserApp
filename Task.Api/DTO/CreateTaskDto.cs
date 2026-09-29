using System.ComponentModel.DataAnnotations;

namespace Task.Api.DTO
{
    public class CreateTaskDto
    {
        [Required(ErrorMessage = "Tytuł zadania jest wymagany")]
        public string Title { get; set; } = null!;
        public DateTime? Deadline { get; set; }
        public bool IsImportant { get; set; }
        public decimal? EstimatedHours { get; set; }
        public int UserId { get; set; }
        public int StatusId { get; set; }
    }
}
