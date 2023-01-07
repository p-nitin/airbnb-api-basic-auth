using System.Text.Json.Serialization;

namespace airbnb.api.DataModel
{
    public class QueryFilter
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("property_type")]
        public string? PropertyType { get; set; }
        [JsonPropertyName("accommodates")]
        public int? Accommodates { get; set; }
        [JsonPropertyName("price")]
        public decimal? Price { get; set; }
    }
}
