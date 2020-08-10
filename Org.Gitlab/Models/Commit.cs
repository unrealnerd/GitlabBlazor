using System.Text.Json.Serialization;

namespace Org.Gitlab.Models
{
    public class Commit
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
    
}