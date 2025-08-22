using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
namespace Models
{
    public class FirewallRequestDto
    {
        //[BsonId] 
        //[BsonRepresentation(BsonType.ObjectId)]
        //[JsonPropertyName("id")]
        //public string? Id { get; set; }

        [BsonId]
        [JsonIgnore]
        public ObjectId _id { get; set; }

        [BsonIgnore]
        [JsonPropertyName("id")]
        public string Id => _id.ToString();
        public int? PStatusId { get; set; }
        public long? FirewallId { get; set; }
        public string? PublicIp { get; set; } = string.Empty;
        public string? Duration { get; set; } = string.Empty; 
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public string? RequestedBy { get; set; } = string.Empty;
        // public string? Status { get; set; } = "Pending"; 
        public string? Status { get; set; } = "Approved";
    }
}
