using Common.Models.Currency;

namespace Common.Models.Culture
{
    public class CultureModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public byte[]? Icon { get; set; }
        public int CurrencyId { get; set; }
        public CurrencyModel Currency { get; set; } = null!;
    }
}
