using System.Text.Json.Serialization;

namespace Template.Aws.Lambda.Application.Models
{
    public class GetClientResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("phone")]
        public string? phone { get; set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; }

    }
}
