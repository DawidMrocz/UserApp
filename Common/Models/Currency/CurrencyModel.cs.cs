using System.ComponentModel.DataAnnotations;

namespace Common.Models.Currency
{
    public class CurrencyModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Code { get; set; } = null!;
        [Required]
        public decimal Rate { get; set; }
    }
}
