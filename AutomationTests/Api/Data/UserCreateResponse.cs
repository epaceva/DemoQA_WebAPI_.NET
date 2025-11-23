using System.Text.Json.Serialization;

namespace AutomationTests.Api.Data
{
    public class UserCreateResponse
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("job")]
        public string Job { get; set; }

        [JsonPropertyName("id")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)] 
        public int? Id { get; set; }

        [JsonPropertyName("createdAt")]
        public string CreatedAt { get; set; }
    }
}