using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class UserDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("azureAdId")]
        public string? azureAdId { get; set; }

        [BsonElement("email")]
        public string? email { get; set; }

        [BsonElement("roles")]
        public List<string>? roles { get; set; }

        [BsonElement("createdAt")]
        public DateTime? createdAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime? updatedAt { get; set; }
    }
}
