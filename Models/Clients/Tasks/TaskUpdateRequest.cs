using System.ComponentModel.DataAnnotations;

namespace Models.Clients.Tasks
{
    public class TaskUpdateRequest : IValidatableObject
    {
        [Required(ErrorMessage = "Tytuł zadania jest wymagany")]
        public string Title { get; set; } = null!;
        public DateTime? Deadline { get; set; }
        public bool IsImportant { get; set; }
        public decimal EstimatedHours { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Deadline != null && Deadline < DateTime.Now)
            {
                yield return new ValidationResult(
                    $"Dedline musi być ustawiony jako przyszła data",
                    [nameof(Deadline)]);
            }
        }
    }
}

