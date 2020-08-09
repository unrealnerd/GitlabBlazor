using System.Text.Json.Serialization;

namespace Org.Gitlab.Models
{
    public class Job
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
    
}