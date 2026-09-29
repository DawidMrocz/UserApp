using System.ComponentModel.DataAnnotations;

namespace Common.Models.Label
{
    public class LabelModel
    {
        public int Id { get; set; }
        [Required]
        public int CultureId { get; set; }
        [Required]
        public string Key { get; set; } = null!;
        [Required]
        public string Text { get; set; } = null!;
    }
}
