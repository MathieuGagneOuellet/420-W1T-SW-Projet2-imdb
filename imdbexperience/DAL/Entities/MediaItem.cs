using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace imdbexperience.DAL.Entities
{
    public class MediaItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)] //je veux garder mes id "tt00.." tels quel
        public string Id { get; set; } = string.Empty;
        
        [BsonElement("titre")]
        public string Titre { get; set; } = string.Empty;

        [BsonElement("type")]
        public string Type { get; set; } = string.Empty;
        [BsonElement("annee")]
        public int Annee { get; set; }

        [BsonElement("duree")]
        public int Duree { get; set; }

        [BsonElement("genres")]
        public List<string> Genres { get; set; } = new();
    }
}