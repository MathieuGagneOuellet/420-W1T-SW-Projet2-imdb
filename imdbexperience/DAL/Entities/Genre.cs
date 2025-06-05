using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace imdbexperience.DAL.Entities
{
    public class Genre
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("_id")]
        public string Id { get; set; } = string.Empty;

        [BsonElement("nom")]
        public string Nom { get; set; } = string.Empty;
    }
}
