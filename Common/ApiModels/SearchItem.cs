using Newtonsoft.Json;

namespace Common.ApiModels
{
    public class SearchItem
    {
        [JsonIgnore]
        public int TotalRows { get; set; }
    }
}
