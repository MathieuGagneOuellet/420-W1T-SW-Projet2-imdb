using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace imdbexperience.DAL.Entities
{
    public class MediaStatus
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("_id")]
        public string Id { get; set; } = string.Empty;

        [BsonElement("userId")]
        public string UserId { get; set; } = string.Empty;

        [BsonElement("mediaId")]
        public string MediaId { get; set; } = string.Empty;

        [BsonElement("status")]
        public string Status { get; set; } = string.Empty; //va être soit "Seen", "Watchlist" ou "Favorite"
    }
}
