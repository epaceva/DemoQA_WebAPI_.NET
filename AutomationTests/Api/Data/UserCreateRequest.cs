using System.Text.Json.Serialization;

namespace AutomationTests.Api.Data
{
    public class UserCreateRequest
    {
        // Maps to JSON property "name".
        [JsonPropertyName("name")]
        public string Name { get; set; }

        // Maps to JSON property "job".
        [JsonPropertyName("job")]
        public string Job { get; set; }
    }
}