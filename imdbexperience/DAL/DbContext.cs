//Je dois adapter mon db context pour travailler avec MongoDB et MongoDB.Driver plutôt que EF Core comme je suis habitué
//Je conserve la méthodologie de séparer la couche de contexte par contre

using MongoDB.Driver;
using imdbexperience.DAL.Entities;
using DotNetEnv;

namespace imdbexperience.DAL
{
    public class AppDbContext
    {
        private readonly IMongoDatabase _database;

        public AppDbContext()
        {
            Env.Load();
            var connectionString = Environment.GetEnvironmentVariable("MONGODB_URI");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new Exception("Problème avec MONGODB_URI");

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("imdb");
        }
        public IMongoCollection<MediaItem> MediaItems => _database.GetCollection<MediaItem>("mediaItems");
        public IMongoCollection<User> Users => _database.GetCollection<User>("users");
        public IMongoCollection<MediaStatus> MediaStatuses => _database.GetCollection<MediaStatus>("mediaStatuses");


    }
}
