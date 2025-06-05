using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace imdbexperience.DAL.Entities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] //id donnés automatiquement par Mongo
        [JsonPropertyName("_id")]
        public string Id { get; set; } = string.Empty;

        [BsonElement("username")]
        public string Username { get; set; } = string.Empty;

        [BsonElement("password")]
        public string Password { get; set; } = string.Empty;        

    }
}
