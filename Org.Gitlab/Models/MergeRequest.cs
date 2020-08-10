using System.Text.Json.Serialization;

namespace Org.Gitlab.Models
{
    public class MergeRequest
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("sha")]
        public string CommitId { get; set; }
    }
    
}