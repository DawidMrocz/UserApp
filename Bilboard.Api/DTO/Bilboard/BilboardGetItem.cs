using Models.Clients.Bilboards;
using Newtonsoft.Json;

namespace Bilboard.Api.DTO.Bilboard
{
    public class BilboardGetItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<BilboardItemItem> BilboardItems { get; set; } = [];

        [JsonIgnore]
        public string BilboardItemsJson
        {
            set
            {
                BilboardItems = JsonConvert.DeserializeObject<List<BilboardItemItem>>(value) ?? [];
            }
        }
    }
}
