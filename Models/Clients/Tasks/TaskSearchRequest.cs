using Common.ApiModels;
using Common.Attributes.Validation;
using System.ComponentModel.DataAnnotations;

namespace Models.Clients.Tasks
{
    [TrimAllStrings]
    public class TaskSearchRequest : SearchRequest , IValidatableObject
    {
        public string? Title { get; set; }
        public string? StatusStrongName { get; set; }
        public DateTime? DateTo { get; set; }
        public DateTime? DateFrom { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateTo < DateFrom)
            {
                yield return new ValidationResult(
                    $"Data OD nie może być większa niż data DO",
                    [nameof(DateTo), nameof(DateFrom)]);
            }
        }
    }
}
